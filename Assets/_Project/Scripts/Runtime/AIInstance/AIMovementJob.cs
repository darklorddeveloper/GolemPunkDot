using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DarkLordGame
{

    [BurstCompile]
    [WithAll(typeof(InstanceAIStateFlagMove), typeof(InstanceAIStateChanged))]
    public partial struct AIMovementResetJob : IJobEntity
    {
        public Random rand;
        public void Execute([ChunkIndexInQuery] int chunk, ref AIMovement movementForward)
        {
            rand.state += (uint)chunk;
            var distance = rand.NextFloat(movementForward.approachMinDistance, movementForward.approachMaxDistance);
            movementForward.approachDistanceSquare = distance * distance;
        }
    }

    [BurstCompile]
    [WithAll(typeof(InstanceAIStateFlagMove))]
    public partial struct AIMovementJob : IJobEntity
    {
        public float3 wallPosition;
        public float3 playerPosition;

        public void Execute(ref TopdownCharacterInput input, in AIMovement movement, in LocalTransform transform, ref InstanceAIState state)
        {
            float3 wallPos = wallPosition;
            wallPos.z = transform.Position.z;
            float3 pos = transform.Position;
            float distanceFromWall = math.distancesq(wallPos, pos);
            float distanceFromplayer = math.distancesq(playerPosition, pos);
            var lookAtPoint = distanceFromWall < distanceFromplayer ? wallPos : playerPosition;
            input.lookAtTargetPoint = lookAtPoint;
            var forward = transform.Forward();
            input.movement = forward;
//TODO: AVOIDANCE using circle cast then change
            var distanceToTarget = distanceFromWall < distanceFromplayer ? distanceFromWall : distanceFromplayer;
            float dot = math.dot(forward, lookAtPoint);
            if (distanceToTarget < movement.approachDistanceSquare && dot > 0.9985f)
            {
                state.interuptState = state.attackState;
            }
        }
    }
}
