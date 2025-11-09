using System;
using UnityEngine.Serialization;

namespace Characters
{
    [Serializable]
    public class BaseCharacterStats
    {
        public int Health;
        public int Damage;
        public float TurnTime;
        
        public float HealthPercent => (float)Health / (float)_maxHealth;

        private int _maxHealth;

        public BaseCharacterStats(int health, int damage, float turnTime)
        {
            Health = health;
            Damage = damage;
            TurnTime = turnTime;
            _maxHealth = health;
        }

        public void SubtractHealth(int amount)
        {
            Health -= amount;
        }
    }
}
