using UnityEngine;

namespace InputService
{
    public interface IInputService
    {
        public bool FirstIngredientButton();
        public bool SecondIngredientButton();
        public bool ThirdIngredientButton();
        public bool ThrowPotionButton();
        
        public Vector2 GetMousePositionOnScreen();
    }
}
