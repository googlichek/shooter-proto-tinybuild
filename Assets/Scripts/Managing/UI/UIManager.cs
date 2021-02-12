using UnityEngine;

namespace Game.Scripts
{
    public class UIManager : TickBehaviour
    {
        [SerializeField] private SettingsScreenManager _settingsScreenManager = default;

        private bool _isSettingsOpened;

        public SettingsScreenManager SettingsScreenManager => _settingsScreenManager;

        public override void Enable()
        {
            base.Enable();

            _isSettingsOpened = false;
        }

        public override void Tick()
        {
            base.Tick();

            if (GameManager.Instance.InputWrapper.IsEscapePressed)
                if (_isSettingsOpened)
                {
                    _isSettingsOpened = false;
                    _settingsScreenManager.gameObject.SetActive(false);
                }
                else
                {
                    _isSettingsOpened = true;
                    _settingsScreenManager.gameObject.SetActive(true);
                }
        }
    }
}
