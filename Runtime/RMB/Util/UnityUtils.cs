using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RMB.Util
{
    public static class UnityUtils
    {
        public static void ClearChildren(this Transform transform)
        {
            foreach (Transform t in transform) Object.Destroy(t.gameObject);
        }

        public static List<GameObject> GetChildren(this Transform transform)
        {
            return (from Transform obj in transform select obj.gameObject).ToList();
        }

        public static List<T> GetChildrenComponent<T>(this Transform transform)
        {
            return (from Transform obj in transform select obj.gameObject.GetComponent<T>()).ToList();
        }

        public static IEnumerable<T> GetUniqueFlags<T>(this T flags) where T : Enum
        {
            ulong flag = 1;
            foreach (var value in Enum.GetValues(flags.GetType()).Cast<T>())
            {
                var bits = Convert.ToUInt64(value);
                while (flag < bits) flag <<= 1;

                if (flag == bits && flags.HasFlag(value)) yield return value;
            }
        }

        public static void SetEnabled(this Transform transform, bool enabled)
        {
            transform.GetComponent<Renderer>().enabled = enabled;
        }
    }
}