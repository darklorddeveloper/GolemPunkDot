using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{


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
