using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public partial struct StartTimeJob : IJobEntity
    {
        public float value;
        public void Execute(ref StartTime startTime, EnabledRefRW<StartTime> startTimeEnables)
        {
            startTime.Value = value;
            startTimeEnables.ValueRW = false;
        }
    }

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
        }
    }
}
