using UnityEngine;

namespace Game.Scripts
{
    public class HealthController : TickBehaviour, IHealth
    {
        [SerializeField] private Rigidbody _rigidbody = default;

        [Space]

        [SerializeField] [Range(0, 1000)] private float _maxHealth = 150;

        private float _health;

        public Rigidbody Rigidbody => _rigidbody;

        public float MaxHealth => _maxHealth;
        public float Health => _health;

        public override void Enable()
        {
            _health = _maxHealth;
        }

        public override void Tick()
        {
            base.Tick();

            if (_health > 0)
                return;

            Destroy(gameObject);
        }

        public void Damage(float value)
        {
            _health = Mathf.Clamp(_health - value, 0, _maxHealth);
        }
    }
}
