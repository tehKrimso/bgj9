using System;
using System.Collections.Generic;
using Infrastructure;
using InputService;
using UI;
using UnityEngine;
using Zenject;

namespace Characters
{
    public enum IngredientType
    {
        Empty = 0,
        Health = 1,
        Damage = 2,
        Speed = 3
    }
    public class MainCharacter : MonoBehaviour
    {
        [SerializeField] private int _maxPotionSize;
        private float _throwCoolDown;
        public float TimePercent => _throwTimer / _throwCoolDown;


        private IInputService _input;
        private IngredientType[] _currentIngredients;
        private PotionFactory _potionFactory;
        private SlicesUIController _slicesUIController;
        private AvailableIngredients _availableIngredientsInBattle;

        private int _currentPotionSize;
        private float _throwTimer;

        public void Initialize(IInputService input, PotionFactory potionFactory, SlicesUIController slicesUIController, AvailableIngredients availableIngredientsInBattle, float throwCoolDown)
        {
            _input = input;
            _currentIngredients = new IngredientType[_maxPotionSize];
            _potionFactory = potionFactory;
            _availableIngredientsInBattle = availableIngredientsInBattle;
            
            gameObject.GetComponent<PotViewController>().Initialize(_availableIngredientsInBattle);
            
            _slicesUIController = slicesUIController;
            ClearIngredientArray();
            
            _throwCoolDown = throwCoolDown;
            
            //init avaliable ingredients here?
            
        }

        private void Update()
        {
            _throwTimer += Time.deltaTime;
            
            if (_availableIngredientsInBattle.IsHealAvaliable && _input.FirstIngredientButton())
            {
                TryAddIngredient(IngredientType.Health);
                return;
            }
            
            if (_availableIngredientsInBattle.IsDamageAvaliable && _input.SecondIngredientButton())
            {
                TryAddIngredient(IngredientType.Damage);
                return;
            }
            
            if (_availableIngredientsInBattle.IsSpeedAvaliable && _input.ThirdIngredientButton())
            {
                TryAddIngredient(IngredientType.Speed);
                return;
            }
            
            if (_input.ThrowPotionButton())
            {

                if (_throwTimer >= _throwCoolDown)
                {
                    PotionThrow();
                }
            }
        }

        private void PotionThrow()
        {
            Vector2 throwDestination = _input.GetMousePositionOnScreen();
            RaycastHit2D hit = Physics2D.Raycast(throwDestination, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.TryGetComponent(out BattleCharacter character))
                {
                    //Debug.Log($"Hit {hit.collider.name}");
                    
                    Potion potion = _potionFactory.InstantiatePotion(_currentIngredients, character);
                    
                    ClearIngredientArray();
                    
                    _slicesUIController.DisableSlices();
                    
                    potion.LaunchPotion();
                    
                    _throwTimer = 0;
                }
                else
                {
                    Debug.Log($"Hit {hit.collider.name}, NOT BattleCharacter");
                }
            }
        }

        public bool TryAddIngredientFromUiButtons(IngredientType type)
        {
            return TryAddIngredient(type);
        }
        
        private bool TryAddIngredient(IngredientType ingredientType)
        {
            //Debug.Log($"Trying add ingredient {ingredientType.ToString()}");

            if (_currentPotionSize < _maxPotionSize)
            {
                _currentIngredients[_currentPotionSize] = ingredientType;
                _slicesUIController.SetActiveSlice(_currentPotionSize, ingredientType);
                
                _currentPotionSize++;
                
                Debug.Log($"Added {ingredientType.ToString()}, potion now contains {_currentPotionSize} ingredients");
                return true;
            }
            else
            {
                Debug.Log($"Max potion capacity reached");
            }
            
            return false;
        }

        private void ClearIngredientArray()
        {
            for (int i = 0; i < _currentIngredients.Length; i++)
            {
                _currentIngredients[i] = IngredientType.Empty;
            }

            _currentPotionSize = 0;
        }
    }
    
}
