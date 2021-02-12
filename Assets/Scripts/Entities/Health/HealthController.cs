using UnityEngine;

namespace Game.Scripts
{
    public class HealthController : TickComponent, IHealth
    {
        [SerializeField] [Range(0, 1000)] private float _maxHealth = 150;

        private float _health;

        public float MaxHealth => _maxHealth;
        public float Health => _health;

        public override void Enable()
        {
            _health = _maxHealth;
        }

        public void Damage(float value)
        {
            _health = Mathf.Clamp(_health - value, 0, _maxHealth);
        }
    }
}
