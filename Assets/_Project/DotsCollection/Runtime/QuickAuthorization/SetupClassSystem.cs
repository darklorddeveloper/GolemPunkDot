using Unity.Entities;
using Unity.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace DarkLordGame
{
    public partial class SetupClassSystem : SystemBase
    {
        private EntityQuery query;
        protected override void OnCreate()
        {
            query = SystemAPI.QueryBuilder().WithAll<SetupClassComponent>().Build();
            base.OnCreate();
            RequireForUpdate<SetupClassComponent>();
        }
        protected override void OnUpdate()
        {
            var em = EntityManager;
            int count = query.CalculateChunkCount();
            if (count <= 0) return;
            using var entities = query.ToEntityArray(Allocator.Temp);

            foreach (var e in entities)
            {
                // Enumerate *all* component types on the entity
                using var types = em.GetComponentTypes(e, Allocator.Temp);
                List<ClassComponentData> targetDatas = new();
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
                    targetDatas.Add(compObj);

                }
                targetDatas = targetDatas.OrderBy(x => x.priority).ToList();
                for (int i = 0, length = targetDatas.Count; i < length; i++)
                {
                    targetDatas[i].Init(e, EntityManager);
                }
                // Done for this entity
                em.SetComponentEnabled<SetupClassComponent>(e, false);
            }
        }
    }
}
