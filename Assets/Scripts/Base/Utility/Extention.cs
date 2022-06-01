namespace Base.Utility
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class Extention
    {
        public static GameObject Random(this List<GameObject> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count - 1)];
        }

        public static void localPositionX(this Transform transform, float x)
        {
            var position = transform.localPosition;
            position.x = x;
            transform.localPosition = position;
        }
        public static void localPositionY(this Transform transform, float y)
        {
            var position = transform.localPosition;
            position.y = y;
            transform.localPosition = position;
        }
        public static void localPositionZ(this Transform transform, float z)
        {
            var position = transform.localPosition;
            position.z = z;
            transform.localPosition = position;
        }
    }
}