using UnityEngine;

namespace Game.Scripts
{
    public interface IDamageDealer
    {
        int OwnerId { get; }
        int Damage { get; }

        Vector3 PushBackForce { get; }
    }
}
