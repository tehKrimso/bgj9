using System.Collections.Generic;
using Characters;
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
        private BattleTurnsManager _battleTurnsManager;
        
        
        public override void InstallBindings()
        {
            Debug.Log("BattleSceneInstaller: InstallBindings start");

            _characterFactory = new CharactersFactory();
            _battleTurnsManager = new BattleTurnsManager();
            
            Container.Bind<ICharacterFactory>().FromInstance(_characterFactory).AsSingle().NonLazy();
            Container.Bind<BattleTurnsManager>().FromInstance(_battleTurnsManager).AsSingle().NonLazy();
            
            SpawnBattleCharacters();
            SpawnEnemyCharacters();
            SpawnMainCharacter(_mainCharacterPrefab, _mainCharacterPos);
            
            _battleTurnsManager.InitializeTurnOrder();
        }

        private void SpawnBattleCharacters()
        {
            for (int i = 0; i < _battleCharactersPrefabs.Count; i++)
            {
               SpawnCharacter(_battleCharactersPrefabs[i], _battleCharactersPosRoot.GetChild(i));
            }
        }
        
        private void SpawnEnemyCharacters()
        {
            for (int i = 0; i < _enemiesPrefabs.Count; i++)
            {
                SpawnCharacter(_enemiesPrefabs[i], _enemiesPosRoot.GetChild(i));
            }
        }
        
        private void SpawnCharacter(GameObject prefab, Transform parent)
        {
            var characterGameObject = _characterFactory.CreateBattleCharacter(prefab, parent);
            
            BattleCharacter battleCharacter = characterGameObject.GetComponent<BattleCharacter>();
            battleCharacter.Init(_battleTurnsManager);
            
            _battleTurnsManager.AddCharacter(battleCharacter);
        }

        private void SpawnMainCharacter(GameObject prefab, Transform parent)
        {
            _characterFactory.CreateMainCharacter(prefab, parent);
        }
        
        
    }
}
