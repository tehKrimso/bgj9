using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Infrastructure;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

namespace Characters
{
    public class BattleCharacter : MonoBehaviour
    {
        [Header("Initial stats")]
        [SerializeField] private int _health;
        [SerializeField] private int _damage;
        [SerializeField] private float _timeBetweenTurns;
        
        
        
        [Header("Additional parameters")]
        [SerializeField] private bool _isEnemy;
        [SerializeField] private BattleStrategie _battleStrategie;
        [SerializeField] private int _emptyBottleDamage = 2;
        
        [Header("Attack View Params")]
        [SerializeField] private bool _meleeAttack;
        [SerializeField] private float _enemieAttackPosXOffset;
        [SerializeField] private float _movementSpeed;

        public float CurrentTimeToTurn => _characterStats.TurnTime - _characterActiveModifiers.TurnTime;
        public float TimePercent => _currentTimer / CurrentTimeToTurn;
        
        private Vector3 _initialPosition;
        

        private BattleTurnsManager _battleTurnsManager;
        
        private BaseCharacterStats _characterStats;
        private BaseCharacterStats _characterActiveModifiers;
        private BaseCharacterStats _modifiersTable;

        private BattleCharacterAnimatorControls _animatorControls;

        private BattleCharacter _focusedTarget;
        
        private Random _random;

        private bool _isPaused;
        private float _currentTimer;
        
        public void Init(BattleTurnsManager battleTurnsManager, BaseCharacterStats modifiersTable)
        {
            _characterStats = new BaseCharacterStats(_health, _damage, _timeBetweenTurns);
            _characterActiveModifiers = new BaseCharacterStats(0, 0, 0);
            _modifiersTable = modifiersTable;
            
            _battleTurnsManager = battleTurnsManager;
            _battleTurnsManager.TurnStart.AddListener(HandleTakeTurn);
            _battleTurnsManager.TurnEnded.AddListener(HandleTurnContinue);
            _battleTurnsManager.FightEnded.AddListener(HandleFightEnded);
            
            _animatorControls = GetComponent<BattleCharacterAnimatorControls>();
            
            
            _random = new Random();
            
            _initialPosition = transform.position;
        }

        private void Update()
        {
            if(_isPaused)
                return;
            
            _currentTimer += Time.deltaTime;
            if (_currentTimer >= CurrentTimeToTurn) //Check if current time more than initial turn time minus modifiers
            {
                _currentTimer = 0;
                _battleTurnsManager.TurnStarted(this);
            }
        }

        public BaseCharacterStats GetStats() => _characterStats;
        public bool IsEnemy() => _isEnemy;

        public Vector3 GetAttackPosition() => transform.position + new Vector3(_enemieAttackPosXOffset, 0, 0);

        public void PlayTurn()
        {
            Debug.Log($"PlayTurn {gameObject.name}");
            List<BattleCharacter> oppositeTeam = _battleTurnsManager.GetOppositeTeamCharactersList(_isEnemy);

            if (oppositeTeam.Count == 0)
            {
                _isPaused = true;
                _battleTurnsManager.TurnEndedInvoke();
                return;
            }
            
            var enemy = ChooseEnemyToAttack(oppositeTeam);
            //Debug
            // StringBuilder sb = new StringBuilder();
            // sb.Append($"OppositeTeam of count {oppositeTeam.Count} contains: -> ");
            // foreach (var character in oppositeTeam)
            // {
            //     sb.Append($" {character.gameObject.name},");
            // }
            // Debug.Log(sb.ToString());
            //

            if (_meleeAttack)
            {
                StartCoroutine(AttackSequence(enemy.GetAttackPosition(),enemy));
            }
            else
            {
                PerformAttack(enemy);
                //waiting for projectile to hit? 
            }
            
            ClearModifiers();
            
            Debug.Log("======");
        }

        public void TakeDamage(int damage)
        {
            _characterStats.Health -= damage;

            if (_characterStats.Health <= 0)
            {
                Debug.Log($"{gameObject.name} is dead!");
                
                
                //
                RemoveListeners();
                _isPaused = true;
            
                _battleTurnsManager.RemoveCharacter(this);
                StartCoroutine(HandleDeath());
                //
            }
        }

