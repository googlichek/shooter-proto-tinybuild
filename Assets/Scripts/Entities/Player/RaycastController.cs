using UnityEngine;

namespace Game.Scripts
{
    public class RaycastController : TickComponent
    {
        [SerializeField] private CapsuleCollider _collider = default;

        [Space]

        [SerializeField] private CrouchToggleController _crouchToggleController = default;

        [Space]

        [SerializeField] private LayerMask _layerMask = LayerMask.GetMask();
        
        [Space]

        [SerializeField] [Range(0, 2)] private float _raycastDistanceStanding = 0;
        [SerializeField] [Range(0, 2)] private float _raycastDistanceCrouching = 0;

        [Space]

        [SerializeField] [Range(0, 1)] private float _accelerationTime = 0;

        private RaycastHit _hitDown;

        public bool HasGround => _hitDown.collider != null;

        private Vector3 _raycastPosition;

        private float _raycastDistance;

        private float _raycastDistanceSmoothing;

        private bool _hasGround;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_raycastPosition, new Vector3(_raycastPosition.x, _raycastPosition.y - _raycastDistance, _raycastPosition.z));
            if (_hitDown.collider != null)
                Gizmos.DrawCube(_hitDown.point, Vector3.one * 0.3f);
        }

        public override void PhysicsTick()
        {
            UpdateRaycastPosition();
            CastRay(_raycastPosition, Vector3.down);
        }

        public override void Tick()
        {
            UpdateRaycastDistance();
        }

        private void UpdateRaycastPosition()
        {
            _raycastPosition.x = _collider.bounds.center.x;
            _raycastPosition.y = _collider.bounds.center.y;
            _raycastPosition.z = _collider.bounds.center.z;
        }

        private void CastRay(Vector3 raycastOrigin, Vector3 direction)
        {
            var ray = new Ray(raycastOrigin, direction);
            Physics.Raycast(ray, out _hitDown, _raycastDistance);

            _hasGround = _hitDown.collider != null;
        }

        private void UpdateRaycastDistance()
        {
            var tartetDistance = _crouchToggleController.IsCrouching
                ? _raycastDistanceCrouching
                : _raycastDistanceStanding;

            _raycastDistance = Mathf.SmoothDamp(_raycastDistance, tartetDistance, ref _raycastDistanceSmoothing,
                _accelerationTime);
        }
    }
}
