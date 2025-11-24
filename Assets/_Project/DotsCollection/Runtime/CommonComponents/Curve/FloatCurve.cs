using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace DarkLordGame
{
    public struct FloatCurveKey
    {
        public float time;
        public float value;
        public float inTangent;
        public float outTangent;
    }

    public struct FloatCurveBlob
    {
        public bool looped;
        public BlobArray<FloatCurveKey> keys;
    }

    public struct FloatCurve : IComponentData
    {
        public BlobAssetReference<FloatCurveBlob> data;
    }

    public static class FloatCurveUtility
    {
        public static BlobAssetReference<FloatCurveBlob> CreateFloatCurveBlobReference(AnimationCurve curve, bool looped)
        {
            // Build the blob
            var curveKeys = curve.keys;
            int len = curveKeys.Length;
            if (len == 0)
                return default;
            var builder = new BlobBuilder(Allocator.Temp);

            ref FloatCurveBlob root = ref builder.ConstructRoot<FloatCurveBlob>();

            BlobBuilderArray<FloatCurveKey> times = builder.Allocate(ref root.keys, len);

            for (int i = 0; i < len; i++)
            {
                var k = curveKeys[i];
                times[i].time = k.time;   // assumed 0–1
                times[i].value = k.value;
                times[i].inTangent = k.inTangent;
                times[i].outTangent = k.outTangent;
            }
            root.looped = looped;
            
            var blobRef = builder.CreateBlobAssetReference<FloatCurveBlob>(Allocator.Persistent);
            builder.Dispose();
            return blobRef;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Evaluate(this in FloatCurve curveRef, float t)
        {
            if (!curveRef.data.IsCreated)
                return 0f;

            ref FloatCurveBlob curve = ref curveRef.data.Value;
            return Evaluate(ref curve, t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Evaluate(this ref FloatCurveBlob curve, float t)
        {
            ref var keys = ref curve.keys;
            int len = keys.Length;

            if (len == 0)
                return 0f;
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
                    float dt = k1.time - k0.time;
                    float u = (t - k0.time) / dt;

                    float v0 = k0.value;
                    float v1 = k1.value;

                    float m0 = k0.outTangent * dt;
                    float m1 = k1.inTangent * dt;

                    float u2 = u * u;
                    float u3 = u2 * u;

                    float h00 = 2f * u3 - 3f * u2 + 1f;
                    float h10 = u3 - 2f * u2 + u;
                    float h01 = -2f * u3 + 3f * u2;
                    float h11 = u3 - u2;

                    return h00 * v0 + h10 * m0 + h01 * v1 + h11 * m1;
                }
            }
            return last.value;
        }
    }
}
