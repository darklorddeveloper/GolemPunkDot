using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class QuickAuthorizationClassData<T1> : MonoBehaviour where T1 : class, IComponentData, new()
    {
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;

    }


    public class QuickAuthorizationClassBaker<T, T1> : Baker<T> where T : QuickAuthorizationClassData<T1>
    where T1 : class, IComponentData, new()
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponentObject(e, authoring.data1);
        }
    }

    public class QuickAuthorizationClassData<T1, T2> : MonoBehaviour
    where T1 : class, IComponentData, new()
    where T2 : class, IComponentData, new()
    {
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;
        public T2 data2;

    }


    public class QuickAuthorizationClassBaker<T, T1, T2> : Baker<T> where T : QuickAuthorizationClassData<T1, T2>
    where T1 : class, IComponentData, new()
    where T2 : class, IComponentData, new()
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponentObject(e, authoring.data1);
            AddComponentObject(e, authoring.data2);
        }
    }


    public class QuickAuthorizationClassData<T1, T2, T3> : MonoBehaviour
    where T1 : class, IComponentData, new()
    where T2 : class, IComponentData, new()
    where T3 : class, IComponentData, new()
    {
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;
        public T2 data2;
        public T3 data3;

    }


    public class QuickAuthorizationClassBaker<T, T1, T2, T3> : Baker<T> where T : QuickAuthorizationClassData<T1, T2, T3>
    where T1 : class, IComponentData, new()
    where T2 : class, IComponentData, new()
    where T3 : class, IComponentData, new()
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponentObject(e, authoring.data1);
            AddComponentObject(e, authoring.data2);
            AddComponentObject(e, authoring.data3);
        }
    }
    
    public class QuickAuthorizationClassData<T1, T2, T3, T4> : MonoBehaviour
     where T1 : class, IComponentData, new()
     where T2 : class, IComponentData, new()
     where T3 : class, IComponentData, new()
     where T4 : class, IComponentData, new()
    {
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;
        public T2 data2;
        public T3 data3;
        public T4 data4;

    }
    

    public class QuickAuthorizationClassBaker<T, T1, T2, T3, T4> : Baker<T> where T : QuickAuthorizationClassData<T1, T2, T3, T4>
    where T1 : class, IComponentData, new()
    where T2 : class, IComponentData, new()
    where T3 : class, IComponentData, new()
    where T4 : class, IComponentData, new()
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponentObject(e, authoring.data1);
            AddComponentObject(e, authoring.data2);
            AddComponentObject(e, authoring.data3);
            AddComponentObject(e, authoring.data4);
        }
    }

}
