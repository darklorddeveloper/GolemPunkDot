using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{

    public struct SafeDestroyComponent : IComponentData, IEnableableComponent
    {
        public float period;
    }

    public struct DestroyImmediate : IComponentData, IEnableableComponent
    {
        public bool destroyChild;
    }
}
