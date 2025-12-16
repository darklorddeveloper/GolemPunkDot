using UnityEngine;

namespace DarkLordGame
{
    public class DamageTimeAuthoring : EnableStructAuthorizer<DamageTime>
    {
    }

    public class DamageTimeBaker : EnableStructBaker<DamageTimeAuthoring, DamageTime>
    {

    }
}
