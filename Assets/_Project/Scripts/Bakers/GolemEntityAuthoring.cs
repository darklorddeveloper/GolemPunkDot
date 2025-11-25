using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class GolemEntityAuthoring : ClassAuthorizer<GolemEntity>
    {
    }

    public class GolemEntityBaker : ClassBaker<GolemEntityAuthoring, GolemEntity>
    {

    }

   
}
