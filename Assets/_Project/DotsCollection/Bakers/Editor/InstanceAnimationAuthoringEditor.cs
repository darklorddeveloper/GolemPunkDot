using UnityEditor;
using UnityEngine;

namespace DarkLordGame
{
    [CustomEditor(typeof(InstanceAnimationAuthoring))]
    public class InstanceAnimationAuthoringEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var t = target as InstanceAnimationAuthoring;
            for (int i = 0, length = t.targets.Count; i < length; i++)
            {
                if (t.targets[i] == null) continue;
                var startTime = t.targets[i].GetComponent<StartTimeAuthoring>();
                if (startTime == null)
                {
                   startTime = t.targets[i].AddComponent<StartTimeAuthoring>();
                   startTime.flags =   Unity.Entities.TransformUsageFlags.Dynamic;
                }

                if (t.hasDamageTime)
                {
                    var damageTime = t.targets[i].GetComponent<DamageTimeAuthoring>();
                    if (damageTime == null)
                    {
                        t.targets[i].AddComponent<DamageTimeAuthoring>();
                    }
                }
            }
        }
    }
}
