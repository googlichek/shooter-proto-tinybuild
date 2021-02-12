using UnityEngine;

namespace Game.Scripts
{
    public class PlayerController : TickBehaviour
    {
        [SerializeField] private MovementController _movementController = default;
        [SerializeField] private CrouchToggleController _crouchToggleController = default;
        [SerializeField] private RaycastController _raycastController = default;
        [SerializeField] private PlayerSoundController _soundController = default;
        [SerializeField] private HeadBobController _headBobController = default;
        [SerializeField] private WalkWeaponDriftController _walkWeaponDriftController = default;
        [SerializeField] private WalkAnimationController _walkAnimationController = default;
        [SerializeField] private WeaponsController _weaponsController = default;

        public MovementController MovementController => _movementController;
        public CrouchToggleController CrouchToggleController => _crouchToggleController;
        public RaycastController RaycastController => _raycastController;
        public PlayerSoundController SoundController => _soundController;
        public HeadBobController HeadBobController => _headBobController;
        public WalkWeaponDriftController WalkWeaponDriftController => _walkWeaponDriftController;
        public WalkAnimationController WalkAnimationController => _walkAnimationController;
        public WeaponsController WeaponsController => _weaponsController;

        public override void Init()
        {
            base.Init();

            AttachComponent(_movementController);
            AttachComponent(_crouchToggleController);
            AttachComponent(_raycastController);
            AttachComponent(_soundController);
            AttachComponent(_headBobController);
            AttachComponent(_walkWeaponDriftController);
            AttachComponent(_walkAnimationController);
            AttachComponent(_weaponsController);
        }

        public override void Dispose()
        {
            DetachComponent(_movementController);
            DetachComponent(_crouchToggleController);
            DetachComponent(_raycastController);
            DetachComponent(_soundController);
            DetachComponent(_headBobController);
            DetachComponent(_walkWeaponDriftController);
            DetachComponent(_walkAnimationController);
            DetachComponent(_weaponsController);

            base.Dispose();
        }
    }
}
