namespace DarkLordGame
{
    public class DeathImpactAuthoring : StructAuthorizer<DeathImpact, DeathImpactMovement, DeathImpactDamage>
    {
        public bool dealDeathImpactDamage = true;
        public float impactRadius = 1.0f;
    }

    public class DeathImpactBaker : StructBaker<DeathImpactAuthoring, DeathImpact, DeathImpactMovement, DeathImpactDamage>
    {
        public override void Bake(DeathImpactAuthoring authoring)
        {
            base.Bake(authoring);
            var e = GetEntity(Unity.Entities.TransformUsageFlags.Dynamic);
            AddComponent(e, new DealDeathImpactDamage { radius = authoring.impactRadius });
            SetComponentEnabled<DealDeathImpactDamage>(e, false);
        }
    }
}