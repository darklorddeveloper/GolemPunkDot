using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public class DamageTextRootEntity : ClassComponentData
    {
        public RectTransform root;
        public Canvas canvas;
        public DamageText prefab;
        public Vector2 startPosition;
        public float period = 0.8f;
        public float height;
        public float fadePeriod = 0.1f;
        // public 
        public override void Init(Entity entity, EntityManager manager)
        {
            base.Init(entity, manager);
            root = GameObject.Instantiate(root);
            canvas =root.GetComponent<Canvas>();
        }
    }
}
