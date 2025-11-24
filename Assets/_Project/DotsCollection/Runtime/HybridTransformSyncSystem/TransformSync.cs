using Unity.Entities;
using UnityEngine;
namespace DarkLordGame
{
    public class TransformSync : IComponentData
    {
        public Transform targetTransform;
        public Vector3 previousPosition;
        public Vector3 deltaPosition;
    }
}