using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [BurstCompile]
    public partial struct StartTimeJob : IJobEntity
    {
        public float value;
        public void Execute(ref StartTime startTime, EnabledRefRW<StartTime> startTimeEnables)
        {
            startTime.Value = value;
            startTimeEnables.ValueRW = false;
        }
    }

    [BurstCompile]
    public partial struct DamageTimeJob : IJobEntity
    {
        public float value;
        public void Execute(ref DamageTime time, EnabledRefRW<DamageTime> timeEnables)
        {
            time.Value = value;
            timeEnables.ValueRW = false;
        }
    }

    [BurstCompile]
    public partial struct StartTimeSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float value = (float)SystemAPI.Time.ElapsedTime;
            var job = new StartTimeJob
            {
                value = value
            };
            job.ScheduleParallel();

             var job2 = new DamageTimeJob
            {
                value = value
            };
            job2.ScheduleParallel();
        }
    }
}
