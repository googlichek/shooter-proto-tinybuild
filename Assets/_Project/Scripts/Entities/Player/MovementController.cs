using UnityEngine;

namespace Game.Scripts
{
    public class MovementController : TickComponent
    {
        [SerializeField] private Camera _camera = default;
        [SerializeField] private Rigidbody _rigidbody = default;

        [Space]

        [SerializeField] private RaycastController _raycastController = default;
        [SerializeField] private PlayerSoundController _soundController = default;
        [SerializeField] private CrouchToggleController _crouchToggleController = default;

        [Space]

        [SerializeField] [Range(0, 100)] private float _maxVelocityX = 0;
        [SerializeField] [Range(0, 100)] private float _maxVelocityY = 0;
        [SerializeField] [Range(0, 100)] private float _maxVelocityZ = 0;

        [Space]

        [SerializeField] [Range(0, 100)] private float _timeToJumpApex = 0;

        [Space]

        [SerializeField] [Range(0, 100)] private float _maxJumpHeight = 0;
        [SerializeField] [Range(0, 100)] private float _minJumpHeight = 0;
        [SerializeField] [Range(0, 100)] private float _minGravityPull = 0;

        [Space]

        [SerializeField] [Range(0, 10)] private float _lookSpeedX = 0;
        [SerializeField] [Range(0, 10)] private float _lookSpeedY = 0;

        [Space]

        [SerializeField] [Range(0, 100)] private float _pitchMin = 0;
        [SerializeField] [Range(0, 100)] private float _pitchMax = 0;

        [Space]

        [SerializeField] [Range(0, 10)] private float _accelerationTimeGrounded = 0;
        [SerializeField] [Range(0, 10)] private float _accelerationTimeAirborne = 0;
        [SerializeField] [Range(0, 10)] private float _accelerationTimeLook = 0;

        [Space]

        [SerializeField] [Range(0, 10)] private float _stepDistance = 0;

        private Vector3 _gravity;
        private Vector3 _position;
        private Vector3 _deltaPosition;

        private Vector3 _velocity;
        private Vector3 _directionVelocity;

        private Vector3 _rotation;
        private Vector3 _cameraRotation;

        private Vector2 _inputVector;

        private float _walkSpeed = 0;
        private float _crouchSpeed = 0;

        private float _smoothingX;
        private float _smoothingZ;
        
        private float _smoothingYaw;
        private float _smoothingPitch;

        private float _maxJumpVelocity;
        private float _minJumpVelocity;

        private float _yaw;
        private float _pitch;

        private float _smoothYaw;
        private float _smoothPitch;

        private float _nextStepRemainingDistance;

        public float StepDistance => _stepDistance;
        public float NextStepRemainingDistance => _nextStepRemainingDistance;

        public override void Init()
        {
            _gravity = Vector3.zero;
            _gravity.y = -2 * _maxJumpHeight / Mathf.Pow(_timeToJumpApex, 2);

            Physics.gravity = _gravity;

            _maxJumpVelocity = Mathf.Abs(_gravity.y) * _timeToJumpApex;
            _minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(_gravity.y) * _minJumpHeight);
        }

        public override void Enable()
        {
            _velocity = Vector3.zero;

            _rotation = Vector3.zero;
            _cameraRotation = Vector3.zero;

            _smoothingX = 0;
            _smoothingZ = 0;

            _yaw = transform.localEulerAngles.y;
            _pitch = _camera.transform.localEulerAngles.x;

            _smoothingYaw = 0;
            _smoothingPitch = 0;

            _nextStepRemainingDistance = _stepDistance;

            GameManager.Instance.DataBase.Settings.OnChanged += HandleSettingsUpdate;
            HandleSettingsUpdate(GameManager.Instance.DataBase.Settings.Data);
        }

        public override void Disable()
        {
            GameManager.Instance.DataBase.Settings.OnChanged -= HandleSettingsUpdate;
        }

        private void HandleSettingsUpdate(SettingsData settings)
        {
            _walkSpeed = settings.WalkSpeed;
            _crouchSpeed = settings.CrouchSpeed;
        }

        public override void PhysicsTick()
        {
            ClampVelocity();
            Move();
        }

