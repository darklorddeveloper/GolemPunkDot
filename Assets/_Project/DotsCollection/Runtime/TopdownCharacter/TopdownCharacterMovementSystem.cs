using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{

    [BurstCompile]
    public partial struct CastJob : IJobEntity
    {
        [ReadOnly] public CollisionWorld CollisionWorld;
        [ReadOnly] public float deltaTime;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private quaternion CalculateRotation(quaternion currentRotation, float3 pos, float3 lookAtTargetPoint, float turnSpeed)
        {
            float3 to = lookAtTargetPoint - pos;
            to.y = 0;
            if (math.lengthsq(to) < 0.01f)
            {
                return currentRotation;
            }
            //qualify rotation
            float3 fwd = math.mul(currentRotation, new float3(0, 0, 1));
            fwd.y = 0;
            fwd = math.normalizesafe(fwd, new float3(0, 0, 1));

            // Desired forward on XZ plane
            float3 desiredFwd = math.normalizesafe(to, new float3(0, 0, 1));

            // Signed angle around Y between current and desired
            float dot = math.clamp(math.dot(fwd, desiredFwd), -1f, 1f);
            float3 cross = math.cross(fwd, desiredFwd);
            float signedAngle = math.atan2(cross.y, dot); // radians, [-pi, pi]

            // Clamp by max step this frame
            float maxStep = turnSpeed * deltaTime;
            float step = math.clamp(signedAngle, -maxStep, maxStep);

            // Apply yaw-only delta rotation
            quaternion dq = quaternion.AxisAngle(math.up(), step);
            return math.normalize(math.mul(dq, currentRotation));
        }

        void Execute(in TopdownCharacterInput characterInput, ref TopdownCharacterMovement movement, in MovementSpeed speed, ref LocalTransform localTransform)
        {
            //if input is not big enough don't move.
            //look at direction
            float3 pos = localTransform.Position;
            if (movement.canTurnOnlyWhileMoving == false)
            {
                float3 to = characterInput.lookAtTargetPoint - pos;
                to.y = 0;
                if (math.lengthsq(to) > 0.01f)
                {
                    localTransform.Rotation = CalculateRotation(localTransform.Rotation, pos, characterInput.lookAtTargetPoint, movement.turnSpeed);
                }
            }

            if (math.lengthsq(characterInput.movement) <= 0.0125f)
            {
                return;
            }

            if (movement.canTurnOnlyWhileMoving)
            {
                localTransform.Rotation = CalculateRotation(localTransform.Rotation, pos, characterInput.lookAtTargetPoint, movement.turnSpeed);
            }
            //movement
            float distance = math.length(characterInput.movement * speed.value * speed.multiplier * deltaTime);

            ColliderCastHit hit;
            bool hitted = CollisionWorld.SphereCast(pos + movement.castOffset * characterInput.movement, movement.size, characterInput.movement, distance, out hit,
            new CollisionFilter
            {
                BelongsTo = movement.layerBit,
                CollidesWith = movement.collideWithLayerBit,
            });
            movement.isHittedObstacle = hitted;
            if (hitted)
            {
                distance -= movement.size;
                distance = math.max(distance, 0);
                movement.hittedPoint = hit.Position;
                movement.hittedNormal = hit.SurfaceNormal;
                var body = CollisionWorld.Bodies[hit.RigidBodyIndex];

                movement.lastHitEntity = body.Entity;
            }
            movement.lastMovedDistance = distance;
            localTransform.Position = pos + characterInput.movement * distance;

        }
    }

    [BurstCompile]
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
