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
    }
}