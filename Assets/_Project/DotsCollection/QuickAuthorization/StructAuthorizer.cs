using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class StructAuthorizer<T1> : MonoBehaviour where T1 : unmanaged, IComponentData
    {
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;

    }

    public abstract class StructBaker<T, T1> : Baker<T> where T : StructAuthorizer<T1>
    where T1 : unmanaged, IComponentData
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponent(e, authoring.data1);
        }
    }
    public class StructAuthorizer<T1, T2> : MonoBehaviour
    where T1 : unmanaged, IComponentData
    where T2 : unmanaged, IComponentData
    {
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;

        public T2 data2;
    }

    public abstract class StructBaker<T, T1, T2> : Baker<T> where T : StructAuthorizer<T1, T2>
    where T1 : unmanaged, IComponentData
    where T2 : unmanaged, IComponentData
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponent(e, authoring.data1);
            AddComponent(e, authoring.data2);
        }
    }

    public class StructAuthorizer<T1, T2, T3> : MonoBehaviour
    where T1 : unmanaged, IComponentData
    where T2 : unmanaged, IComponentData
    where T3 : unmanaged, IComponentData
    {
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;

        public T2 data2;
        public T3 data3;
    }


    public abstract class StructBaker<T, T1, T2, T3> : Baker<T> where T : StructAuthorizer<T1, T2, T3>
    where T1 : unmanaged, IComponentData
    where T2 : unmanaged, IComponentData
    where T3 : unmanaged, IComponentData
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponent(e, authoring.data1);
            AddComponent(e, authoring.data2);
            AddComponent(e, authoring.data3);
        }
    }

    public class StructAuthorizer<T1, T2, T3, T4> : MonoBehaviour
    where T1 : unmanaged, IComponentData
    where T2 : unmanaged, IComponentData
    where T3 : unmanaged, IComponentData
    where T4 : unmanaged, IComponentData
    {
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;

        public T2 data2;
        public T3 data3;
        public T4 data4;
    }


    public abstract class StructBaker<T, T1, T2, T3, T4> : Baker<T> where T : StructAuthorizer<T1, T2, T3, T4>
    where T1 : unmanaged, IComponentData
    where T2 : unmanaged, IComponentData
    where T3 : unmanaged, IComponentData
    where T4 : unmanaged, IComponentData
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponent(e, authoring.data1);
            AddComponent(e, authoring.data2);
            AddComponent(e, authoring.data3);
            AddComponent(e, authoring.data4);
        }
    }
}
