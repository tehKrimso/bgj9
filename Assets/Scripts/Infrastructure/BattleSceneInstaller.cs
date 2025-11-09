using System.Collections.Generic;
using Characters;
using Characters.Scriptable;
using InputService;
using UI;
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
        [SerializeField] private GameObject _potionPrefab;
        

        [Header("Modifiers Table")] 
        [SerializeField] private ModifiersSettings _modifiers;
        [SerializeField] private AvailableIngredients _availableIngredientsInBattle;
        
        [Space]
        [Header("OtherLinks")]
        [SerializeField] private SlicesUIController _slicesUIController;
        [SerializeField] private IngredientsButtonsControls _ingredientsButtonsControls;
        
        private ICharacterFactory _characterFactory;
        private IInputService _input;
        private BattleTurnsManager _battleTurnsManager;
        private PotionFactory _potionFactory;

        private BaseCharacterStats _modifiersTable;
        
        
        public override void InstallBindings()
        {
            Debug.Log("BattleSceneInstaller: InstallBindings start");

            if (_slicesUIController == null)
            {
                _slicesUIController = FindObjectOfType<SlicesUIController>();
            }
            
            if (_ingredientsButtonsControls == null)
            {
                _ingredientsButtonsControls = FindObjectOfType<IngredientsButtonsControls>();
            }
            
            _modifiersTable = new BaseCharacterStats(_modifiers.HealthModifier, _modifiers.DamageModifier, _modifiers.SpeedModifier);

            _input = new StandaloneInputService();
            _characterFactory = new CharactersFactory();
            _battleTurnsManager = new BattleTurnsManager(Container.Resolve<SceneLoader>());
            _potionFactory = new PotionFactory(_potionPrefab, _mainCharacterPos);
            
            Container.Bind<ICharacterFactory>().FromInstance(_characterFactory).AsSingle().NonLazy();
            Container.Bind<IInputService>().FromInstance(_input).AsSingle().NonLazy();
            Container.Bind<BattleTurnsManager>().FromInstance(_battleTurnsManager).AsSingle().NonLazy();
            Container.Bind<PotionFactory>().FromInstance(_potionFactory).AsSingle().NonLazy();
            
            SpawnBattleCharacters();
            SpawnEnemyCharacters();
            var mainCharacter = SpawnMainCharacter(_mainCharacterPrefab, _mainCharacterPos);
            
            _ingredientsButtonsControls.Initialize(_availableIngredientsInBattle, mainCharacter);
            
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
            battleCharacter.Init(_battleTurnsManager, _modifiersTable);
            
            _battleTurnsManager.AddCharacter(battleCharacter);
        }

        private MainCharacter SpawnMainCharacter(GameObject prefab, Transform parent)
        {
            var mainCharacterGameObject = _characterFactory.CreateMainCharacter(prefab, parent);
            MainCharacter mainCharacter = mainCharacterGameObject.GetComponent<MainCharacter>();
            mainCharacter.Initialize(_input, _potionFactory, _slicesUIController, _availableIngredientsInBattle);
            
            Container.Bind<MainCharacter>().FromInstance(mainCharacter).AsSingle().NonLazy();
            
            return mainCharacter;
        }
        
        
    }
}
