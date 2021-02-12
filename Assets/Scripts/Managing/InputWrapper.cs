using Rewired;

namespace Game.Scripts
{
    public class InputWrapper : TickBehaviour
    {
        private Player _player;

        private int _playerId = 0;

        private float _leftStickHorizontal;
        private float _leftStickVertical;
        
        private float _rightStickHorizontal;
        private float _rightStickVertical;

        private bool _isJumpPressed;
        private bool _isJumpReleased;
        private bool _isJumpHeld;
        
        private bool _isAttackPressed;
        private bool _isAttackReleased;
        private bool _isAttackHeld;
        
        private bool _isCrouchPressed;
        private bool _isCrouchReleased;
        private bool _isCrouchHeld;
        
        private bool _isWeaponZeroPressed;
        private bool _isWeaponZeroReleased;
        private bool _isWeaponZeroHeld;

        private bool _isWeaponOnePressed;
        private bool _isWeaponOneReleased;
        private bool _isWeaponOneHeld;

        private bool _isEscapePressed;
        private bool _isEscapeReleased;
        private bool _isEscapeHeld;

        public float LeftStickHorizontal => _leftStickHorizontal;
        public float LeftStickVertical => _leftStickVertical;

        public float RightStickHorizontal => _rightStickHorizontal;
        public float RightStickVertical => _rightStickVertical;

        public bool IsJumpPressed => _isJumpPressed;
        public bool IsJumpReleased => _isJumpReleased;
        public bool IsJumpHeld => _isJumpHeld;
        
        public bool IsAttackPressed => _isAttackPressed;
        public bool IsAttackReleased => _isAttackReleased;
        public bool IsAttackHeld => _isAttackHeld;
        
        public bool IsCrouchPressed => _isCrouchPressed;
        public bool IsCrouchReleased => _isCrouchReleased;
        public bool IsCrouchHeld => _isCrouchHeld;
        
        public bool IsWeaponZeroPressed => _isWeaponZeroPressed;
        public bool IsWeaponZeroReleased => _isWeaponZeroReleased;
        public bool IsWeaponZeroHeld => _isWeaponZeroHeld;
        
        public bool IsWeaponOnePressed => _isWeaponOnePressed;
        public bool IsWeaponOneReleased => _isWeaponOneReleased;
        public bool IsWeaponOneHeld => _isWeaponOneHeld;

        public bool IsEscapePressed => _isEscapePressed;
        public bool IsWEscapeReleased => _isEscapeReleased;
        public bool IsEscapeHeld => _isEscapeHeld;

        private bool _isTimeToReset;

        public override void Init()
        {
            base.Init();

            priority = TickPriority.High;

            _player = ReInput.players.GetPlayer(_playerId);
        }

        public override void PhysicsTick()
        {
            base.PhysicsTick();

            _isTimeToReset = true;
        }

        public override void Tick()
        {
            base.Tick();

            ResetInput();

            _isTimeToReset = false;

            HandleInput();
        }

        private void ResetInput()
        {
            if (!_isTimeToReset)
                return;

            _leftStickHorizontal = 0;
            _leftStickVertical = 0;

            _rightStickHorizontal = 0;
            _rightStickVertical = 0;

            _isJumpPressed = false;
            _isJumpReleased = false;
            _isJumpHeld = false;

            _isAttackPressed = false;
            _isAttackReleased = false;
            _isAttackHeld = false;

            _isCrouchPressed = false;
            _isCrouchReleased = false;
            _isCrouchHeld = false;

            _isWeaponZeroPressed = false;
            _isWeaponZeroReleased = false;
            _isWeaponZeroHeld = false;

            _isWeaponOnePressed = false;
            _isWeaponOneReleased = false;
            _isWeaponOneHeld = false;
            
            _isEscapePressed = false;
            _isEscapeReleased = false;
            _isEscapeHeld = false;
        }

        private void HandleInput()
        {
            _leftStickHorizontal += _player.GetAxis(InputActions.Horizontal);
            _leftStickVertical += _player.GetAxis(InputActions.Vertical);
            
            _rightStickHorizontal += _player.GetAxis(InputActions.LookX);
            _rightStickVertical += _player.GetAxis(InputActions.LookY);

            _isJumpPressed = _isJumpPressed || _player.GetButtonDown(InputActions.Jump);
            _isJumpReleased = _isJumpReleased || _player.GetButtonUp(InputActions.Jump);
            _isJumpHeld = _isJumpHeld || _player.GetButton(InputActions.Jump);

            _isAttackPressed = _isAttackPressed || _player.GetButtonDown(InputActions.Attack);
            _isAttackReleased = _isAttackReleased || _player.GetButtonUp(InputActions.Attack);
            _isAttackHeld = _isAttackHeld || _player.GetButton(InputActions.Attack);
            
            _isCrouchPressed = _isCrouchPressed || _player.GetButtonDown(InputActions.Crouch);
            _isCrouchReleased = _isCrouchReleased || _player.GetButtonUp(InputActions.Crouch);
            _isCrouchHeld = _isCrouchHeld || _player.GetButton(InputActions.Crouch);
            
            _isWeaponZeroPressed = _isWeaponZeroPressed || _player.GetButtonDown(InputActions.WeaponZero);
            _isWeaponZeroReleased = _isWeaponZeroReleased || _player.GetButtonUp(InputActions.WeaponZero);
            _isWeaponZeroHeld = _isWeaponZeroHeld || _player.GetButton(InputActions.WeaponZero);

            _isWeaponOnePressed = _isWeaponOnePressed || _player.GetButtonDown(InputActions.WeaponOne);
            _isWeaponOneReleased = _isWeaponOneReleased || _player.GetButtonUp(InputActions.WeaponOne);
            _isWeaponOneHeld = _isWeaponOneHeld || _player.GetButton(InputActions.WeaponOne);

            _isEscapePressed = _isEscapePressed || _player.GetButtonDown(InputActions.WeaponOne);
            _isEscapeReleased = _isEscapeReleased || _player.GetButtonUp(InputActions.WeaponOne);
            _isEscapeHeld = _isEscapeHeld || _player.GetButton(InputActions.WeaponOne);
        }
    }
}
