using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct SafeDestroyCountDownJob : IJobEntity
    {
        public float deltaTime;
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([ChunkIndexInQuery] int chunk, Entity e, ref SafeDestroyComponent component)
        {
            component.period -= deltaTime;
            if (component.period <= 0)
            {
                ecb.SetComponentEnabled<DestroyImmediate>(chunk, e, true);
            }
        }
    }

    [BurstCompile]
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct SafeDestroySystem : ISystem
    {
        private EntityQuery query;
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SafeDestroyComponent>();
            query = SystemAPI.QueryBuilder().WithAll<SafeDestroyComponent>().Build();
        }

        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            var job = new SafeDestroyCountDownJob
            {
                deltaTime = deltaTime,
                ecb = ecb.AsParallelWriter()
            };
            var dependency = job.ScheduleParallel(state.Dependency);
            dependency.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }

    [UpdateInGroup(typeof(PresentationSystemGroup))]
    [UpdateAfter(typeof(SafeDestroySystem))]
    public partial class CleanUpDestroySystem : SystemBase
    {
        private EntityQuery query;
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<SafeCleanupObject>();
            RequireForUpdate<DestroyImmediate>();
        }
        protected override void OnUpdate()
        {
            foreach (var (cleanup, destroy, e) in SystemAPI.Query<SafeCleanupObject, DestroyImmediate>().WithEntityAccess())
            {
                if (cleanup.mainGameObject != null)
                {
                    GameObject.Destroy(cleanup.mainGameObject);
                }
            }
        }
    }

    [UpdateInGroup(typeof(PresentationSystemGroup))]
    [UpdateAfter(typeof(CleanUpDestroySystem))]
    public partial struct FinalDestroySystem : ISystem
    {
        private EntityQuery queryNoChild;
        private EntityQuery query;
        private NativeArray<Entity> entities;
        private NativeArray<DestroyImmediate> destroyImmediates;
        public void OnCreate(ref SystemState state)
        {
            queryNoChild = SystemAPI.QueryBuilder().WithAll<DestroyImmediate>().WithNone<Child>().Build();
            query = SystemAPI.QueryBuilder().WithAll<DestroyImmediate>().WithAll<Child>().Build();
            state.RequireForUpdate<DestroyImmediate>();
        }

        public void OnUpdate(ref SystemState state)
        {
            state.EntityManager.DestroyEntity(queryNoChild);//faster

            entities = query.ToEntityArray(Allocator.Temp);
            destroyImmediates = query.ToComponentDataArray<DestroyImmediate>(Allocator.Temp);
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            for (int i = 0, length = entities.Length; i < length; i++)
            {
                if (destroyImmediates[i].destroyChild == false)
                {
                    ecb.DestroyEntity(entities[i]);
                    continue;
                }
                DestroyRecursive(ecb, state.EntityManager, entities[i]);
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
            destroyImmediates.Dispose();
        }

        static void DestroyRecursive(EntityCommandBuffer ecb, EntityManager em, Entity e)
        {
            if (em.HasBuffer<Child>(e))
            {
                var children = em.GetBuffer<Child>(e);
                for (int i = 0; i < children.Length; i++)
                    DestroyRecursive(ecb, em, children[i].Value);
            }
            ecb.DestroyEntity(e);
        }
    }
}
