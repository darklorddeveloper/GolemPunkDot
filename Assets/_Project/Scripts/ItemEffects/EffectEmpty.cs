using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [CreateAssetMenu(fileName = "Effect_Empty", menuName = "Effect/Empty")]
    public class EffectEmpty : EffectBase
    {
        public override void OnActivate(Entity activator, EntityManager entityManager)
        {
        }
    }
}
