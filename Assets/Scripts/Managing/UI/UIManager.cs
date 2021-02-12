using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class UIManager : TickBehaviour
    {
        [SerializeField] private SettingsScreenManager _settingsScreenManager = default;

        [Space]

        [SerializeField] private Text _scoreText = default;

        private int _score;

        private bool _isSettingsOpened;

        public override void Enable()
        {
            base.Enable();

            _isSettingsOpened = false;

            _score = 0;
            Score(0);
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

        public void Score(int value)
        {
            _score += value;
            _scoreText.text = $"SCORE: {_score}";
        }
    }
}
