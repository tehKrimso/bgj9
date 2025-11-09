using System;
using Characters;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BottleSliceUI : MonoBehaviour
    {
        public GameObject SliceImage;
        public GameObject DmgText;
        public GameObject HPText;
        public GameObject SpeedText;

        private Image _sliceSpriteImage;

        private void Awake()
        {
            _sliceSpriteImage = SliceImage.GetComponent<Image>();
        }

        public void DisableAll()
        {
            SliceImage.gameObject.SetActive(false);
            DmgText.gameObject.SetActive(false);
            HPText.gameObject.SetActive(false);
            SpeedText.gameObject.SetActive(false);
        }

        public void ActivateSlice(IngredientType ingredient, Color sliceColor)
        {
            switch (ingredient)
            {
                case IngredientType.Damage:
                    DmgText.SetActive(true);
                    break;
                case IngredientType.Health:
                    HPText.SetActive(true);
                    break;
                case IngredientType.Speed:
                    SpeedText.SetActive(true);
                    break;
                default:
                    DisableAll();
                    break;
            }
            
            SliceImage.gameObject.SetActive(true);
            _sliceSpriteImage.color = sliceColor;
           
        }
    }
}
