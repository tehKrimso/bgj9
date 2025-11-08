using System;

namespace Characters
{
    [Serializable]
    public class BaseCharacterStats
    {
        public int Health;
        public int Damage;
        public int Initiative;

        private int _maxHealth;

        public BaseCharacterStats(int health, int damage, int initiative)
        {
            Health = health;
            Damage = damage;
            Initiative = initiative;
            _maxHealth = health;
        }

        public void SubtractHealth(int amount)
        {
            Health -= amount;
        }
    }
}
