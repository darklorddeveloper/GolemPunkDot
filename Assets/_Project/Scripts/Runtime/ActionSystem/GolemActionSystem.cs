using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [UpdateAfter(typeof(TopdownPlayerInputSystem))]
    [UpdateAfter(typeof(SetupClassSystem))]
    public partial class GolemActionSystem : SystemBase
    {
        private static int locomotionName = Animator.StringToHash("Locomotion");
        public static GolemActionSystem Instance;

        protected override void OnCreate()
        {
            base.OnCreate();
            Instance = this;
        }

        protected override void OnUpdate()
        {
            float delta = SystemAPI.Time.DeltaTime;
            foreach (var golemEntity in SystemAPI.Query<GolemEntity>())
            {
                var golem = golemEntity.golem;
                for (int i = 0, length = golem.attachedParts.Count; i < length; i++)
                {
                    golem.attachedParts[i].cooldownTimeCount += delta;
                }
            }
            foreach (var (golemEntity, input, entity) in SystemAPI.Query<GolemEntity, TopdownCharacterInput>().WithEntityAccess())
            {
                var golem = golemEntity.golem;

                var part = GetActivatingPart(input, golem);
                if (part != null)
                {
                    OnActivatePartSkill(entity, golem, part);
                }
                if (golem.runningEnumerator != null && golem.runningEnumerator.MoveNext())
                {
                    continue;
                }
                if (golem.runningEnumerator != null)
                {
                    golem.runningEnumerator = null;
                    golem.activatingPart = null;
                    golem.PlayAnimation(locomotionName);

                    var movement = EntityManager.GetComponentData<MovementSpeed>(entity);
                    movement.multiplier = 1.0f;
                    EntityManager.SetComponentData(entity, movement);
                }
            }
        }

        private void OnActivatePartSkill(Entity entity, Golem golem, GolemPart part)
        {
            if (part == null) return;
            if (part.cooldownTimeCount < part.cooldownTime)
            {
                return;
            }
            part.cooldownTimeCount = 0;

            GolemActionData actionData = part.actionDatas[0];
            if (golem.activatingPart == part)
            {
                golem.currentActionIndex++;
                int index = golem.currentActionIndex % part.actionDatas.Count;
                actionData = part.actionDatas[index];
            }
            else
            {
                golem.currentActionIndex = 0;
            }
            golem.activatingPart = part;
            //on activate effects runes and relics
            if (actionData.isHoldable)
            {
                golem.runningEnumerator = ChargeActionEnumerator(entity, golem, actionData);
            }
            else
            {
                golem.runningEnumerator = SimpleActionEnumerator(entity, golem, actionData);
            }
        }

        private IEnumerator ChargeActionEnumerator(Entity entity, Golem golem, GolemActionData golemActionData)
        {
            golem.PlayAnimation(golemActionData.startAnimationName, golemActionData.crossFade);
            float t = 0;
            golem.currentChargeRate = 0;
            var movement = EntityManager.GetComponentData<MovementSpeed>(entity);
            movement.multiplier = golemActionData.movement.Evaluate(golem.currentChargeRate);
            EntityManager.SetComponentData(entity, movement);
            while (t < golemActionData.totalPeriod && CheckHoldingActionKey(entity, golem))
            {
                t += SystemAPI.Time.DeltaTime;
                golem.currentChargeRate = t / golemActionData.totalPeriod;
                movement.multiplier = golemActionData.movement.Evaluate(golem.currentChargeRate);
                EntityManager.SetComponentData(entity, movement);
                yield return null;
            }
            golem.PlayAnimation(golemActionData.releaseAnimationName, golemActionData.crossFade);
            movement.multiplier = golemActionData.movement.Evaluate(1.0f);
            EntityManager.SetComponentData(entity, movement);
            ManualActivatePartEffect(entity, golem);
        }
        private IEnumerator SimpleActionEnumerator(Entity entity, Golem golem, GolemActionData golemActionData)
        {
            golem.PlayAnimation(golemActionData.startAnimationName, golemActionData.crossFade);
            float t = 0;
            while (t < golemActionData.activateDelayed)
            {
                t += SystemAPI.Time.DeltaTime;
                yield return null;
            }

            ManualActivatePartEffect(entity, golem);
            //cooldown time is ready can interupt
            while (t < golemActionData.totalPeriod)
            {
                t += SystemAPI.Time.DeltaTime;
                yield return null;
            }
        }

        private GolemPart GetActivatingPart(TopdownCharacterInput input, Golem golem)
        {
            GolemPart part = null;
            if (input.dashAction)
            {
                part = golem.GetPart(GolemPartType.Legs);
            }
            else if (input.primaryAction)
            {
                part = golem.GetPart(GolemPartType.Arms);
            }
            else if (input.secondaryAction)
            {
                part = golem.GetPart(GolemPartType.Head);
            }
            else if (input.skill1)
            {
                part = golem.GetPart(GolemPartType.Body);
            }
            else if (input.skill2)
            {
                part = golem.GetPart(GolemPartType.Core);
            }
            return part;
        }

        private bool CheckHoldingActionKey(Entity entity, Golem golem)
        {
            var input = EntityManager.GetComponentData<TopdownCharacterInput>(entity);
            if (golem.activatingPart == null)
            {
                return false;
            }
            switch (golem.activatingPart.partType)
            {
                case GolemPartType.Legs:
                    return input.isHoldingDashAction;
                case GolemPartType.Arms:
                    return input.isHoldingPrimaryAction;
                case GolemPartType.Head:
                    return input.isHoldingSecondaryAction;
                case GolemPartType.Body:
                    return input.isHoldingSkill1;
                case GolemPartType.Core:
                    return input.isHoldingSkill2;
            }
            return false;
        }

        private EffectTiming GetActivatingEffectTiming(Golem golem)
        {
            switch (golem.activatingPart.partType)
            {
                case GolemPartType.Legs:
                    return EffectTiming.ActiveDash;
                case GolemPartType.Arms:
                    return EffectTiming.ActivePrimary;
                case GolemPartType.Head:
                    return EffectTiming.ActiveSecondary;
                case GolemPartType.Body:
                    return EffectTiming.ActiveSpecial1;
                case GolemPartType.Core:
                    return EffectTiming.ActiveSpecial2;
                default:
                    return EffectTiming.ActivePrimary;
            }
        }

        private void ActivateAllPartEffect(Entity entity, Golem golem, EffectTiming effectTiming)
        {
            for (int i = 0, length = golem.attachedParts.Count; i < length; i++)
            {
                ActivatePartEffect(entity, golem, golem.attachedParts[i], effectTiming);
            }
        }
        private void ManualActivatePartEffect(Entity entity, Golem golem)
        {
            var part = golem.activatingPart;
            if (part == null) return;
            for (int i = part.linkedPart.Count - 1; i >= 0; i--)
            {
                if (part.linkedPart[i] == null)
                {
                    part.linkedPart.RemoveAt(i);
                }
            }
            if (part.linkedPart.Count <= 0)
            {
                golem.ActivatePartEffect(entity, EntityManager, part, EffectTiming.ManualActivate);
            }
            else
            {
                var parts = new List<GolemPart> { part };
                parts.AddRange(part.linkedPart);
                if (SystemAPI.ManagedAPI.TryGetSingleton<EffectFusionTableEntity>(out var table))
                {
                    var effects  =table.table.GetClosestFusionEffect(parts);
                    for (int i = 0, length = effects.Count; i < length; i++)
                    {
                        effects[i].OnActivate(entity, EntityManager);
                    }
                }
            }


            var timing = GetActivatingEffectTiming(golem);
            ActivateAllPartEffect(entity, golem, timing);
        }

        private void ActivatePartEffect(Entity entity, Golem golem, GolemPart part, EffectTiming effectTiming)
        {
            if (part == null) return;
            golem.ActivatePartEffect(entity, EntityManager, part, effectTiming);
        }

        public void ForceActivateAllEffects(Entity entity, Golem golem, GolemPart part)
        {
            golem.ForceActivatePartEffect(entity, EntityManager, part);
        }
    }
}
