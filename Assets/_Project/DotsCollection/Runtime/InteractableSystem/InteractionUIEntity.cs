using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public class InteractionUIEntity : ClassComponentData
    {
        public InteractionUI ui;
        public override void Init(Entity entity, EntityManager manager)
        {
            base.Init(entity, manager);
            ui = GameObject.Instantiate(ui);
            ui.gameObject.SetActive(false);
        }
    }
}
