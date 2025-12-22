using Unity.Entities;

namespace DarkLordGame
{
    //use blob assets for datas iteself ---- no need blob assets
    //dynamic buffer to store all the prefabs
    //wave counter for counter

    public struct WaveData : IBufferElementData
    {
        public Entity prefab;
        public Entity spawnEffect;
        public float activateAtPercentage;
        public int numbers;
        public int numbersPerSpawn;
        public float interval;
        public bool isInfinite;
        public float spawnOffset;
        public float horizontalOffsetSplit;
        public float splitAngle;
    }

    public struct WaveDataCounter : IBufferElementData
    {
        public int spawned;
        public float timeCount;
    }

    
}
