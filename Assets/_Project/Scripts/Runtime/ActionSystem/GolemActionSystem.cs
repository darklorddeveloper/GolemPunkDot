using System.Collections;
using Unity.Entities;
using UnityEditor.Embree;
using UnityEngine;

namespace DarkLordGame
{
    [UpdateAfter(typeof(TopdownPlayerInputSystem))]
    [UpdateAfter(typeof(SetupClassSystem))]
    public partial class GolemActionSystem : SystemBase
    {
        private static int locomotionName = Animator.StringToHash("Locomotion");
        protected override void OnUpdate()
        {
            float delta = SystemAPI.Time.DeltaTime;
            foreach (var golemEntity in SystemAPI.Query<GolemEntity>())
            {
                var golem = golemEntity.golem;
                for (int i = 0, length = golem.skills.Count; i < length; i++)
                {
                    golem.skills[i].cooldownTimeCount += delta;
                }
            }
            foreach (var (golemEntity, input, entity) in SystemAPI.Query<GolemEntity, TopdownCharacterInput>().WithEntityAccess())
            {
                var golem = golemEntity.golem;

                var skill = GetActivatingSkill(input, golem);
                if (skill != null)
                {
                    OnActivateSkill(entity, golem, skill);
                }
                if (golem.runningEnumerator != null && golem.runningEnumerator.MoveNext())
                {
                    continue;
                }
                if (golem.runningEnumerator != null)
                {
                    golem.runningEnumerator = null;
                    golem.activatingSkill = null;
                    golem.PlayAnimation(locomotionName);

                    var movement = EntityManager.GetComponentData<MovementSpeed>(entity);
                    movement.multiplier = 1.0f;
                    EntityManager.SetComponentData(entity, movement);
                }
            }
        }


        private void OnActivateSkill(Entity entity, Golem golem, Skill skill)
        {
            if (skill == null) return;
            if (skill.cooldownTimeCount < skill.cooldownTime)
            {
                return;
            }
            skill.cooldownTimeCount = 0;

            GolemActionData actionData = skill.actionDatas[0];
            if (golem.activatingSkill == skill)
            {
                golem.currentActionIndex++;
                int index = golem.currentActionIndex % skill.actionDatas.Count;
                actionData = skill.actionDatas[index];
            }
            else
            {
                golem.currentActionIndex = 0;
            }
            golem.activatingSkill = skill;
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
            var timing = GetActivatingEffectTiming(golem);
            ActivateSkill(entity, golem, timing);
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
            var timing = GetActivatingEffectTiming(golem);
            ActivateSkill(entity, golem, timing);
            //cooldown time is ready can interupt
            while (t < golemActionData.totalPeriod)
            {
                t += SystemAPI.Time.DeltaTime;
                yield return null;
            }
        }

        private Skill GetActivatingSkill(TopdownCharacterInput input, Golem golem)
        {
            int index = -1;
            if (input.dashAction)
            {
                index = (int)GolemSkillType.Dash;
            }
            else if (input.primaryAction)
            {
                index = (int)GolemSkillType.Primary;
            }
            else if (input.secondaryAction)
            {
                index = (int)GolemSkillType.Secondary;
            }
            else if (input.skill1)
            {
                index = (int)GolemSkillType.Special1;
            }
            else if (input.skill2)
            {
                index = (int)GolemSkillType.Special2;
            }
            if (index != -1)
            {
                return golem.GetActiveSkill(index);
            }
            return null;
        }
        private bool CheckHoldingActionKey(Entity entity, Golem golem)
        {
            var input = EntityManager.GetComponentData<TopdownCharacterInput>(entity);
            switch (golem.activatingSkillType)
            {
                case GolemSkillType.Dash:
                    return input.isHoldingDashAction;
                case GolemSkillType.Primary:
                    return input.isHoldingPrimaryAction;
                case GolemSkillType.Secondary:
                    return input.isHoldingSecondaryAction;
                case GolemSkillType.Special1:
                    return input.isHoldingSkill1;
                case GolemSkillType.Special2:
                    return input.isHoldingSkill2;
            }
            return false;
        }

        private EffectTiming GetActivatingEffectTiming(Golem golem)
        {
            switch (golem.activatingSkillType)
            {
                case GolemSkillType.Dash:
                    return EffectTiming.ActiveDash;
                case GolemSkillType.Primary:
                    return EffectTiming.ActivePrimary;
                case GolemSkillType.Secondary:
                    return EffectTiming.ActiveSecondary;
                case GolemSkillType.Special1:
                    return EffectTiming.ActiveSpecial1;
                case GolemSkillType.Special2:
                    return EffectTiming.ActiveSpecial2;
                default:
                    return EffectTiming.ActivePrimary;
            }
        }

        private void ActivateSkill(Entity entity, Golem golem, EffectTiming effectTiming)
        {
            var skill = golem.activatingSkill;
            if (skill.preActivateEffect != null)
            {
                skill.preActivateEffect.OnActivate(entity, EntityManager);
            }

            golem.ActiveAllEffects(entity, EntityManager, effectTiming);
            for (int i = 0, length = skill.chainedEffects.Count; i < length; i++)
            {
                skill.chainedEffects[i].OnActivate(entity, EntityManager);
            }
            if (skill.postActivateEffect != null)
            {
                skill.postActivateEffect.OnActivate(entity, EntityManager);
            }

        }
    }
}
