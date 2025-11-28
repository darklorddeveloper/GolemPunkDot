namespace DarkLordGame
{
    public class AttackRequestDataAuthoring : StructAuthorizer<AttackRequestData>
    {
    }

    public class AttackRequestDataBaker : StructBaker<AttackRequestDataAuthoring, AttackRequestData>
    {
        public override void Bake(AttackRequestDataAuthoring authoring)
        {
            base.Bake(authoring);
            var e = GetEntity(Unity.Entities.TransformUsageFlags.Dynamic);
            SetComponentEnabled<AttackRequestData>(e, false);
        }
    }
}
