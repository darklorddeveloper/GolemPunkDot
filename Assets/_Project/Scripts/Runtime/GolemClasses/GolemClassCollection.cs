using Unity.Entities;

namespace DarkLordGame
{

    [InternalBufferCapacity(16)]
    public struct GolemClassCollection : IBufferElementData
    {
        public Entity prefab;
    }
}
