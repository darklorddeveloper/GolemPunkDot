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
        public override void Init()
        {
            base.Init();
            fadeLayer = GameObject.Instantiate<FadeLayer>(fadeLayer);
        }
    }
}
