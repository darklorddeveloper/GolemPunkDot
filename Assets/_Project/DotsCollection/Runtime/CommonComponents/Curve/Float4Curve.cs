using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
namespace DarkLordGame
{
    public struct Float4CurveKey
    {
        public float time;
        public float4 value;
    }

    public struct Float4CurveBlob
    {
        public bool looped;
        public BlobArray<Float4CurveKey> keys;
    }


    public struct Float4Curve : IComponentData
    {
        public float period;
        public BlobAssetReference<Float4CurveBlob> data;
    }

    public static class Float4CurveUtility
    {
        public static BlobAssetReference<Float4CurveBlob> CreateBlob(Gradient gradient, bool looped)
        {
            var colorKeys = gradient.colorKeys;
            var alphaKeys = gradient.alphaKeys;

            // Collect unique times from both color and alpha keys
            var times = new List<float>(colorKeys.Length + alphaKeys.Length);
            foreach (var ck in colorKeys)
                if (!times.Contains(ck.time))
                    times.Add(ck.time);
            foreach (var ak in alphaKeys)
                if (!times.Contains(ak.time))
                    times.Add(ak.time);

            if (times.Count == 0)
                return default;

            times.Sort();

            var builder = new BlobBuilder(Allocator.Temp);
            ref Float4CurveBlob root = ref builder.ConstructRoot<Float4CurveBlob>();
            
            root.looped = looped;
            
            var keys = builder.Allocate(ref root.keys, times.Count);

            for (int i = 0; i < times.Count; i++)
            {
                float t = times[i];
                Color c = gradient.Evaluate(t);

                keys[i] = new Float4CurveKey
                {
                    time = t,
                    value = new float4(c.r, c.g, c.b, c.a)
                };
            }

            var blobRef = builder.CreateBlobAssetReference<Float4CurveBlob>(Allocator.Persistent);
            builder.Dispose();
            return blobRef;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 Evaluate(this ref Float4CurveBlob curve, float t)
        {
            ref var keys = ref curve.keys;
            int len = keys.Length;

            if (len == 0)
                return float4.zero;

            if (len == 1)
                return keys[0].value;

            var first = keys[0];
            var last = keys[len - 1];
            if (curve.looped)
            {
                t = t.Repeat(first.time, last.time);
            }
            else
            {
                t = math.clamp(t, first.time, last.time);
            }


            if (t <= first.time)
                return first.value;

            if (t >= last.time)
                return last.value;

            for (int i = 0; i < len - 1; i++)
            {
                var k0 = keys[i];
                var k1 = keys[i + 1];

                if (t <= k1.time)
                {
                    float segmentT = (t - k0.time) / (k1.time - k0.time);
                    return math.lerp(k0.value, k1.value, segmentT);
                }
            }

            return last.value;
        }
        public static float4 Evaluate(this in Float4Curve gradientRef, float t)
        {
            if (!gradientRef.data.IsCreated)
                return float4.zero;

            ref var blob = ref gradientRef.data.Value;
            return blob.Evaluate(t);
        }
    }
}
