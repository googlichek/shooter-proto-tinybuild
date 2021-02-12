using UnityEngine;

namespace Game.Scripts
{
    public class CrouchToggleController : TickComponent
    {
        [SerializeField] private CapsuleCollider _collider = default;

        [Space]

        [SerializeField] [Range(0, 2)] private float _crouchingHeight = 0;
        [SerializeField] [Range(0, 2)] private float _standingHeight = 0;

        [Space]

        [SerializeField] [Range(0, 1)] private float _accelerationTime = 0;

        private Vector3 _position;

        private bool _isCrouching;

        private float _height;

        private float _smoothingHeight;
        private float _positionSmoothingY;

        public bool IsCrouching => _isCrouching;

        public override void Enable()
        {
            _position = transform.localPosition;

            _isCrouching = false;

            _height = _standingHeight;
        }

        public override void Tick()
        {
            var isCrouching = _isCrouching;
            if (GameManager.Instance.InputWrapper.IsCrouchPressed)
                isCrouching = !isCrouching;

            UpdateState();

            if (isCrouching == _isCrouching)
                return;

            _isCrouching = isCrouching;
            SetHeight(!_isCrouching ? _standingHeight : _crouchingHeight);
        }


        private void UpdateState()
        {
            _collider.height = Mathf.SmoothDamp(_collider.height, _height, ref _smoothingHeight, _accelerationTime);
        }

        private void SetHeight(float height)
        {
            _height = height;
        }
    }
}
