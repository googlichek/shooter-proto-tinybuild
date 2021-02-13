using UnityEngine;

namespace Game.Scripts
{
    public class Projectile : TickBehaviour, IResource, IDamageDealer
    {
        [SerializeField] protected Rigidbody projectileRigidbody = default;
        [SerializeField] protected AudioSource audioSource = default;

        [Space]

        [SerializeField] protected ResourceType type = ResourceType.None;

        [Space]

        [SerializeField] protected LayerMask layerMask = LayerMask.GetMask();

        [Space]

        [SerializeField] [Range(0, 500)] protected int damage = 50;

        [Space]

        [SerializeField] [Range(0, 1000)] protected int launchForce = 500;

        [Space]

        [SerializeField] [Range(0, 10)] protected float totalLifeTime = 5f;

        protected int ownerId;

        protected float creationTime;

        protected bool isHit;

        public GameObject GameObject => gameObject;
        public ResourceType Type => type;
        public bool IsValid => !isHit && creationTime >= Time.time - totalLifeTime;

        public int OwnerId => ownerId;
        public int Damage => damage;

        public int LaunchForce => launchForce;

        public virtual void OnTriggerEnter(Collider bump)
        {
            if (!layerMask.HasLayer(bump.gameObject.layer))
                return;

            isHit = true;
            projectileRigidbody.velocity = Vector3.zero;
            audioSource.Play();

            var damageReciever = bump.gameObject.GetComponent<IHealth>();
            if (damageReciever == null || damageReciever.Id == ownerId)
                return;

            damageReciever.Damage(damage);
            TryToScore(damageReciever);
        }

        public override void Enable()
        {
            base.Enable();

            isHit = false;
            creationTime = Time.time;
            projectileRigidbody.velocity = Vector3.zero;
        }

        public override void Tick()
        {
            base.Tick();

            if (!IsValid && !audioSource.isPlaying)
                GameManager.Instance.PoolManager.Despawn(this);
        }

        public override void Disable()
        {
            isHit = false;
            creationTime = -1;
            projectileRigidbody.velocity = Vector3.zero;

            base.Disable();
        }

        public virtual void Setup(int damagerId, Vector3 direction)
        {
            ownerId = damagerId;

            projectileRigidbody.AddForce(direction * launchForce);
        }

        protected virtual void TryToScore(IHealth damageReciever)
        {
            if (damageReciever.Health <= 0)
                GameManager.Instance.UIManager.Score(1);
        }
    }
}
