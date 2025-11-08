using System;
using System.Text;
using Infrastructure;
using UnityEngine;
using Zenject;

namespace Characters
{
    public class BattleCharacter : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private int _damage;
        [SerializeField] private int _initiative;
        [SerializeField] private bool _isEnemy;
        
        private BaseCharacterStats _characterStats;
        
        
        private BattleTurnsManager _battleTurnsManager;
        
        public void Init(BattleTurnsManager battleTurnsManager)
        {
            _characterStats = new BaseCharacterStats(_health, _damage, _initiative);
            _battleTurnsManager = battleTurnsManager;
        }
        
        public BaseCharacterStats GetStats() => _characterStats;
        public bool IsEnemy() => _isEnemy;

        public void PlayTurn()
        {
            Debug.Log($"PlayTurn {gameObject.name}");
            var oppositeTeam = _battleTurnsManager.GetOppositeTeamCharactersList(_isEnemy);
            
            //Debug
            StringBuilder sb = new StringBuilder();
            sb.Append($"OppositeTeam of count {oppositeTeam.Count} contains: -> ");
            foreach (var character in oppositeTeam)
            {
                sb.Append($" {character.gameObject.name},");
            }
            Debug.Log(sb.ToString());
            //
        }
    }
}
