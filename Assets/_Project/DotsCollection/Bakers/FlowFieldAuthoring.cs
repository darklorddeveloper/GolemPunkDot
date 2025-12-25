using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class FlowFieldAuthoring : MonoBehaviour
    {
        public List<FlowFieldLayer> layers = new();
        private static float root3 = Mathf.Sqrt(3.0f);

        void OnDrawGizmosSelected()
        {
            if (layers.Count <= 0) return;
            var origin = transform.position;
            for (int i = 0, length = layers.Count; i < length; i++)
            {
                var layer = layers[i];
                for (int j = 0, num = layer.x * layer.y; j < num; j++)
                {
                    var x = j % layer.x;
                    var y = j / layer.x;
                    // float posX = layer.cellSize * root3 * (x + (y * 0.5f));
                    float posX = layer.cellSize * root3 * (x + y * 0.5f);
                    float posZ = layer.cellSize * 1.5f * y;
                    GizmosExtension.DrawHexagon(origin + new Vector3(posX, 0, posZ), layer.cellSize);
                }

            }
        }


    }

    public class FlowFieldBaker : Baker<FlowFieldAuthoring>
    {
        public override void Bake(FlowFieldAuthoring authoring)
        {
            if (authoring.layers.Count <= 0) return;
            var e = GetEntity(TransformUsageFlags.None);
            var buff = AddBuffer<FlowFieldLayer>(e);
            var origin = authoring.transform.position;
            for (int i = 0, length = authoring.layers.Count; i < length; i++)
            {
                var layer = authoring.layers[i];
                layer.origin = origin;
                buff.Add(authoring.layers[i]);
            }
        }
    }
}
