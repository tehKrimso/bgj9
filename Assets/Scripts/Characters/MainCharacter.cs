using System;
using System.Collections.Generic;
using InputService;
using UnityEngine;

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

        private IInputService _input;
        
        private Stack<IngredientType> _currentIngredients;

        public void Initialize(IInputService input)
        {
            _input = input;
            _currentIngredients = new Stack<IngredientType>();
            
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
                    
                    character.AddModifiers(_currentIngredients);
                    _currentIngredients.Clear();
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

            if (_currentIngredients.Count < _maxPotionSize)
            {
                _currentIngredients.Push(ingredientType);
                Debug.Log($"Added {ingredientType.ToString()}, potion now contains {_currentIngredients.Count} ingredients");
                return true;
            }
            else
            {
                Debug.Log($"Max potion capacity reached");
            }
            
            return false;
        }
    }
    
}
