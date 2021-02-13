using UnityEngine;

namespace Game.Scripts
{
    public static class Extentions
    {
        private const float EqualityThreshold = 0.0000001f;

        public static bool IsEqual(this float a, float b)
        {
            return Mathf.Abs(a - b) <= EqualityThreshold;
        }

        public static bool IsEqual(this Vector2 a, Vector2 b)
        {
            return (a - b).sqrMagnitude <= EqualityThreshold;
        }

        public static Vector2 Normalized(this Vector2 a)
        {
            var magnitude = a.magnitude;
            if (a.magnitude > 9.99999974737875E-06)
                a /= magnitude;
            else
                a = Vector2.zero;

            return a;
        }

        public static bool HasLayer(this LayerMask layerMask, int layer)
        {
            return (layerMask & (1 << layer)) != 0;
        }
    }
}
