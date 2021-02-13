using UnityEngine;

namespace Game.Scripts
{
    public class HeadBobController : TickComponent
    {
        [SerializeField] private MovementController _movementController = default;

        [Space]

        [SerializeField] [Range(0, 1)] private float _offset = 0;

        [Space]

        [SerializeField] [Range(0, 1)] private float _smoothingTime = 0.05f;

        private Vector3 _position;

        private float _localPositionY;
        private float _smoothingVelocity;

        private float _stepDelta;
        private float _bobOffset;

        public override void Init()
        {
            _localPositionY = transform.localPosition.y;
        }

        public override void Enable()
        {
            _position = Vector3.zero;
        }

        public override void Tick()
        {
            Bob();
        }

        private void Bob()
        {
            _stepDelta = (_movementController.StepDistance - _movementController.NextStepRemainingDistance) / _movementController.StepDistance;

            _bobOffset = Mathf.Sin(2 * Mathf.PI * _stepDelta) * _offset;

            _position.x = transform.localPosition.x;
            _position.y = Mathf.SmoothDamp(_position.y, _localPositionY + _bobOffset, ref _smoothingVelocity, _smoothingTime);
            _position.z = transform.localPosition.z;

            transform.localPosition = _position;
        }
    }
}
