using UnityEngine;

namespace Game.Scripts
{
    public class BaseWeaponController : TickBehaviour, IWeapon
    {
        [SerializeField] private Transform _bulletStart = default;

        [Space]

        [SerializeField] private AudioSource _audioSource = default;

        [Space]

        [SerializeField] private BaseWeaponModelController _weaponModel = default;

        [Space]

        [SerializeField] private Projectile _regularBullet = default;
        [SerializeField] private Projectile _doublePowerBullet = default;

        [Space]

        [SerializeField] private LayerMask _layerMask = LayerMask.GetMask();

        protected bool isSelected;
        protected bool isInUse;

        private Camera _camera;
        private CrouchToggleController _toggleController;

        public BaseWeaponModelController WeaponModel => _weaponModel;

        public bool IsSelected => isSelected;
        public bool IsInUse => isInUse;

        public override void Init()
        {
            base.Init();

            AttachComponent(_weaponModel);
        }

        public override void Enable()
        {
            _camera = FindObjectOfType<Camera>();
        }
        
        public override void Dispose()
        {
            DetachComponent(_weaponModel);

            base.Dispose();
        }

        public void Setup(CrouchToggleController toggleController)
        {
            _toggleController = toggleController;
        }

        public void SetSelectedState(bool value)
        {
            isSelected = value;

            if (isSelected)
                _weaponModel.SwitchIn();
            else
            {
                _weaponModel.SwitchOut();
            }
        }

        public virtual void Use()
        {
            if (!_weaponModel.IsUseAllowed)
                return;

            var direction = GetBulletDirection();
            var projectile =
                GameManager.Instance.PoolManager.Spawn(
                    _toggleController.IsCrouching
                        ? _doublePowerBullet
                        : _regularBullet,
                    null,
                    _bulletStart.position,
                    Quaternion.identity);
            projectile.GameObject.GetComponent<Projectile>().Setup(id, direction);

            _weaponModel.Use();
            _audioSource.Play();
        }

        private Vector3 GetBulletDirection()
        {
            var ray = new Ray(_camera.transform.position, _camera.transform.forward);
            var targetPoint = Physics.Raycast(ray, out var hitInfo, _camera.nearClipPlane, _layerMask) ? hitInfo.point : ray.GetPoint(_camera.nearClipPlane);

            var direction = (targetPoint - _camera.transform.position).normalized;
            return direction;
        }
    }
}
