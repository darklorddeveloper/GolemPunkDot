using UnityEngine;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace DarkLordGame
{
    public static class CurveMathUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Repeat(this float t, float min, float max)
        {
            var length = max - min;
            if (length == 0.0f) return t;
            return math.clamp(t - Mathf.Floor(t / length) * length, 0, length) + min;
        }
    }
}
