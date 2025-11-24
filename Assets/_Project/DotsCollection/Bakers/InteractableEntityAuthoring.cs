using UnityEngine;

namespace DarkLordGame
{
    public class InteractableEntityAuthoring : ClassAuthorizer<InteractableEntity>
    {
    }

    public class InteractableEntityBaker : ClassBaker<InteractableEntityAuthoring, InteractableEntity> { }


}
