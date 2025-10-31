using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class SettingUIEntityAuthoring : ClassAuthorizer<SettingUIEntity>
    {

    }

    public class SettingUIEntityBaker : ClassBaker<SettingUIEntityAuthoring, SettingUIEntity>
    {
        
    }
    
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
