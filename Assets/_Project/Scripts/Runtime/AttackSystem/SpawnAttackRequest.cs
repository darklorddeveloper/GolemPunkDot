using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public struct SpawnAttackRequest : IComponentData, IEnableableComponent
    {
       
    }

    public struct SpawnAttackRequestSetting : IComponentData
    {
        public Entity prefab;
        public BlobAssetReference<SpawnRequestBlob> setting;
    }

    public struct SpawnAttackCounter : IComponentData
    {
        public float timeCount;
        public int loopCount;
    }

    [System.Serializable]
    public struct SpawnRequestBlob
    {
        public float forwardOffset;
        public float skews;
        public float splitAngle;
        public float loopCastInterval;
        public int split;//1
        public int loopCast;//minimal 1
    }
}
