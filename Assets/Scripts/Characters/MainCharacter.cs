using System;
using System.Collections.Generic;
using Infrastructure;
using InputService;
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
        private int _currentPotionSize;

        private IInputService _input;
        
        private IngredientType[] _currentIngredients;
        
        private PotionFactory _potionFactory;

        public void Initialize(IInputService input, PotionFactory potionFactory)
        {
            _input = input;
            _currentIngredients = new IngredientType[_maxPotionSize];
            _potionFactory = potionFactory;
            ClearIngredientArray();
            
            //init avaliable ingredients here?
            
        }

        private void Update()
        {
            if (_input.FirstIngredientButton())
            {
                TryAddIngredient(IngredientType.Health);
                return;
            }
            
            if (_input.SecondIngredientButton())
            {
                TryAddIngredient(IngredientType.Damage);
                return;
            }
            
            if (_input.ThirdIngredientButton())
            {
                TryAddIngredient(IngredientType.Speed);
                return;
            }
            
            if (_input.ThrowPotionButton())
            {
                PotionThrow();
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
                    potion.LaunchPotion();
                }
                else
                {
                    Debug.Log($"Hit {hit.collider.name}, NOT BattleCharacter");
                }
            }
        }
        
        private bool TryAddIngredient(IngredientType ingredientType)
        {
            //Debug.Log($"Trying add ingredient {ingredientType.ToString()}");

            if (_currentPotionSize < _maxPotionSize)
            {
                _currentIngredients[_currentPotionSize] = ingredientType;
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
