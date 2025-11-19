using UnityEngine;

namespace DarkLordGame
{
    public enum EffectTiming
    {
        ManualActivate = 0, //sort on frequency
        OnAttack = 10,
        OnAttackWithCurrentPart,// for runes only not include relics
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
