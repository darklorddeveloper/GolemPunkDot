using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    //only work with prefabs
    public class ClassComponentData : IComponentData
    {
        public bool initialized = false;//check for rush systel
        public virtual void Init()
        {
            initialized = true; 
            //instantiate self prefab and assign
        }
    }

    public struct SetupClassComponent : IComponentData, IEnableableComponent
    {

    }

    public class ClassAuthorizer<T1> : MonoBehaviour where T1 : ClassComponentData, IComponentData, new()
    {
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;

    }


    public class ClassBaker<T, T1> : Baker<T> where T : ClassAuthorizer<T1>
    where T1 : ClassComponentData, IComponentData, new()
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponentObject(e, authoring.data1);
            AddComponent(e, new SetupClassComponent());

        }
    }

    public class ClassAuthorizer<T1, T2> : MonoBehaviour
    where T1 : ClassComponentData, IComponentData, new()
    where T2 : ClassComponentData, IComponentData, new()
    {
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;
        public T2 data2;

    }


    public class ClassBaker<T, T1, T2> : Baker<T> where T : ClassAuthorizer<T1, T2>
    where T1 : ClassComponentData, IComponentData, new()
    where T2 : ClassComponentData, IComponentData, new()
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponentObject(e, authoring.data1);
            AddComponentObject(e, authoring.data2);
            AddComponent(e, new SetupClassComponent());
        }
    }


    public class ClassAuthorizer<T1, T2, T3> : MonoBehaviour
    where T1 : ClassComponentData, IComponentData, new()
    where T2 : ClassComponentData, IComponentData, new()
    where T3 : ClassComponentData, IComponentData, new()
    {
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;
        public T2 data2;
        public T3 data3;

    }


    public class ClassBaker<T, T1, T2, T3> : Baker<T> where T : ClassAuthorizer<T1, T2, T3>
    where T1 : ClassComponentData, IComponentData, new()
    where T2 : ClassComponentData, IComponentData, new()
    where T3 : ClassComponentData, IComponentData, new()
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponentObject(e, authoring.data1);
            AddComponentObject(e, authoring.data2);
            AddComponentObject(e, authoring.data3);
            AddComponent(e, new SetupClassComponent());
        }
    }

    public class ClassAuthorizer<T1, T2, T3, T4> : MonoBehaviour
     where T1 : ClassComponentData, IComponentData, new()
     where T2 : ClassComponentData, IComponentData, new()
     where T3 : ClassComponentData, IComponentData, new()
     where T4 : ClassComponentData, IComponentData, new()
    {
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;
        public T2 data2;
        public T3 data3;
        public T4 data4;

    }


    public class ClassBaker<T, T1, T2, T3, T4> : Baker<T> where T : ClassAuthorizer<T1, T2, T3, T4>
    where T1 : ClassComponentData, IComponentData, new()
    where T2 : ClassComponentData, IComponentData, new()
    where T3 : ClassComponentData, IComponentData, new()
    where T4 : ClassComponentData, IComponentData, new()
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponentObject(e, authoring.data1);
            AddComponentObject(e, authoring.data2);
            AddComponentObject(e, authoring.data3);
            AddComponentObject(e, authoring.data4);
            AddComponent(e, new SetupClassComponent());
        }
    }

}
