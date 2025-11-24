using UnityEngine;

namespace DarkLordGame
{
    public class CameraShakeAuthoring : EnableStructAuthorizer<CameraShake>
    {
        
    }
    
    public class CameraShakeBaker : EnableStructBaker<CameraShakeAuthoring, CameraShake>
    {
    }

}
