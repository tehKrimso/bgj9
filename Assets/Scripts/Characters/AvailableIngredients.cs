using System;
using UnityEngine;

namespace Characters
{
    [Serializable]
    public struct AvailableIngredients
    {
        public bool IsHealAvaliable;
        public bool IsDamageAvaliable;
        public bool IsSpeedAvaliable;
    }
}
