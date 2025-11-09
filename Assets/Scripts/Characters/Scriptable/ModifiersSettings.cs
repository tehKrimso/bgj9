using UnityEngine;

namespace Characters.Scriptable
{
    [CreateAssetMenu(fileName = "ModifiersSettings", menuName = "Scriptable Objects/ModifiersSettings")]

    public class ModifiersSettings : ScriptableObject
    {
        public int HealthModifier;
        public int DamageModifier;
        public float SpeedModifier;
    }
}
