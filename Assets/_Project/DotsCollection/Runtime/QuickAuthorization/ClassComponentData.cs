using Unity.Entities;

namespace DarkLordGame
{
    public class ClassComponentData : IComponentData
    {
        public bool initialized = false;//check for rush systel
        public virtual void Init(Entity entity, EntityManager manager)
        {
            initialized = true; 
            //instantiate self prefab and assign
        }
    }

    public struct SetupClassComponent : IComponentData, IEnableableComponent
    {

    }
}
