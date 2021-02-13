using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class UIManager : TickBehaviour
    {
        private const int SettingsScreenIndex = 2;

        [SerializeField] private List<GameObject> _screens = default;

        [Space]

        [SerializeField] private Text _scoreText = default;

        private int _score;

        private int _activeScreenIndex;
        private int _cachedScreenIndex;

        public bool IsOnSettingsScreen => _activeScreenIndex == SettingsScreenIndex;

        public override void Init()
        {
            base.Init();

            _activeScreenIndex = SceneManager.GetActiveScene().buildIndex;
            _cachedScreenIndex = _activeScreenIndex;

            for (int i = 0; i < _screens.Count; i++)
            {
                if (i != _activeScreenIndex)
                {
                    TurnOffScreen(i);
                }
                else
                {
                    TurnOnScreen(i);
                }
            }
        }

        public override void Enable()
        {
            base.Enable();

            SceneManager.sceneLoaded += HandleSceneLoaded;

            _score = 0;
            Score(0);
        }

        public override void Disable()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;

            base.Disable();
        }

        public override void Tick()
        {
            base.Tick();

            if (GameManager.Instance.InputWrapper.IsEscapePressed)
                if (_activeScreenIndex == SettingsScreenIndex)
                {
                    TurnOffSettingsScreen();
                }
                else
                {
                    TurnOnScreen(SettingsScreenIndex);
                }
        }

        public void Score(int value)
        {
            _score += value;
            _scoreText.text = $"SCORE: {_score}";
        }

        public void Restart()
        {
            SceneManager.LoadScene(1);
        }

        public void TurnOnScreen(int index)
        {
            _cachedScreenIndex = _activeScreenIndex;
            _activeScreenIndex = index;

            _screens[index].SetActive(true);
        }

        public void TurnOffScreen(int index)
        {
            _screens[index].SetActive(false);
        }

        public void TurnOffSettingsScreen()
        {
            _activeScreenIndex = _cachedScreenIndex;
            _cachedScreenIndex = SettingsScreenIndex;

            _screens[_cachedScreenIndex].SetActive(false);
            _screens[_activeScreenIndex].SetActive(true);
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            _score = 0;
            Score(0);
        }
    }
}
