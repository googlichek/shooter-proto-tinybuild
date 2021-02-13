using TMPro;
using UnityEngine;

namespace Game.Scripts
{
    public class WalkWeaponDriftController : TickComponent
    {
        [SerializeField] [Range(0, 100)] private float _randomOffset = 0;
        [SerializeField] [Range(0, 2)] private float _randomOffsetDuration = 0;

        private Vector3 _initialLocalPosition;
        
        private Vector3 _randomOffsetPosition;
        private Vector3 _randomOffsetTargetPosition;
        private Vector3 _randomOffsetSmoothing;

        private int _randomOffsetTick;

        public override void Init()
        {
            _initialLocalPosition = transform.localPosition;

            _randomOffsetTick = Mathf.CeilToInt(_randomOffsetDuration / Constants.SecondsPerFrame);
        }

        public override void Enable()
        {
            _randomOffsetPosition = _initialLocalPosition;
            transform.localPosition = _initialLocalPosition;
        }

        public override void Tick()
        {
            UpdateRandomOffsetPosition();
        }

        private void UpdateRandomOffsetPosition()
        {
            if (GameManager.Instance.Tick % _randomOffsetTick == 0)
            {
                _randomOffsetTargetPosition = _initialLocalPosition + Random.insideUnitSphere * _randomOffset;
                _randomOffsetPosition = transform.localPosition;
            }

            _randomOffsetPosition.x =
                Mathf.SmoothDamp(_randomOffsetPosition.x, _randomOffsetTargetPosition.x, ref _randomOffsetSmoothing.x, _randomOffsetDuration, Mathf.Infinity, Time.deltaTime);
            _randomOffsetPosition.y =
                Mathf.SmoothDamp(_randomOffsetPosition.y, _randomOffsetTargetPosition.y, ref _randomOffsetSmoothing.y, _randomOffsetDuration, Mathf.Infinity, Time.deltaTime);
            _randomOffsetPosition.z =
                Mathf.SmoothDamp(_randomOffsetPosition.z, _randomOffsetTargetPosition.z, ref _randomOffsetSmoothing.z, _randomOffsetDuration, Mathf.Infinity, Time.deltaTime);

            transform.localPosition = _randomOffsetPosition;
        }
    }
}
