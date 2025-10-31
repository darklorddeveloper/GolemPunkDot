using Unity.Entities;
using Unity.Collections;
using UnityEngine;

namespace DarkLordGame
{
    public partial class SetupClassSystem : SystemBase
    {

        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<SetupClassComponent>();
        }
        protected override void OnUpdate()
        {
            var em = EntityManager;

            // Grab entities that still need initialization (or remove the tag query if you want to run unconditionally)
            var query = SystemAPI.QueryBuilder().WithAll<SetupClassComponent>().Build();
            using var entities = query.ToEntityArray(Allocator.Temp);

            foreach (var e in entities)
            {
                // Enumerate *all* component types on the entity
                using var types = em.GetComponentTypes(e, Allocator.Temp);

                foreach (var ct in types)
                {
                    if (!ct.IsManagedComponent) continue;

                    // Get the managed System.Type of this component
                    var managedType = ct.GetManagedType();

                    // Only care about components deriving from InitComponent
                    if (!typeof(ClassComponentData).IsAssignableFrom(managedType)) continue;
                    // Fetch the actual managed component instance and call Init
                    // Debug.Log("managed type " + managedType.Name);
                    var compObj = em.GetComponentObject<ClassComponentData>(e, managedType);
                    compObj.Init(e, em);
                }

                // Done for this entity
                em.RemoveComponent<SetupClassComponent>(e);
            }
        }
    }
}
