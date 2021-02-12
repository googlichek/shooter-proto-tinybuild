using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class SettingsScreenManager : TickBehaviour
    {
        [SerializeField] private Text _walkHeader = default;
        [SerializeField] private Text _crouchHeader = default;

        [Space]

        [SerializeField] private Slider _walkSpeedSlider = default;
        [SerializeField] private Slider _crouchSpeedSlider = default;

        public override void Enable()
        {
            var walkSpeed = GameManager.Instance.DataBase.Settings.Data.WalkSpeed;
            var crouchSpeed = GameManager.Instance.DataBase.Settings.Data.CrouchSpeed;

            _walkSpeedSlider.value = walkSpeed;
            _crouchSpeedSlider.value = crouchSpeed;

            UpdateHeaders();
        }

        public override void Disable()
        {
            GameManager.Instance.DataBase.Settings.SetValues(_walkSpeedSlider.value, _crouchSpeedSlider.value);
        }

        public override void Tick()
        {
            UpdateHeaders();
        }

        private void UpdateHeaders()
        {
            _walkHeader.text = $"WALK SPEED: {_walkSpeedSlider.value}";
            _crouchHeader.text = $"CROUCH SPEED: {_crouchSpeedSlider.value}";
        }
    }
}
