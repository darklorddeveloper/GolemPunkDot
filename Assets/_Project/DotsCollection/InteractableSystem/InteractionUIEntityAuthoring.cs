using UnityEngine;

namespace DarkLordGame
{
    public class InteractionUIEntityAuthoring : ClassAuthorizer<InteractionUIEntity>
    {
    }

    public class InteractionUIBaker : ClassBaker<InteractionUIEntityAuthoring, InteractionUIEntity> { }

    [System.Serializable]
    public class InteractionUIEntity : ClassComponentData
    {
        public InteractionUI ui;
        public override void Init()
        {
            base.Init();
            ui = GameObject.Instantiate(ui);
            ui.gameObject.SetActive(false);
        }
    }
}
