using System.Collections;
using Unity.Entities;
using UnityEditor.Embree;
using UnityEngine;

namespace DarkLordGame
{
    [UpdateAfter(typeof(TopdownPlayerInputSystem))]
    public partial class GolemActionSystem : SystemBase
    {
        private static int locomotionName = Animator.StringToHash("Locomotion");
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
                    OnActivatePartAction(entity, golem, part);
                }
                if (golem.runningEnumerator != null && golem.runningEnumerator.MoveNext())
                {
                    continue;
                }
                if (golem.runningEnumerator != null)
                {
                    golem.runningEnumerator = null;
                    golem.previousActivatedPart = null;
                    golem.activatingPart = null;
                    golem.PlayAnimation(locomotionName);

                    var movement = EntityManager.GetComponentData<MovementSpeed>(entity);
                    movement.multiplier = 1.0f;
                    EntityManager.SetComponentData(entity, movement);
                }
            }
        }


        private void OnActivatePartAction(Entity entity, Golem golem, GolemPart part)
        {
            if (part == null) return;
            if (part.cooldownTimeCount < part.cooldownTime)
            {
                return;
            }
            part.cooldownTimeCount = 0;
            bool hasManualEffect = false;
            for (int i = 0, length = part.effects.Count; i < length; i++)
            {
                if (part.effects[i].effectTiming == EffectTiming.ManualActivate)
                {
                    hasManualEffect = true;
                    break;
                }
            }

            if (hasManualEffect == false) return;


            GolemActionData actionData = part.actionDatas[0];
            if (golem.previousActivatedPart == part)
            {
                part.currentActionIndex++;
                part.currentActionIndex %= part.actionDatas.Count;
                actionData = part.actionDatas[part.currentActionIndex];
            }
            golem.previousActivatedPart = golem.activatingPart;
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

            ActivatePartEffect(entity, golem, EffectTiming.ManualActivate);
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

            ActivatePartEffect(entity, golem, EffectTiming.ManualActivate);
            //cooldown time is ready can interupt
            while (t < golemActionData.totalPeriod)
            {
                t += SystemAPI.Time.DeltaTime;
                yield return null;
            }
        }

        private GolemPart GetActivatingPart(TopdownCharacterInput input, Golem golem)
        {
            if (input.dashAction)
            {
                return golem.GetPart(GolemPartType.Legs);
            }
            else if (input.primaryAction)
            {
                return golem.GetPart(GolemPartType.RightArms);
            }
            else if (input.secondaryAction)
            {
                return golem.GetPart(GolemPartType.LeftArms);
            }
            else if (input.skill1)
            {
                return golem.GetPart(GolemPartType.Head);
            }
            else if (input.skill2)
            {
                return golem.GetPart(GolemPartType.Body);
            }
            return null;
        }
        private bool CheckHoldingActionKey(Entity entity, Golem golem)
        {
            var input = EntityManager.GetComponentData<TopdownCharacterInput>(entity);
            Debug.Log("input holding  --- " + input.isHoldingDashAction);
            switch (golem.activatingPart.partType)
            {
                case GolemPartType.Legs:
                    return input.isHoldingDashAction;
                case GolemPartType.RightArms:
                    return input.isHoldingPrimaryAction;
                case GolemPartType.LeftArms:
                    return input.isHoldingSecondaryAction;
                case GolemPartType.Head:
                    return input.isHoldingSkill1;
                case GolemPartType.Body:
                    return input.isHoldingSkill2;
            }
            return false;
        }

        private void ActivatePartEffect(Entity entity, Golem golem, EffectTiming effectTiming)
        {
            var part = golem.activatingPart;

            for (int i = 0, length = part.effects.Count; i < length; i++)
            {
                if (part.effects[i].effectTiming == effectTiming)
                {
                    part.effects[i].OnActivate(entity, EntityManager);
                }
            }

            for (int i = 0, length = part.runes.Count; i < length; i++)
            {
                if (part.runes[i].effect.effectTiming == effectTiming)
                {
                    part.runes[i].effect.OnActivate(entity, EntityManager);
                }
            }
        }
    }
}
