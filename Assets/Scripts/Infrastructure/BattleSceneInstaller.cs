using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Infrastructure
{
    public class BattleSceneInstaller : MonoInstaller
    {
        [Header("Position Transforms")]
        [SerializeField] private Transform _battleCharactersPosRoot;
        [SerializeField] private Transform _enemiesPosRoot;
        [SerializeField] private Transform _mainCharacterPos;
        
        [Header("Prefabs")]
        [SerializeField] private List<GameObject> _battleCharactersPrefabs;
        [SerializeField] private List<GameObject> _enemiesPrefabs;
        [SerializeField] private GameObject _mainCharacterPrefab;
        
        private ICharacterFactory _characterFactory;
        
        
        public override void InstallBindings()
        {
            Debug.Log("BattleSceneInstaller: InstallBindings start");

            _characterFactory = new CharactersFactory();
            
            Container.Bind<ICharacterFactory>().FromInstance(_characterFactory).AsSingle().NonLazy();
            
            SpawnBattleCharacters();
            SpawnEnemyCharacters();
            SpawnMainCharacter();
        }

        private void SpawnBattleCharacters()
        {
            for (int i = 0; i < _battleCharactersPrefabs.Count; i++)
            {
                _characterFactory.CreateBattleCharacter(_battleCharactersPrefabs[i], _battleCharactersPosRoot.GetChild(i));
            }
        }
        
        private void SpawnEnemyCharacters()
        {
            for (int i = 0; i < _enemiesPrefabs.Count; i++)
            {
                _characterFactory.CreateBattleCharacter(_enemiesPrefabs[i], _enemiesPosRoot.GetChild(i));
            }
        }
        
        private void SpawnMainCharacter()
        {
            _characterFactory.CreateBattleCharacter(_mainCharacterPrefab, _mainCharacterPos);
        }
    }
}
