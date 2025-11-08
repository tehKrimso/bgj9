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

        private bool _isPlaying;

        public BattleTurnsManager()
        {
            _characters = new List<BattleCharacter>();
            _isPlaying = true;
        }
        
        public void AddCharacter(BattleCharacter character) => _characters.Add(character);
        
        public void RemoveCharacter(BattleCharacter character)
        {
            _characters.Remove(character);

            CheckWinLose();
        }

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
            if(!_isPlaying)
                return;
            
            if (_currentCharacterTurnIndex >= _characters.Count)
            {
                _currentCharacterTurnIndex = 0;
                _roundCount++;

                InitializeTurnOrder(); //to change order if someone boosted/slowed
                
                Debug.Log($"Round {_roundCount}");
            }
            
            _characters[_currentCharacterTurnIndex].PlayTurn();
            _currentCharacterTurnIndex++;
        }

        private bool CheckWinLose()
        {
            if (_characters.Where(x => x.IsEnemy()).ToList().Count == 0)
            {
                _isPlaying = false;
                Debug.Log($"At round {_roundCount} characters win");
                return true;
            }
            
            if (_characters.Where(x => !x.IsEnemy()).ToList().Count == 0)
            {
                _isPlaying = false;
                Debug.Log($"At round {_roundCount} characters lose");
                return true;
            }
            
            return false;
        }
    }
}
