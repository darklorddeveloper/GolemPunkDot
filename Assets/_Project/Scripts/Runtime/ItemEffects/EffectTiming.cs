using UnityEngine;

namespace DarkLordGame
{
    public enum EffectTiming
    {
        ManualActivate = 0,
        ActivePrimary = 1, //sort on frequency
        ActiveSecondary = 2,
        ActiveDash = 3,
        ActiveSkill1 = 4,
        ActiveSkill2 = 5,
        ActiveSkill3 = 5,
        OnAttack = 10,
        OnTakeDamage = 20,
        OnDeath = 21,
        OnTakeCriticalDamage = 22,
        OnCrit = 30,
        OnDealDamage = 40,
        OnMoved = 50,
        OnDash = 51,
        OnHeal = 60,
        OnOtherActive = 60,

        OnChargingSkill = 70,
        OnActivateChargingEffect,
        OnEquiped = 80,
        OnUnequiped = 81,

        Always = 100,

    }
}
