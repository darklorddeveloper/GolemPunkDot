using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DarkLordGame
{
    [UpdateBefore(typeof(DamageSystem))]
    public partial class DamageTextSystem : SystemBase
    {
        private DamageTextRootEntity damageTextRootEntity;
        private MainCamera mainCamera;
        private List<DamageText> runningDamageTexts = new();
        private List<DamageText> sleepingDamageTexts = new();
        private const int prePool = 20;
        private EntityQuery entityQuery;
        
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<DamageTextRootEntity>();
            RequireForUpdate<Damage>();
            entityQuery = SystemAPI.QueryBuilder().WithAll<Damage, LocalTransform>().Build();
        }

        protected override void OnUpdate()
        {
            if (mainCamera == null)
            {
                mainCamera = SystemAPI.ManagedAPI.GetSingleton<MainCamera>();
            }
            if (damageTextRootEntity == null || damageTextRootEntity.initialized == false)
            {
                damageTextRootEntity = SystemAPI.ManagedAPI.GetSingleton<DamageTextRootEntity>();
                if (damageTextRootEntity == null || damageTextRootEntity.initialized == false)
                {
                    damageTextRootEntity = null;
                    return;
                }

                for (int i = 0; i < prePool; i++)
                {
                    var damage = GameObject.Instantiate<DamageText>(damageTextRootEntity.prefab, damageTextRootEntity.root);
                    PutDamageTextToSleep(damage);
                }
            }

            var damages = entityQuery.ToComponentDataArray<Damage>(Allocator.Temp);
            if (damages.Length > 0)
            {
                var damageTexts = GetTextDamageInstance(damages.Length);
                for (int i = 0, length = damageTexts.Count; i < length; i++)
                {
                    damageTexts[i].gameObject.SetActive(true);
                    damageTexts[i].timeCount = 0;
                    damageTexts[i].root.position = CalculatePosition(damages[i].damagePosition);
                    damageTexts[i].text.SetText(damages[i].attack.damage.ToString());
                    damageTexts[i].startPosition = damageTexts[i].root.anchoredPosition;
                }
                runningDamageTexts.AddRange(damageTexts);
            }
            float deltTime = SystemAPI.Time.DeltaTime;
            for (int i = runningDamageTexts.Count - 1; i >= 0; i--)
            {
                runningDamageTexts[i].timeCount += deltTime;
                var pos = runningDamageTexts[i].startPosition;
                float t = runningDamageTexts[i].timeCount / damageTextRootEntity.period;
                pos.y = math.lerp(pos.y, pos.y + damageTextRootEntity.height, t);
                runningDamageTexts[i].root.anchoredPosition = pos;
                var col = runningDamageTexts[i].text.color;
                var period = damageTextRootEntity.period / 2;
                col.a = math.max(0, 1 - math.abs((t - period) / (period - damageTextRootEntity.period)));
                runningDamageTexts[i].text.color = col;
                if(runningDamageTexts[i].timeCount >= damageTextRootEntity.period)
                {
                    PutDamageTextToSleep(runningDamageTexts[i]);
                    runningDamageTexts.RemoveAt(i);
                }
            }
        }

        private Vector3 CalculatePosition(float3 pos)
        {
            return mainCamera.camera.WorldToScreenPoint(pos);
        }

        private void SetActiveDamageText(List<DamageText> damageTexts, NativeArray<Damage> localTransforms)
        {

        }

        private List<DamageText> GetTextDamageInstance(int numbers)
        {
            List<DamageText> damageTexts;
            if (sleepingDamageTexts.Count < numbers)
            {
                damageTexts = new();
                damageTexts.AddRange(sleepingDamageTexts);
                numbers -= sleepingDamageTexts.Count;
                for (int i = 0; i < numbers; i++)
                {
                    var instance = GameObject.Instantiate<DamageText>(damageTextRootEntity.prefab, damageTextRootEntity.root);
                    damageTexts.Add(instance);
                }
            }
            else
            {
                damageTexts = sleepingDamageTexts.GetRange(sleepingDamageTexts.Count - numbers, numbers);
                sleepingDamageTexts.RemoveRange(sleepingDamageTexts.Count - numbers, numbers);
            }

            return damageTexts;
        }

        private void PutDamageTextToSleep(DamageText text)
        {
            text.gameObject.SetActive(false);
            sleepingDamageTexts.Add(text);
        }
    }
}
