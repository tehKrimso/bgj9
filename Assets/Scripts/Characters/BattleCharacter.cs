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
        [SerializeField] private int _initiative;
        
        
        
        [Header("Additional parameters")]
        [SerializeField] private bool _isEnemy;
        [SerializeField] private BattleStrategie _battleStrategie;
        [SerializeField] private int _emptyBottleDamage = 2;
        
        [Header("Attack View Params")]
        [SerializeField] private bool _meleeAttack;
        [SerializeField] private float _enemieAttackPosXOffset;
        [SerializeField] private float _movementSpeed;
        
        private Vector3 _initialPosition;
        

        private BattleTurnsManager _battleTurnsManager;
        
        private BaseCharacterStats _characterStats;
        private BaseCharacterStats _characterActiveModifiers;
        private BaseCharacterStats _modifiersTable;

        private BattleCharacter _focusedTarget;
        
        private Random _random;
        
        public void Init(BattleTurnsManager battleTurnsManager, BaseCharacterStats modifiersTable)
        {
            _characterStats = new BaseCharacterStats(_health, _damage, _initiative);
            _characterActiveModifiers = new BaseCharacterStats(0, 0, 0);
            _modifiersTable = modifiersTable;
            
            _battleTurnsManager = battleTurnsManager;
            _random = new Random();
            
            _initialPosition = transform.position;
        }
        
        public BaseCharacterStats GetStats() => _characterStats;
        public bool IsEnemy() => _isEnemy;

        public void PlayTurn()
        {
            Debug.Log($"PlayTurn {gameObject.name}");
            List<BattleCharacter> oppositeTeam = _battleTurnsManager.GetOppositeTeamCharactersList(_isEnemy);
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
                // MoveToAttackPos(enemy.GetAttackPosition());
                // PerformAttack(enemy);
                // MoveToInitialPos();
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
                _battleTurnsManager.RemoveCharacter(this);
                
                //
                Destroy(gameObject);
                //
            }
        }

        public void AddModifiers(IngredientType[] modifiers)
        {
            Debug.Log($"Before {gameObject.name} has {_characterStats.Damage}+{_characterActiveModifiers.Damage} damage, {_characterStats.Health} health, {_characterStats.Initiative}+{_characterActiveModifiers.Initiative} initiative");

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
                        _characterActiveModifiers.Initiative += _modifiersTable.Initiative;
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
            
            Debug.Log($"After {gameObject.name} has {_characterStats.Damage}+{_characterActiveModifiers.Damage} damage, {_characterStats.Health} health, {_characterStats.Initiative}+{_characterActiveModifiers.Initiative} initiative");
        }
        
        public Vector3 GetAttackPosition() => transform.position + new Vector3(_enemieAttackPosXOffset, 0, 0);

        private void MoveToAttackPos(Vector3 targetPosition)
        {
            Coroutine moveToAttackPos;
            moveToAttackPos = StartCoroutine(MoveTo(targetPosition));
        }
        
        private IEnumerator PerformAttack(BattleCharacter target)
        {
            
            yield return new WaitForSeconds(2f); //TODO temp waiting
            
            target.TakeDamage(_characterStats.Damage + _characterActiveModifiers.Damage);
            
            //animation
            //vfx
            
            Debug.Log($"Select {target.gameObject.name} to attack, enemy hp left: {target.GetStats().Health}");
        }
        
        private void MoveToInitialPos()
        {
            StartCoroutine(MoveTo(_initialPosition));
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
            yield return StartCoroutine(PerformAttack(target));
            yield return StartCoroutine(MoveTo(_initialPosition));
        }

        private void ClearModifiers()
        {
            _characterActiveModifiers.Damage = 0;
            _characterActiveModifiers.Health = 0;
            _characterActiveModifiers.Initiative = 0;
            
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
    }


    public enum BattleStrategie
    {
        Random = 0,
        HitLowestHealth = 1,
        HitHighestHealth = 2,
        FocusOnTarget = 3,
    }
}
