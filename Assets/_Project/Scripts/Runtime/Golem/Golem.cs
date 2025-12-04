using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

namespace DarkLordGame
{
    public enum GolemSkillType
    {
        Primary,
        Secondary,
        Dash,
        Special1,
        Special2,
    }
    public class Golem : MonoBehaviour
    {
        public Transform skinnedRoot;
        public SkinnedMeshRenderer originalSkinnedMesh;
        public List<GolemPart> attachedParts;
        public Animator animator;
        public float lerpLocomotionSpeed = 0.3f;
        public List<GolemAttachPointData> allAttachPoints = new();
        public GolemSkillType activatingSkillType;
        public Skill activatingSkill;
        public float currentChargeRate;
        public AttackRequestData currentRequestData;
        public IEnumerator runningEnumerator;
        public int currentActionIndex;
        public List<Skill> skills = new();
        public List<Skill> skillPools = new();

        public static int maxSkills = 4;
        public void Init()
        {
            for (int i = 0, length = attachedParts.Count; i < length; i++)
            {
                attachedParts[i] = SetupPart(attachedParts[i]);
            }

            for (int i = 0, length = skills.Count; i < length; i++)
            {
                skills[i] = ScriptableObject.Instantiate(skills[i]);
                skills[i].isInstance = true;
            }
        }

        public void AddSkill(Skill skill)
        {
            var instance = ScriptableObject.Instantiate(skill);
            instance.isInstance = true;
            skillPools.Add(instance);

            if (skills.Count < maxSkills)
            {
                skills.Add(instance);
                return;
            }
        }

        public Skill GetActiveSkill(int skillID)
        {
            if (skillID >= skills.Count)
            {
                return null;
            }
            return skills[skillID];
        }

        public void SetSkillAtIndex(Skill skill, int index)
        {
            if (skill.isInstance == false)
            {
                skill = ScriptableObject.Instantiate(skill);
                skill.isInstance = true;
            }

            if (index < skills.Count)
            {
                var temp = skills[index];
                if (temp == skill)
                {
                    return;
                }
                if (skills.Contains(skill))
                {
                    var i = skills.IndexOf(skill);
                    skills[index] = skill;
                    skills[i] = temp;
                    return;
                }
                skills[index] = skill;
            }
            else
            {
                Debug.Log("setting skill over range");
            }
        }

        void OnDestroy()
        {

            for (int i = 0, length = attachedParts.Count; i < length; i++)
            {
                if (attachedParts[i] != null && attachedParts[i].isInstance)//isinstance is prevent destroying original source
                {
                    ScriptableObject.Destroy(attachedParts[i]);
                }
            }
        }

        public Transform GetAttachPoint(GolemAttachPoint point)
        {
            for (int i = 0, length = allAttachPoints.Count; i < length; i++)
            {
                if (allAttachPoints[i].point == point)
                {
                    return allAttachPoints[i].transform;
                }
            }
            return skinnedRoot;
        }

        private GolemPart SetupPart(GolemPart part)
        {
            var instance = ScriptableObject.Instantiate(part);
            instance.isInstance = true;
            instance.original = part;
            instance.Init();

            var obj = new GameObject();
            if (instance.isUsingAttachPoint)
            {
                var filter = obj.AddComponent<MeshFilter>();
                var render = obj.AddComponent<MeshRenderer>();
                filter.sharedMesh = instance.mesh;
                render.sharedMaterials = instance.materials;
                //find point then attach
                obj.transform.SetParent(transform, GetAttachPoint(instance.attachPoint));
            }
            else
            {
                obj.transform.SetParent(skinnedRoot);

                var skinned = obj.AddComponent<SkinnedMeshRenderer>();
                skinned.sharedMaterials = instance.materials;
                skinned.sharedMesh = instance.mesh;
                skinned.rootBone = originalSkinnedMesh.rootBone;
                skinned.bones = originalSkinnedMesh.bones;
            }
            // skinned.sharedMesh.RecalculateBounds();
            instance.runtimeGameObject = obj;
            return instance;
        }

        public bool ChangePart(GolemPart golemPart)
        {
            for (int i = 0, length = attachedParts.Count; i < length; i++)
            {
                if (attachedParts[i].partType == golemPart.partType)
                {
                    if (attachedParts[i].runtimeGameObject != null)
                    {
                        GameObject.Destroy(attachedParts[i].runtimeGameObject);
                    }
                    if (attachedParts[i].isInstance)
                    {
                        ScriptableObject.Destroy(attachedParts[i]);
                    }
                    attachedParts[i] = SetupPart(golemPart);
                    return true;
                }
            }
            return false;
        }

        public void AddPart(GolemPart part)
        {
            if (ChangePart(part))
            {
                return;
            }
            SetupPart(part);
            attachedParts.Add(part);
        }

        public void PlayAnimation(string animationName, float crossFade = 0.0f)
        {
            if (crossFade <= 0)
            {
                animator.Play(animationName, 0, 0);
            }
            else
            {
                animator.CrossFade(animationName, crossFade);
            }
        }

        public void PlayAnimation(int animationHash)
        {
            animator.Play(animationHash, 0, 0);
        }

        public GolemPart GetPart(GolemPartType golemPartType)
        {
            for (int i = 0, length = attachedParts.Count; i < length; i++)
            {
                if (attachedParts[i].partType == golemPartType)
                {
                    return attachedParts[i];
                }
            }
            return null;
        }

        public void ActiveAllEffects(Entity entity, EntityManager entityManager, EffectTiming effectTiming)
        {
            for (int k = 0, num = attachedParts.Count; k < num; k++)
            {
                var part = attachedParts[k];

                for (int i = 0, length = part.effects.Count; i < length; i++)
                {
                    if (part.effects[i].effectTiming == effectTiming)
                    {
                        part.effects[i].OnActivate(entity, entityManager);
                    }
                }

                for (int i = 0, length = part.runes.Count; i < length; i++)
                {
                    if (part.runes[i].effect.effectTiming == effectTiming)
                    {
                        part.runes[i].effect.OnActivate(entity, entityManager);
                    }
                }

            }
        }
    }
}
