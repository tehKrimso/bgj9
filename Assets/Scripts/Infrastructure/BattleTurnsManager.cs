using System.Collections.Generic;
using System.Linq;
using Characters;
using UnityEngine;

namespace Infrastructure
{
    public class BattleTurnsManager
    {
        private List<BattleCharacter> _characters;

        private int _roundCount = 1;

        private int _currentCharacterTurnIndex;

        public BattleTurnsManager()
        {
            _characters = new List<BattleCharacter>();
        }
        
        public void AddCharacter(BattleCharacter character) => _characters.Add(character);
        
        public void RemoveCharacter(BattleCharacter character) => _characters.Remove(character); //reorder after removal?
        
        public void InitializeTurnOrder() => _characters = _characters.OrderByDescending(x => x.GetStats().Initiative).ToList();

        public List<BattleCharacter> GetOppositeTeamCharactersList(bool isEnemyAsking)
        {
            if (isEnemyAsking)
            {
                return _characters.Where(x => !x.IsEnemy()).ToList();
            }
            else
            {
                return _characters.Where(x => x.IsEnemy()).ToList();
            }
        }

        public void NextTurn()
        {
            if (_currentCharacterTurnIndex == _characters.Count)
            {
                _currentCharacterTurnIndex = 0;
                _roundCount++;

                InitializeTurnOrder(); //to change order if someone boosted/slowed
                
                Debug.Log($"Round {_roundCount}");
            }
            
            _characters[_currentCharacterTurnIndex].PlayTurn();
            _currentCharacterTurnIndex++;
        }
    }
}
