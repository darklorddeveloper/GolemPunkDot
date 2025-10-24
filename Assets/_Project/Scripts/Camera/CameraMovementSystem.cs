using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public partial class CameraMovementSystem : SystemBase
    {
        private Vector3 startPos;
        private Quaternion startRot;
        private float timeCount;
        private bool isChangingAngle;
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<MainCamera>();
        }

        protected override void OnUpdate()
        {
            if (SystemAPI.ManagedAPI.TryGetSingleton<MainCamera>(out var cam) == false) return;
            if (cam.initialized == false) return;

            if(SystemAPI.TryGetSingleton<CameraMovement>(out var camMovement) == false) return;
            if(isChangingAngle)
            {
                timeCount += SystemAPI.Time.DeltaTime;
                cam.cameraRootTransform.position = Vector3.Slerp(startPos, camMovement.position, timeCount / camMovement.movePeriod);
                cam.cameraRootTransform.rotation = Quaternion.Slerp(startRot, Quaternion.Euler(camMovement.eulerRotation), timeCount / camMovement.movePeriod);
                isChangingAngle = timeCount >= camMovement.movePeriod;
            }
            if (SystemAPI.TryGetSingleton<CurrentPhase>(out var currentPhase) == false) return;
            if(currentPhase.isChangingPhase == false)
            {
                return;
            }
            timeCount = 0;
            isChangingAngle = true;
            startPos = cam.cameraRootTransform.position;
            startRot = cam.cameraRootTransform.rotation;
        }
    }
}
