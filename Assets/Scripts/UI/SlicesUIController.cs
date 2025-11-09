using Characters;
using UnityEngine;

namespace UI
{
    public class SlicesUIController : MonoBehaviour
    {
        public BottleSliceUI[] BottleSlices;

        public void SetActiveSlice(int index, IngredientType ingredient)
        {
            Color sliceColor = Color.clear;
            
            switch (ingredient)
            {
                case IngredientType.Damage:
                    sliceColor = Color.red;
                    break;
                case IngredientType.Health:
                    sliceColor = Color.green;
                    break;
                case IngredientType.Speed:
                    sliceColor = Color.blue;
                    break;
                case IngredientType.Empty:
                default:
                    break;
            }
            
            BottleSlices[index].ActivateSlice(ingredient, sliceColor);
        }

        public void DisableSlices()
        {
            foreach (var slice in BottleSlices)
            {
                slice.DisableAll();
            }
        }
    }
}
