using System;
using Unity.Burst;
using Unity.Entities;
using UnityEditorInternal;
using UnityEngine;

namespace DarkLordGame
{
    public partial class CreateTempStatSystem : SystemBase
    {
        private EntityArchetype componentTypeSet;
        public static Action<TempStat, Entity, float> onCreateTempStat;

        protected override void OnCreate()
        {
            base.OnCreate();
            onCreateTempStat = OnCreateTempStat;
            componentTypeSet = EntityManager.CreateArchetype(typeof(TempStat),
            typeof(ApplyTempStatFlag),
            typeof(SafeDestroyComponent),
            typeof(DestroyImmediate));

        }

        private void OnCreateTempStat(TempStat stat, Entity target, float period)
        {
            var e = EntityManager.CreateEntity(componentTypeSet);
            stat.target = target;
            EntityManager.SetComponentData(e, stat);
            EntityManager.SetComponentData(e, new SafeDestroyComponent
            {
                period = period,
            });
            EntityManager.SetComponentEnabled<DestroyImmediate>(e, false);
        }

        protected override void OnUpdate()
        {

        }
    }


    [BurstCompile]

    public partial struct ApplyTempStatSystem : ISystem
    {
        private ComponentLookup<Unit> unitLookup;
        public void OnCreate(ref SystemState state)
        {
            unitLookup = state.GetComponentLookup<Unit>();
        }

        public void OnUpdate(ref SystemState state)
        {
            unitLookup.Update(ref state);
            foreach (var (tempstat, applyFlag, e) in SystemAPI.Query<TempStat, ApplyTempStatFlag>().WithEntityAccess())
            {
                if (unitLookup.TryGetComponent(tempstat.target, out var unit))
                {
                    unit.tempAttack += tempstat.tempAttack;
                    unit.tempAttackMultiplier += tempstat.tempAttackMultiplier;
                    unit.tempShield += tempstat.tempShield;
                    unit.tempCooldownSpeed += tempstat.tempCooldownSpeed;
                    unit.tempCriticalChance += tempstat.tempCriticalChance;
                    unit.tempCriticalDamage += tempstat.tempCriticalDamage;
                    unit.tempMovementSpeed += tempstat.tempMovementSpeed;
                    state.EntityManager.SetComponentData(tempstat.target, unit);//update unit
                }
                state.EntityManager.SetComponentEnabled<ApplyTempStatFlag>(e, false);
            }
        }
    }

    [BurstCompile]
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    [UpdateBefore(typeof(CleanUpDestroySystem))]
    public partial struct RemoveTempStatSystem : ISystem
    {
        private ComponentLookup<Unit> unitLookup;
        public void OnCreate(ref SystemState state)
        {
            unitLookup = state.GetComponentLookup<Unit>();
        }

        public void OnUpdate(ref SystemState state)
        {
            unitLookup.Update(ref state);


            foreach (var (tempstat, destroyFlag, e) in SystemAPI.Query<TempStat, DestroyImmediate>().WithEntityAccess())
            {
                if (unitLookup.TryGetComponent(tempstat.target, out var unit))
                {
                    unit.tempAttack -= tempstat.tempAttack;
                    unit.tempAttackMultiplier -= tempstat.tempAttackMultiplier;
                    unit.tempShield -= tempstat.tempShield;
                    unit.tempCooldownSpeed -= tempstat.tempCooldownSpeed;
                    unit.tempCriticalChance -= tempstat.tempCriticalChance;
                    unit.tempCriticalDamage -= tempstat.tempCriticalDamage;
                    unit.tempMovementSpeed -= tempstat.tempMovementSpeed;
                    state.EntityManager.SetComponentData(tempstat.target, unit);//update unit
                }
            }
        }
    }
}
