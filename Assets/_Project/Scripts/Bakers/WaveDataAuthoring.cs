using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public class WaveDataForBaker
    {
        public GameObject prefab;
        public GameObject spawnEffect;
        [Header("Sort by percentage 0 -> 1")]
        public float activateAtPercentage;
        public int numbers;
        public int numbersPerSpawn;
        public float interval;
        public bool isInfinite;
        public float spawnOffset;
        public float horizontalOffsetSplit;
        public float splitAngle;
    }

    public class WaveDataAuthoring : MonoBehaviour
    {

        public List<WaveDataForBaker> waveDatas = new();

        public class Baker : Baker<WaveDataAuthoring>
        {
            public override void Bake(WaveDataAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.Dynamic);
                var buffer = AddBuffer<WaveData>(e);
                var buffer2 = AddBuffer<WaveDataCounter>(e);

                for (int i = 0, length = authoring.waveDatas.Count; i < length; i++)
                {
                    var data = authoring.waveDatas[i];
                    buffer.Add(new WaveData
                    {
                        prefab = GetEntity(data.prefab, TransformUsageFlags.Dynamic),
                        spawnEffect = data.spawnEffect == null ? Entity.Null : GetEntity(data.spawnEffect, TransformUsageFlags.Dynamic),

                        activateAtPercentage = data.activateAtPercentage,
                        isInfinite = data.isInfinite,
                        interval = data.interval,
                        numbers = data.numbers,
                        numbersPerSpawn = data.numbersPerSpawn,
                        spawnOffset = data.spawnOffset,
                        horizontalOffsetSplit = data.horizontalOffsetSplit,
                        splitAngle = data.splitAngle
                    });
                    buffer2.Add(new WaveDataCounter { spawned = 0, timeCount = 0 });
                }
            }
        }
    }
}
