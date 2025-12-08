using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace DarkLordGame
{
    [CreateAssetMenu(fileName = "FusionTable", menuName = "FusionTable")]
    public class PartFusionTable : ScriptableObject
    {
        [System.Serializable]
        public class SpecialFusionData
        {
            public List<GolemPart> parts = new();
            public List<EffectBase> effects = new();
            public bool IsTarget(List<GolemPart> targets)
            {
                if (parts.Count != targets.Count) return false;
                for (int i = 0, length = targets.Count; i < length; i++)
                {
                    if (parts.Contains(targets[i]) == false)
                    {
                        return false;
                    }
                }
                return true;
            }
        }


        [System.Serializable]
        public class FusionData
        {
            public List<PartTagValue> partTags = new();
            public List<EffectBase> effects = new();

            [System.NonSerialized] public float[] scores;
            public static readonly PartTag[] AllTags = (PartTag[])Enum.GetValues(typeof(PartTag));
            public void Init()
            {
                scores = new float[AllTags.Length];
                for (int i = 0, length = partTags.Count; i < length; i++)
                {
                    scores[(int)partTags[i].tagType] += partTags[i].value;
                }
            }

            public float GetDistance(float[] values)
            {
                float distance = 0;
                for (int i = 0, length = values.Length; i < length; i++)//unified array or list
                {
                    distance = 100 * math.abs(values[i] - scores[i]);
                }
                return distance;
            }
        }

        public List<SpecialFusionData> specialFusions = new();
        public List<FusionData> fusionDatas = new();

        public void Init()
        {
            for (int i = 0, length = fusionDatas.Count; i < length; i++)
            {
                fusionDatas[i].Init();
            }
        }

        public List<EffectBase> GetClosestFusionEffect(List<GolemPart> fuse)
        {
            for (int i = 0, length = specialFusions.Count; i < length; i++)
            {
                if (specialFusions[i].IsTarget(fuse))
                {
                    return specialFusions[i].effects;
                }
            }

            float distance = math.INFINITY;
            int targetIndex = 0;
            float[] scores = new float[FusionData.AllTags.Length];
            
            for (int i = 0, length = fuse.Count; i < length; i++)
            {
                var p = fuse[i];
                for (int j = 0, num = fuse[i].partTagValues.Count; j < num; j++)
                {
                    scores[(int)p.partTagValues[j].tagType] += p.partTagValues[j].value;
                }
            }

            for (int i = 0, length = fusionDatas.Count; i < length; i++)
            {
                var d = fusionDatas[i].GetDistance(scores);
                if(d < distance)
                {
                    distance = d;
                    targetIndex = i;
                }
            }
            return fusionDatas[targetIndex].effects;
        }
    }
}
