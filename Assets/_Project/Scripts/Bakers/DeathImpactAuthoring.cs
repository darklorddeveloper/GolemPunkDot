namespace DarkLordGame
{
    public class DeathImpactAuthoring : StructAuthorizer<DeathImpact, DeathImpactMovement, DeathImpactDamage>
    {
        public float impactRadius = 1.0f;
        public float destroyPeriod = 0.5f;
    }

    public class DeathImpactBaker : StructBaker<DeathImpactAuthoring, DeathImpact, DeathImpactMovement, DeathImpactDamage>
    {
        public override void Bake(DeathImpactAuthoring authoring)
        {
            base.Bake(authoring);
            var e = GetEntity(Unity.Entities.TransformUsageFlags.Dynamic);
            AddComponent(e, new DealDeathImpactDamage { radius = authoring.impactRadius });
            AddComponent<InitDeathImpact>(e);
            AddComponent(e, new SafeDestroyComponent { period = authoring.destroyPeriod });
            AddComponent(e, new DestroyImmediate { destroyChild = true });
            SetComponentEnabled<SafeDestroyComponent>(e, false);
            SetComponentEnabled<DestroyImmediate>(e, false);
        }
    }
}