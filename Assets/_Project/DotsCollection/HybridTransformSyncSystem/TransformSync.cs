using Unity.Entities;
using UnityEngine;
namespace DarkLordGame
{
    public class TransformSync : IComponentData
    {
        public Transform targetTransform;
    }
}