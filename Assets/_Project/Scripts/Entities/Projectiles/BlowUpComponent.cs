using UnityEngine;

namespace Game.Scripts
{
    public class BlowUpComponent : TickComponent
    {
        [SerializeField] private LayerMask _layerMask = LayerMask.GetMask();

        [Space]

        [SerializeField] [Range(0, 500)] private int _pushBackForce = 0;

        [Space]

        [SerializeField] [Range(0, 10)] private float _blowUpRadius = 0;

        private int _ownerId;
        private int _damage;

        void OnTriggerEnter(Collider bump)
        {
            if (!_layerMask.HasLayer(bump.gameObject.layer))
                return;

            var damageReciever = bump.gameObject.GetComponent<IHealth>();
            if (damageReciever == null || damageReciever.Id == _ownerId)
                return;

            var distance = Vector3.Distance(transform.position, bump.transform.position);
            var damageRatio = 1 - Mathf.Clamp01(distance / _blowUpRadius);
            var pushBackDirection = (bump.transform.position - transform.position).normalized;

            damageReciever.Damage(_damage * damageRatio);
            damageReciever.Rigidbody.AddForce(_pushBackForce * damageRatio * pushBackDirection, ForceMode.Impulse);
            TryToScore(damageReciever);
        }

        public override void Enable()
        {
            gameObject.SetActive(false);
        }

        public override void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Setup(int ownerId, int damage)
        {
            _ownerId = ownerId;
            _damage = damage;

            gameObject.SetActive(true);
        }

        private void TryToScore(IHealth damageReciever)
        {
            if (damageReciever.Health <= 0)
                GameManager.Instance.UIManager.Score(2);
        }
    }
}
