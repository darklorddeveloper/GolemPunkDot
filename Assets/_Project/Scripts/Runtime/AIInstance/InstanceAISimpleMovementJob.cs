using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;
namespace DarkLordGame
{

    [BurstCompile]
    [WithAll(typeof(InstanceAIStateFlagMove), typeof(InstanceAIStateChanged))]
    public partial struct InstanceAISimpleMovementResetJob : IJobEntity
    {
        public Random rand;
        public void Execute([ChunkIndexInQuery] int chunk, ref InstanceAISimpleMovement movementForward)
        {
            rand.state += (uint)chunk;
            var distance = rand.NextFloat(movementForward.approachMinDistance, movementForward.approachMaxDistance);
            movementForward.approachDistanceSquare = distance * distance;
        }
    }

    [BurstCompile]
    [WithAll(typeof(InstanceAIStateFlagMove))]
    public partial struct InstanceAISimpleMovementJob : IJobEntity
    {
        public float3 wallPosition;
        public float3 playerPosition;
        public float deltaTime;
        public void Execute(ref TopdownCharacterInput input,
        in FlowFieldAgent agent,
        // in TopdownCharacterMovement topdownCharacterMovement,
        ref InstanceAISimpleMovement movement, in LocalTransform transform, ref InstanceAIState state,
        in InstanceAIStateSetting settingComponent)
        {

            // float3 wallPos = wallPosition;
            // wallPos.z = transform.Position.z;
            // float3 pos = transform.Position;
            // float distanceFromWall = math.distancesq(wallPos, pos);
            // float distanceFromplayer = math.distancesq(playerPosition, pos);
            // var lookAtPoint = distanceFromWall < distanceFromplayer ? wallPos : playerPosition;
            // var forward = transform.Forward();
            // input.movement = forward;
            // if (movement.isAvoiding)
            // {
            //     movement.avoidanceTimeCount += deltaTime;
            //     movement.isAvoiding = movement.avoidanceTimeCount < movement.avoidancePeriod;
            //     movement.isPreviouslyAvoiding = true;
            //     input.lookAtTargetPoint = movement.avoidingDirection + pos;

            // }
            // else if (topdownCharacterMovement.isHittedObstacle)
            // {
            //     //movement.avoidDirection
            //     // var diff = math.normalizesafe(topdownCharacterMovement.hittedPoint - pos, forward);
            //     var norm = -topdownCharacterMovement.hittedNormal;
            //     norm.y = 0;
            //     norm = math.normalizesafe(norm, forward);

            //     var l = new half3((half)norm.z, (half)0.0, (half)(-norm.x));
            //     var dotl = math.sign(math.dot(forward, l));
            //     movement.avoidingDirection = dotl >= 0 ? l : new half3((half)(-norm.z), (half)0.0f, (half)norm.x);
            //     input.lookAtTargetPoint = movement.avoidingDirection + pos;
            //     movement.isAvoiding = true;
            //     movement.avoidanceTimeCount = 0;
            // }
            // else
            // {
            //     if (movement.isPreviouslyAvoiding)
            //     {
            //         movement.isPreviouslyAvoiding = false;
            //         //
            //         input.movement = math.normalizesafe(lookAtPoint - pos, forward);
            //         input.lookAtTargetPoint = movement.avoidingDirection + pos;
            //     }
            //     else
            //     {
            //         input.lookAtTargetPoint = lookAtPoint;
            //     }
            //     movement.avoidanceTimeCount = 0;
            //     movement.isAvoiding = false;
            // }

            var forward = transform.Forward();
            var distanceToTarget = agent.lastDistanceToTarget;
            var pos = transform.Position;
            input.lookAtTargetPoint = agent.movement;
            input.movement = forward;

            // var distanceToTarget = distanceFromWall < distanceFromplayer ? distanceFromWall : distanceFromplayer;

            float dot = math.dot(forward, math.normalize(agent.lastTargetPoint - pos));
            // float dot = math.dot(forward, math.normalize(lookAtPoint - pos));

            if (distanceToTarget < movement.approachDistanceSquare && dot > 0.985f)
            {
                input.movement = float3.zero;
                ref var setting = ref settingComponent.setting.Value;
                var currentState = InstanceAIStateDataUtility.GetStateData(setting, state.currentStateType);
                state.timeSinceStarted += currentState.stateMaxPeriod;
            }


            
        }
    }
}
