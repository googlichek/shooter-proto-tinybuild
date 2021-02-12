using UnityEngine;

namespace Game.Scripts
{
    public class DataBase : TickBehaviour
    {
        [SerializeField] private SettingsScriptableObject _settings = default;

        public SettingsScriptableObject Settings => _settings;

        public override void Init()
        {
            base.Init();

            _settings.Init();
        }
    }
}
