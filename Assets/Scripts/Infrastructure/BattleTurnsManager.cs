using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Infrastructure
{
    public class BattleTurnsManager
    {
        private SceneLoader _sceneLoader;
        
        
        public UnityEvent<BattleCharacter> TurnStart;
        public UnityEvent TurnEnded;
        public UnityEvent FightEnded;
        
        private List<BattleCharacter> _characters;

        private int _roundCount = 1;

        private int _currentCharacterTurnIndex;

        private bool _isPlaying;
        private bool _isPlayerWin;

        public BattleTurnsManager(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            
            _characters = new List<BattleCharacter>();
            TurnStart = new UnityEvent<BattleCharacter>();
            TurnEnded = new UnityEvent();
            FightEnded = new UnityEvent();
            _isPlaying = true;

            
        }

        public void TurnStarted(BattleCharacter character)
        {
            TurnStart?.Invoke(character);
        }

        public void TurnEndedInvoke()
        {
            if (CheckWinLose())
            {
                FightEnded?.Invoke();

                if (_isPlayerWin)
                {
                    //load next scene
                    _sceneLoader.LoadNextScene();
                }
                else
                {
                    //reload current scene
                    _sceneLoader.ReloadCurrentScene();
                }
                
                return;
            }
            TurnEnded?.Invoke();
        }
        
        public void AddCharacter(BattleCharacter character) => _characters.Add(character);
        
        public void RemoveCharacter(BattleCharacter character)
        {
            _characters.Remove(character);

            CheckWinLose();
        }

        public void InitializeTurnOrder() => _characters = _characters.OrderByDescending(x => x.GetStats().TurnTime).ToList();

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
                _isPlayerWin = true;
                Debug.Log($"At round {_roundCount} characters win");
                return true;
            }
            
            if (_characters.Where(x => !x.IsEnemy()).ToList().Count == 0)
            {
                _isPlaying = false;
                _isPlayerWin = false;
                Debug.Log($"At round {_roundCount} characters lose");
                return true;
            }
            
            return false;
        }
    }
}
