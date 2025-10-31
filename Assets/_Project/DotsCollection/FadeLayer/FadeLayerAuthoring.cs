using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{

    public class FadeLayerEntityAuthoring : ClassAuthorizer<FadeLayerContainer>
    {

    }

    public class FadeLayerEntityBaker : ClassBaker<FadeLayerEntityAuthoring, FadeLayerContainer>
    {
    }

    [System.Serializable]
    public class FadeLayerContainer : ClassComponentData
    {
        public FadeLayer fadeLayer;
        public override void Init(Entity entity, EntityManager manager)
        {
            base.Init(entity, manager);
            fadeLayer = GameObject.Instantiate<FadeLayer>(fadeLayer);
        }
    }
}
