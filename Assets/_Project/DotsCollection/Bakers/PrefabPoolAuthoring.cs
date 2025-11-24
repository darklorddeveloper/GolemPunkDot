using System.Collections.Generic;
using UnityEngine;
namespace DarkLordGame
{
    public class PrefabPoolAuthoring : MonoBehaviour
    {
        public List<GameObject> prefabs = new List<GameObject>();

        [Header("Order keeping is faster")]
        public List<GameObject> orderKeeping = new List<GameObject>();
    }
}