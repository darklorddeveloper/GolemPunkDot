using Unity.Entities;
namespace DarkLordGame
{
    public class SpawnerAuthoring: StructAuthorizer<Spawner>
    {
        
    }

    public class SpawnerBaker : StructBaker<SpawnerAuthoring, Spawner>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            base.Bake(authoring);
            var e = GetEntity(TransformUsageFlags.Dynamic);
            SetComponent(e, new Spawner { spawner = e });

        }
    }
}