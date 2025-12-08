using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public class EffectFusionTableEntity : ClassComponentData
    {
        public PartFusionTable table;
        public override void Init(Entity entity, EntityManager manager)
        {
            base.Init(entity, manager);
            table = ScriptableObject.Instantiate(table);
            table.Init();
        }
    }
}
