namespace DarkLordGame
{
    public class DeathImpactAuthoring : StructAuthorizer<DeathImpact, DeathImpactMovement, DeathImpactDamage>
    {
        public bool dealDeathImpactDamage = true;
    }

    public class DeathImpactBaker : StructBaker<DeathImpactAuthoring, DeathImpact, DeathImpactMovement, DeathImpactDamage>
    {
        public override void Bake(DeathImpactAuthoring authoring)
        {
            base.Bake(authoring);
            var e = GetEntity(Unity.Entities.TransformUsageFlags.Dynamic);
            if(authoring.dealDeathImpactDamage)
            {
                AddComponent(e, new DealDeathImpactDamage{});
            }
        }
    }
}