using UnityEngine;
using Unity.Entities;
namespace DarkLordGame
{
    [System.Serializable]
    public struct AttackMovementStraightline : IComponentData
    {
        public float offsetFromWall;
    }
}
