using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public struct AIStateIdle : IComponentData, IEnableableComponent
    {
        public float idleStatePeriod;
    }
}