        public void AddModifiers(IngredientType[] modifiers)
        {
            Debug.Log($"Before {gameObject.name} has {_characterStats.Damage}+{_characterActiveModifiers.Damage} damage, {_characterStats.Health} health, {_characterStats.TurnTime}+{_characterActiveModifiers.TurnTime} initiative");

            int modifiersCount = modifiers.Length;
            int emptySlots = 0;

            
            for (int i = 0; i < modifiersCount; i++)
            {
                IngredientType ingredient = modifiers[i];
                switch (ingredient)
                {
                    case IngredientType.Health:
                        _characterStats.Health = math.min(_health, _characterStats.Health + _modifiersTable.Health);
                        break;
                    case IngredientType.Damage:
                        _characterActiveModifiers.Damage += _modifiersTable.Damage;
                        break;
                    case IngredientType.Speed:
                        _characterActiveModifiers.TurnTime += _modifiersTable.TurnTime;
                        break;
                    case IngredientType.Empty:
                    default:
                        emptySlots++;
                        break;
                }
            }

            if (emptySlots == modifiersCount) //empty bottle
            {
                TakeDamage(_emptyBottleDamage);
            }
            
            Debug.Log($"After {gameObject.name} has {_characterStats.Damage}+{_characterActiveModifiers.Damage} damage, {_characterStats.Health} health, {_characterStats.TurnTime}+{_characterActiveModifiers.TurnTime} initiative");
        }

        private void HandleTakeTurn(BattleCharacter character)
        {
            _isPaused = true;
            
            if (character != this)
            {
                return;
            }
            
            PlayTurn();
            
            
        }

        private void HandleTurnContinue()
        {
            _isPaused = false;
        }

        private void HandleFightEnded()
        {
            _isPaused = true;
            //TODO endgame anim?
        }
        
        private IEnumerator PerformAttack(BattleCharacter target)
        {
            
            //yield return new WaitForSeconds(1f); //TODO temp waiting
            yield return new WaitForSeconds(_animatorControls.AttackWaitingTime); //TODO temp waiting
            
            target.TakeDamage(_characterStats.Damage + _characterActiveModifiers.Damage);
            
            //animation
            //vfx
            
            Debug.Log($"Select {target.gameObject.name} to attack, enemy hp left: {target.GetStats().Health}");
        }

        private IEnumerator MoveTo(Vector3 targetPosition)
        {
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, _movementSpeed * Time.deltaTime);
                yield return null;
            }
        }

        private IEnumerator AttackSequence(Vector3 targetPosition, BattleCharacter target)
        {
            yield return StartCoroutine(MoveTo(targetPosition));
            
            _animatorControls.SetAttackParam(true);
            yield return StartCoroutine(PerformAttack(target));
            _animatorControls.SetAttackParam(false);
            
            yield return StartCoroutine(MoveTo(_initialPosition));
            _battleTurnsManager.TurnEndedInvoke();
            
        }

        private IEnumerator HandleDeath()
        {
            _animatorControls.SetIsDeadParam();

            yield return new WaitForSeconds(_animatorControls.DeathAnimTime);
            
            //Destroy(gameObject);
        }

        private void ClearModifiers()
        {
            _characterActiveModifiers.Damage = 0;
            _characterActiveModifiers.Health = 0;
            _characterActiveModifiers.TurnTime = 0;
            
            Debug.Log("Modificators cleared");
        }

        private BattleCharacter ChooseEnemyToAttack(List<BattleCharacter> oppositeTeam)
        {
            //strategies switch here or interfaces on init?
            switch (_battleStrategie)
            {
                case BattleStrategie.FocusOnTarget:
                {
                    if (_focusedTarget == null || _focusedTarget.GetStats().Health <= 0)
                    {
                        _focusedTarget = oppositeTeam[_random.Next(0, oppositeTeam.Count)];
                    }
                    return _focusedTarget;
                }
                case BattleStrategie.HitLowestHealth:
                    return oppositeTeam.OrderBy(x => x.GetStats().Health).Last();
                case BattleStrategie.HitHighestHealth:
                    return oppositeTeam.OrderBy(x => x.GetStats().Health).First();
                case BattleStrategie.Random:
                default:
                    //var random = new Random();
                    return oppositeTeam[_random.Next(0, oppositeTeam.Count)];
            }
        }

        private void RemoveListeners()
        {
            _battleTurnsManager.TurnStart.RemoveListener(HandleTakeTurn);
            _battleTurnsManager.TurnEnded.RemoveListener(HandleTurnContinue);
            _battleTurnsManager.FightEnded.RemoveListener(HandleFightEnded);
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }
    }


    public enum BattleStrategie
    {
        Random = 0,
        HitLowestHealth = 1,
        HitHighestHealth = 2,
        FocusOnTarget = 3,
    }
}
