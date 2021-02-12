using System;
using UnityEngine;

namespace Game.Scripts
{
    [Serializable]
    [CreateAssetMenu(fileName = "Settings", menuName = "Data/Settings")]
    public class SettingsScriptableObject : ScriptableObject
    {
        public Action<SettingsData> OnChanged;

        [SerializeField] [Range(1, 10)] private float _walkSpeed = 0;
        [SerializeField] [Range(1, 10)] private float _crouchSpeed = 0;

        private SettingsData _data;

        public SettingsData Data => _data;

        public float WalkSpeed => _walkSpeed;
        public float CrouchSpeed => _crouchSpeed;

        public void Init()
        {
            SetValues(_walkSpeed, _crouchSpeed);
        }

        public void SetValues(float walkSpeed, float crouchSpeed)
        {
            _data = new SettingsData(walkSpeed, crouchSpeed);
            OnChanged?.Invoke(_data);
        }
    }
}
