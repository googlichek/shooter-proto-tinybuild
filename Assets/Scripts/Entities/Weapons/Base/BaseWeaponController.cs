using UnityEngine;

namespace Game.Scripts
{
    public class BaseWeaponController : TickBehaviour, IWeapon
    {
        [SerializeField] private AudioSource _audioSource = default;

        [Space]

        [SerializeField] private BaseWeaponModelController _weaponModel = default;

        protected bool isSelected;
        protected bool isInUse;

        public BaseWeaponModelController WeaponModel => _weaponModel;

        public bool IsSelected => isSelected;
        public bool IsInUse => isInUse;

        public override void Init()
        {
            base.Init();

            AttachComponent(_weaponModel);
        }
        
        public override void Dispose()
        {
            DetachComponent(_weaponModel);

            base.Dispose();
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

            _weaponModel.Use();
            _audioSource.Play();
        }
    }
}
