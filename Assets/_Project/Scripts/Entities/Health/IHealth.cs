using UnityEngine;

namespace Game.Scripts
{
    public interface IHealth
    {
        Rigidbody Rigidbody { get; }

        int Id { get; }

        float MaxHealth { get; }
        float Health { get; }

        void Damage(float value);
    }
}