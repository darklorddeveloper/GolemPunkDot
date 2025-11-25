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

}
