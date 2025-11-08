using UnityEngine;

namespace InputService
{
    public class StandaloneInputService : IInputService
    {
        private Camera _mainCamera;

        public StandaloneInputService()
        {
            _mainCamera = Camera.main;
        }
        
        
        public bool FirstIngredientButton()
        {
            return Input.GetKeyUp(KeyCode.Alpha1);
        }

        public bool SecondIngredientButton()
        {
            return Input.GetKeyUp(KeyCode.Alpha2);
        }

        public bool ThirdIngredientButton()
        {
            return Input.GetKeyUp(KeyCode.Alpha3);
        }

        public bool ThrowPotionButton()
        {
            return Input.GetKeyUp(KeyCode.Mouse0);
        }

        public Vector2 GetMousePositionOnScreen()
        {
            return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
