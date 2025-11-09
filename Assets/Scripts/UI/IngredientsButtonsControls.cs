using System;
using Characters;
using InputService;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class IngredientsButtonsControls : MonoBehaviour
    {
        [Header("Health")]
        public Button HealthButton;
        public Image HealthButtonIconImage;
        [Header("Damage")]
        public Button DamageButton;
        public Image DamageButtonIconImage;
        [Header("Speed")]
        public Button SpeedButton;
        public Image SpeeedButtonIconImage;

        private MainCharacter _character;

        public void Initialize(AvailableIngredients availableIngredientsInBattle, MainCharacter character)
        {
            _character = character;

            if (availableIngredientsInBattle.IsHealAvaliable)
            {
                HealthButton.interactable = true;
                HealthButtonIconImage.enabled = true;
                HealthButton.onClick.AddListener(HandleHealthButtonClick);
            }
            else
            {
                HealthButton.interactable = false;
                HealthButtonIconImage.enabled = false;
            }
            
            if (availableIngredientsInBattle.IsDamageAvaliable)
            {
                DamageButton.interactable = true;
                DamageButtonIconImage.enabled = true;
                DamageButton.onClick.AddListener(HandleDamageButtonClick);
            }
            else
            {
                DamageButton.interactable = false;
                DamageButtonIconImage.enabled = false;
            }
            
            if (availableIngredientsInBattle.IsSpeedAvaliable)
            {
                SpeedButton.interactable = true;
                SpeeedButtonIconImage.enabled = true;
                SpeedButton.onClick.AddListener(HandleSpeedButtonClick);
            }
            else
            {
                SpeedButton.interactable = false;
                SpeeedButtonIconImage.enabled = false;
            }
        }

        public void HandleHealthButtonClick()
        {
            _character.TryAddIngredientFromUiButtons(IngredientType.Health);
        }
        
        public void HandleDamageButtonClick()
        {
            _character.TryAddIngredientFromUiButtons(IngredientType.Damage);
        }
        
        public void HandleSpeedButtonClick()
        {
            _character.TryAddIngredientFromUiButtons(IngredientType.Speed);
        }

        private void OnDestroy()
        {
            HealthButton.onClick.RemoveAllListeners();
            DamageButton.onClick.RemoveAllListeners();
            SpeedButton.onClick.RemoveAllListeners();
        }
    }
}
