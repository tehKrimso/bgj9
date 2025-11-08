using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Infrastructure
{
    public class PotionFactory
    {
        private GameObject _potionPrefab;
        private Transform _mainCharacterPosTransform;

        private const float COLOR_MULTIPLIER = 51f;
        
        public PotionFactory(GameObject potionPrefab, Transform mainCharacterPosTransform)
        {
            _potionPrefab = potionPrefab;
            _mainCharacterPosTransform = mainCharacterPosTransform;
        }
        
        public Potion InstantiatePotion(IngredientType[] potionContent, BattleCharacter target)
        {
            //color here?
            var potionGameObject = GameObject.Instantiate(_potionPrefab, _mainCharacterPosTransform.transform.position, Quaternion.identity);
            
            int redCount = 0; //damage
            int greenCount = 0; //health
            int blueCount = 0; //speed
            int emptyCount = 0; //empty

            foreach (var ingredient in potionContent)
            {
                switch (ingredient)
                {
                    case IngredientType.Damage:
                        redCount++;
                        break;
                    case IngredientType.Health:
                        greenCount++;
                        break;
                    case IngredientType.Speed:
                        blueCount++;
                        break;
                    case IngredientType.Empty:
                    default:
                        emptyCount++;
                        break;
                }
            }
            
            Color potionColor = new Color(redCount * COLOR_MULTIPLIER, greenCount * COLOR_MULTIPLIER, blueCount * COLOR_MULTIPLIER, emptyCount * COLOR_MULTIPLIER);
            
            Potion potion = potionGameObject.GetComponent<Potion>();
            potion.Init(potionContent, target, potionColor);
            
            return potion;
        }
    }
}
