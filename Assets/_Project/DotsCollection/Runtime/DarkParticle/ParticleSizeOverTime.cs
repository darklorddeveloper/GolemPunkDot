using Unity.Entities;


namespace DarkLordGame
{
    public struct ParticleSizeOverTime : IComponentData
    {
        public float maxSize;
        public BlobAssetReference<FloatCurveBlob> data;
    }

    public struct ParticleStartSize : IComponentData, IEnableableComponent
    {
    }
}
