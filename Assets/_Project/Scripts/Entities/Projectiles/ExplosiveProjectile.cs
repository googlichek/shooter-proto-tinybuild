using UnityEngine;

namespace Game.Scripts
{
    public class ExplosiveProjectile : Projectile
    {
        [Space]

        [SerializeField] private BlowUpComponent _blowUpComponent = default;

        public override void OnTriggerEnter(Collider bump)
        {
            if (!layerMask.HasLayer(bump.gameObject.layer))
                return;

            isHit = true;
            projectileRigidbody.velocity = Vector3.zero;
            audioSource.Play();

            _blowUpComponent.Setup(ownerId, damage);
        }

        public override void Init()
        {
            base.Init();

            AttachComponent(_blowUpComponent);
        }

        public override void Dispose()
        {
            DetachComponent(_blowUpComponent);

            base.Dispose();
        }
    }
}
