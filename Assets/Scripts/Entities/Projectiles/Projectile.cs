
using UnityEngine;

namespace Game.Scripts
{
    public class Projectile : TickBehaviour, IResource, IDamageDealer
    {
        [SerializeField] private ResourceType _type = ResourceType.None;

        [Space]

        [SerializeField] private LayerMask _layerMask = LayerMask.GetMask();

        [Space]

        [SerializeField] [Range(0, 500)] private int _damage = 50;

        [Space]

        [SerializeField] [Range(0, 1000)] private int _launchForce = 500;

        [SerializeField] [Range(0, 50)] private int _pushBackForceX = 0;
        [SerializeField] [Range(0, 50)] private int _pushBackForceY = 0;
        [SerializeField] [Range(0, 50)] private int _pushBackForceZ = 0;

        [Space]

        [SerializeField] [Range(0, 10)] private float _totalLifeTime = 5f;
        [SerializeField] [Range(0, 10)] private float _nextSpawnDelay = 0.25f;

        private Rigidbody _rigidbody;
        private AudioSource _audioSource;

        private Vector3 _pushBackForce;

        private int _ownerId;

        private float _creationTime;

        private bool _isHit;

        public GameObject GameObject => gameObject;
        public ResourceType Type => _type;
        public bool IsValid => !_isHit && _creationTime >= Time.time - _totalLifeTime;

        public int OwnerId => _ownerId;
        public int Damage => _damage;
        public Vector3 PushBackForce { get; }

        public int LaunchForce => _launchForce;
        public float NextSpawnDelay => _nextSpawnDelay;

        void OnTriggerEnter(Collider bump)
        {
            if (!_layerMask.HasLayer(bump.gameObject.layer))
                return;

            _isHit = true;
            _rigidbody.velocity = Vector3.zero;
            _audioSource.Play();

            var damageReciever = bump.gameObject.GetComponent<IHealth>();
            if (damageReciever == null || damageReciever.Id == _ownerId)
                return;

            damageReciever.Damage(Damage);
        }

        public override void Init()
        {
            base.Init();

            _rigidbody = GetComponent<Rigidbody>();
            _audioSource = GetComponent<AudioSource>();
        }

        public override void Enable()
        {
            base.Enable();

            _isHit = false;
            _creationTime = Time.time;
            _rigidbody.velocity = Vector3.zero;

            _pushBackForce = new Vector3(_pushBackForceX, _pushBackForceY, _pushBackForceZ);
        }

        public override void Tick()
        {
            base.Tick();

            if (!IsValid)
                GameManager.Instance.PoolManager.Despawn(this);
        }

        public override void Disable()
        {
            _isHit = false;
            _creationTime = -1;
            _rigidbody.velocity = Vector3.zero;

            base.Disable();
        }

        public virtual void Setup(int damagerId, Vector3 direction)
        {
            _ownerId = damagerId;

            _rigidbody.AddForce(direction * _launchForce);
        }
    }
}