        public override void Tick()
        {
            CalculateGravityInfluence();
            CalculateHorizontalVelocity();
            CalculateRotation();
            Rotate();
            ProcessJump();
            ProcessSteps();
        }

        private void CalculateGravityInfluence()
        {
            _velocity.y = _raycastController.HasGround && _velocity.y <= 0 ? -_minGravityPull : _velocity.y + _gravity.y * Time.deltaTime;
        }

        private void CalculateHorizontalVelocity()
        {
            var accelerationTime = _raycastController.HasGround ? _accelerationTimeGrounded : _accelerationTimeAirborne;
            var walkSpeed =
                !_crouchToggleController.IsCrouching
                    ? _walkSpeed
                    : _crouchSpeed;

            _inputVector.x = GameManager.Instance.InputWrapper.LeftStickHorizontal;
            _inputVector.y = GameManager.Instance.InputWrapper.LeftStickVertical;

            if (_inputVector.magnitude > 1)
                _inputVector = _inputVector.Normalized();

            _velocity.x = Mathf.SmoothDamp(_velocity.x, _inputVector.x * walkSpeed, ref _smoothingX, accelerationTime);
            _velocity.z = Mathf.SmoothDamp(_velocity.z, _inputVector.y * walkSpeed, ref _smoothingZ, accelerationTime);
        }

        private void ClampVelocity()
        {
            _velocity.x = Mathf.Clamp(_velocity.x, -_maxVelocityX, _maxVelocityX);
            _velocity.y = Mathf.Clamp(_velocity.y, -_maxVelocityY, _maxVelocityY);
            _velocity.z = Mathf.Clamp(_velocity.z, -_maxVelocityZ, _maxVelocityZ);
        }

        private void SwitchToJumpVelocityY()
        {
            _velocity.y = _maxJumpVelocity;
        }

        private void SwitchToMinJumpVelocityY()
        {
            _velocity.y = _minJumpVelocity;
        }

        public void CalculateRotation()
        {
            _yaw += GameManager.Instance.InputWrapper.RightStickHorizontal * _lookSpeedX;
            _pitch -= GameManager.Instance.InputWrapper.RightStickVertical * _lookSpeedY;

            _pitch = Mathf.Clamp(_pitch, -_pitchMin, _pitchMax);

            _smoothYaw = Mathf.SmoothDampAngle(_smoothYaw, _yaw, ref _smoothingYaw, _accelerationTimeLook);
            _smoothPitch = Mathf.SmoothDampAngle(_smoothPitch, _pitch, ref _smoothingPitch, _accelerationTimeLook);
        }

        private void Move()
        {
            _directionVelocity = transform.TransformDirection(_velocity);

            _deltaPosition.x = _directionVelocity.x * Time.fixedDeltaTime;
            _deltaPosition.y = _directionVelocity.y * Time.fixedDeltaTime;
            _deltaPosition.z = _directionVelocity.z * Time.fixedDeltaTime;

            _position.x = _rigidbody.position.x + _deltaPosition.x;
            _position.y = _rigidbody.position.y + _deltaPosition.y;
            _position.z = _rigidbody.position.z + _deltaPosition.z;

            _rigidbody.MovePosition(_position);
        }

        private void Rotate()
        {
            _rotation.y = _smoothYaw;
            transform.localEulerAngles = _rotation;

            _cameraRotation.x = _smoothPitch;
            _camera.transform.localEulerAngles = _cameraRotation;
        }

        private void ProcessJump()
        {
            if (_raycastController.HasGround &&
                GameManager.Instance.InputWrapper.IsJumpPressed &&
                !_crouchToggleController.IsCrouching)
                SwitchToJumpVelocityY();

            if (!_raycastController.HasGround && _velocity.y > _minJumpVelocity &&
                GameManager.Instance.InputWrapper.IsJumpReleased)
                SwitchToMinJumpVelocityY();
        }

        private void ProcessSteps()
        {
            if (!_raycastController.HasGround)
                return;

            if (_inputVector.IsEqual(Vector2.zero))
                _nextStepRemainingDistance = _stepDistance;
            else
                _nextStepRemainingDistance -= _deltaPosition.magnitude;

            if (_nextStepRemainingDistance <= 0)
            {
                _nextStepRemainingDistance = _stepDistance;
                _soundController.PlayStepSound();
            }
        }
    }
}
