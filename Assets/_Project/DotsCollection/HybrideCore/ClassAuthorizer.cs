using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{

    public class ClassAuthorizer<T1> : MonoBehaviour where T1 : ClassComponentData, IComponentData, new()
    {
        [Header("Must enabledOnly once incase add multiple class baker")]
        public bool addSetupClassComponent = true;
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;

    }


    public abstract class ClassBaker<T, T1> : Baker<T> where T : ClassAuthorizer<T1>
    where T1 : ClassComponentData, IComponentData, new()
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponentObject(e, authoring.data1);
            if (authoring.addSetupClassComponent)
                AddComponent(e, new SetupClassComponent());
        }
    }

    public class ClassAuthorizer<T1, T2> : MonoBehaviour
    where T1 : ClassComponentData, IComponentData, new()
    where T2 : ClassComponentData, IComponentData, new()
    {
        [Header("Must enabledOnly once incase add multiple class baker")]
        public bool addSetupClassComponent = true;
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;
        public T2 data2;

    }


    public abstract class ClassBaker<T, T1, T2> : Baker<T> where T : ClassAuthorizer<T1, T2>
    where T1 : ClassComponentData, IComponentData, new()
    where T2 : ClassComponentData, IComponentData, new()
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponentObject(e, authoring.data1);
            AddComponentObject(e, authoring.data2);
            if (authoring.addSetupClassComponent)
                AddComponent(e, new SetupClassComponent());
        }
    }


    public class ClassAuthorizer<T1, T2, T3> : MonoBehaviour
    where T1 : ClassComponentData, IComponentData, new()
    where T2 : ClassComponentData, IComponentData, new()
    where T3 : ClassComponentData, IComponentData, new()
    {
        [Header("Must enabledOnly once incase add multiple class baker")]
        public bool addSetupClassComponent = true;
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;
        public T2 data2;
        public T3 data3;

    }


    public abstract class ClassBaker<T, T1, T2, T3> : Baker<T> where T : ClassAuthorizer<T1, T2, T3>
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
            if (authoring.addSetupClassComponent)
                AddComponent(e, new SetupClassComponent());
        }
    }

    public class ClassAuthorizer<T1, T2, T3, T4> : MonoBehaviour
     where T1 : ClassComponentData, IComponentData, new()
     where T2 : ClassComponentData, IComponentData, new()
     where T3 : ClassComponentData, IComponentData, new()
     where T4 : ClassComponentData, IComponentData, new()
    {
        [Header("Must enabledOnly once incase add multiple class baker")]
        public bool addSetupClassComponent = true;
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;
        public T2 data2;
        public T3 data3;
        public T4 data4;

    }


    public abstract class ClassBaker<T, T1, T2, T3, T4> : Baker<T> where T : ClassAuthorizer<T1, T2, T3, T4>
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
            if (authoring.addSetupClassComponent)
                AddComponent(e, new SetupClassComponent());
        }
    }

}
