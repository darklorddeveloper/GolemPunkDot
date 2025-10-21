using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{

    // [BurstCompile]
    public partial struct CastJob : IJobEntity
    {
        [ReadOnly] public CollisionWorld CollisionWorld;
        [ReadOnly] public float deltaTime;
        void Execute(in TopdownCharacterInput characterInput, in TopdownCharacterMovement movement, ref LocalTransform localTransform)
        {
            //if input is not big enough don't move.
            //look at direction

            if (math.lengthsq(characterInput.movement) <= 0.0125f)
            {
                return;
            }
            //movement
            float3 pos = localTransform.Position;
            float distance = math.length(characterInput.movement * movement.movementSpeed * deltaTime);
            // var endPoint = pos + characterInput.movement * (distance + movement.castOffset);
            // var input = new RaycastInput
            // {
            //     Start = pos,
            //     End = endPoint,
            //     Filter = new CollisionFilter
            //     {
            //         BelongsTo = uint.MaxValue,
            //         CollidesWith = uint.MaxValue,
            //     }
            // };
            ColliderCastHit hit;
            if (CollisionWorld.SphereCast(pos + movement.castOffset * characterInput.movement, movement.size, characterInput.movement, distance, out hit,
            new CollisionFilter
            {
                BelongsTo = movement.layerBit,
                CollidesWith = movement.collideWithLayerBit,
            })
            )
            {
                distance -= movement.size;
                distance = math.max(distance, 0);
            }
                localTransform.Position = pos + characterInput.movement * distance;
            
        }
    }

    // [BurstCompile]
    [UpdateAfter(typeof(TopdownPlayerInputSystem))]
    public partial struct TopdownCharacterMovementSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {

            var physics = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            var cw = physics.CollisionWorld; // a value type, safe to capture
            float deltaTime = SystemAPI.Time.DeltaTime;
            // Run per-entity in parallel: read RaycastQuery, write RaycastResult
            var job = new CastJob
            {
                CollisionWorld = cw,
                deltaTime = deltaTime
            };
            job.ScheduleParallel();
        }
    }
}
