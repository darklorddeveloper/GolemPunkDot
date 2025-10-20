using Unity.Entities;
using UnityEngine;
namespace DarkLordGame
{
    public class MainCameraAuthoring : ClassAuthorizer<MainCamera>
    {

    }

    public class MainCameraBaker : ClassBaker<MainCameraAuthoring, MainCamera>
    {
        
    }

    [System.Serializable]
    public class MainCamera : ClassComponentData
    {
        public Camera camera;
        [System.NonSerialized] public GameObject cameraRootObject;
        [System.NonSerialized] public Transform cameraRootTransform;
        [System.NonSerialized] public Transform cameraTransform;
        public override void Init()
        {
            base.Init();
            camera = GameObject.Instantiate<Camera>(camera);
            cameraTransform = camera.transform;
            cameraRootObject = new GameObject("Camera Root");
            cameraRootTransform = cameraRootObject.transform;
            cameraRootTransform.position = cameraTransform.position;
            cameraRootTransform.rotation = cameraTransform.rotation;
            cameraTransform.SetParent(cameraRootTransform);
        }
    }
}