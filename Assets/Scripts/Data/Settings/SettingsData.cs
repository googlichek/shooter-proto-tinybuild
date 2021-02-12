namespace Game.Scripts
{
    public struct SettingsData
    {
        private float _walkSpeed;
        private float _crouchSpeed;

        public float WalkSpeed => _walkSpeed;
        public float CrouchSpeed => _crouchSpeed;

        public SettingsData(float walkSpeed, float crouchSpeed)
        {
            _walkSpeed = walkSpeed;
            _crouchSpeed = crouchSpeed;
        }
    }
}
