using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public class WeaponsController : TickComponent
    {
        private const int SimpleGunIndex = 0;
        private const int ExplosiveGunIndex = 1;

        [SerializeField] private Transform _modelRoot = null;

        [Space]

        [SerializeField] private List<BaseWeaponController> _weaponTemplates = new List<BaseWeaponController>();

        private readonly List<BaseWeaponController> _weapons = new List<BaseWeaponController>();

        private CrouchToggleController _toggleController;

        private int _activeWeaponIndex;

        public override void Init()
        {
            _activeWeaponIndex = -1;
            foreach (var weaponTemplate in _weaponTemplates)
            {
                var weapon = Instantiate(weaponTemplate, _modelRoot);
                _weapons.Add(weapon);
            }

            UpdateWeaponSelection(SimpleGunIndex);
        }

        public override void Tick()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (GameManager.Instance.InputWrapper.IsWeaponZeroPressed)
            {
                UpdateWeaponSelection(SimpleGunIndex);
            }

            if (GameManager.Instance.InputWrapper.IsWeaponOnePressed)
            {
                UpdateWeaponSelection(ExplosiveGunIndex);
            }

            if (GameManager.Instance.InputWrapper.IsAttackPressed)
            {
                _weapons[_activeWeaponIndex].Use();
            }
        }

        private void UpdateWeaponSelection(int activeWeaponIndex)
        {
            if (_activeWeaponIndex == activeWeaponIndex)
                return;

            for (var i = 0; i < _weapons.Count; i++)
                _weapons[i].SetSelectedState(i == activeWeaponIndex);

            _activeWeaponIndex = activeWeaponIndex;
        }
    }
}
