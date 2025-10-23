using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;
namespace DarkLordGame
{
    public partial class CameraShakeSystem : SystemBase
    {
        private bool isShaking = false;
        private float currentPower;
        private float shakeTimeCount;
        private float period;
        private uint frameCount;
        private Random rand;
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<MainCamera>();
            frameCount = 0;
            
        }

        protected override void OnUpdate()
        {
            if (SystemAPI.ManagedAPI.TryGetSingleton<MainCamera>(out var mainCamera) == false)
            {
                return;
            }
            if (mainCamera.initialized == false) return;
            //update shaker then disabled it
            foreach( var (cameraShake, entity) in SystemAPI.Query<CameraShake>().WithEntityAccess())
            {
                currentPower = math.max(cameraShake.power, currentPower);
                period = math.max(period - shakeTimeCount, cameraShake.period);
                shakeTimeCount = 0;
                isShaking = true;
                EntityManager.SetComponentEnabled<CameraShake>(entity, false);
            }

            if(isShaking)
            {
                shakeTimeCount += SystemAPI.Time.DeltaTime;
                float power = math.lerp(currentPower, 0, shakeTimeCount / period);
                if (power < 0)
                {
                    mainCamera.cameraTransform.localPosition = Vector3.zero;
                    isShaking = false;
                    return;
                }
                frameCount++;
                rand = Random.CreateFromIndex(frameCount);
                float x = rand.NextFloat(-power, power);
                float y = rand.NextFloat(-power, power) * 0.5f;
                mainCamera.cameraTransform.localPosition = new Vector3(x, y, 0);
            }
        }
    }
}
