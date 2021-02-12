using UnityEngine;

namespace Game.Scripts
{
    public class RaycastController : TickComponent
    {
        [SerializeField] private CapsuleCollider _collider = null;

        [Space]

        [SerializeField] private LayerMask _layerMask = LayerMask.GetMask();

        [Space]

        [SerializeField] [Range(0, 16)] private float _contactDistanceY = 0.025f;

        private RaycastHit _hitDown;

        public bool HasGround => _hitDown.collider != null;

        private Vector3 _raycastPosition;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_raycastPosition, _raycastPosition + Vector3.down * _contactDistanceY);
            if (_hitDown.collider != null)
                Gizmos.DrawCube(_hitDown.point, Vector3.one * 0.3f);
        }

        public override void PhysicsTick()
        {
            UpdateRaycastPosition();
            CastSphere(_raycastPosition, Vector2.down, _contactDistanceY);
        }

        private void UpdateRaycastPosition()
        {
            _raycastPosition.x = _collider.bounds.center.x;
            _raycastPosition.y = _collider.bounds.center.y;
            _raycastPosition.z = _collider.bounds.center.z;
        }

        private void CastSphere(Vector3 raycastOrigin, Vector3 direction, float contactDistance)
        {
            var ray = new Ray(raycastOrigin, direction);
            Physics.SphereCast(ray, _collider.radius, out _hitDown, _collider.radius + contactDistance, _layerMask);
        }
    }
}
