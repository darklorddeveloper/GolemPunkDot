using UnityEngine;

namespace DarkLordGame
{
    public enum EffectTiming
    {
        ActivePrimary = 0, //sort on frequency
        ActiveSecondary = 1,
        ActiveDash = 2,
        ActiveSpecial1 = 3,
        ActiveSpecial2 = 4,
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

        Always = 100,

    }
}
