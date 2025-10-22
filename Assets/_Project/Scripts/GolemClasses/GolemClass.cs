using System.Collections.Generic;
using UnityEngine;

namespace DarkLordGame
{
    [CreateAssetMenu(fileName = "GolemClass", menuName = "Create new Class")]
    public class GolemClass : ScriptableObject
    {
        public int classID; // we will use from entity to spawn
    }
}
