using UnityEngine;

namespace Characters
{
    public class PotViewController : MonoBehaviour
    {
        public PotViewControls[] Pots;
        //0 - health
        //1 - damage
        //2 - speed

        public void Initialize(AvailableIngredients availableIngredientsInBattle)
        {
            if (availableIngredientsInBattle.IsHealAvaliable)
            {
                Pots[0].SetFullPot();
            }
            else
            {
                Pots[0].SetEmptyPot();
            }
            
            if (availableIngredientsInBattle.IsDamageAvaliable)
            {
                Pots[1].SetFullPot();
            }
            else
            {
                Pots[1].SetEmptyPot();
            }
            
            if (availableIngredientsInBattle.IsSpeedAvaliable)
            {
                Pots[2].SetFullPot();
            }
            else
            {
                Pots[2].SetEmptyPot();
            }
        }
    }
}
