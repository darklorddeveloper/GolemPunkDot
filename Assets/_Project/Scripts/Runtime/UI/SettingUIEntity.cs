using UnityEngine;
using Unity.Entities;

namespace DarkLordGame
{
    
    [System.Serializable]
    public class SettingUIEntity: ClassComponentData
    {
        public SettingUI ui;
        public override void Init(Entity entity, EntityManager manager)
        {
            base.Init(entity, manager);
            ui = GameObject.Instantiate<SettingUI>(ui);
            ui.gameObject.SetActive(false);
        }
    }
}
