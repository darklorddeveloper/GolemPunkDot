using UnityEngine;
using Unity.Entities;
namespace DarkLordGame
{
    public class EnableStructAuthorizer<T1> : MonoBehaviour where T1 : unmanaged, IComponentData, IEnableableComponent
    {
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;
        [Header("Whether the component starts enabled on this entity.")]
        public bool enabledByDefault = true;
    }

    [DisableAutoCreation]
    public abstract class EnableStructBaker<T, T1> : Baker<T> where T : EnableStructAuthorizer<T1>
    where T1 : unmanaged, IComponentData, IEnableableComponent
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponent(e, authoring.data1);
            SetComponentEnabled<T1>(e, authoring.enabledByDefault);
        }
    }
    public class EnableStructAuthorizer<T1, T2> : MonoBehaviour
    where T1 : unmanaged, IComponentData, IEnableableComponent
    where T2 : unmanaged, IComponentData, IEnableableComponent
    {
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;

        public T2 data2;
        [Header("Whether the component starts enabled on this entity.")]
        public bool enabledByDefault1 = true;
        public bool enabledByDefault2 = true;
    }

    public abstract class EnableStructBaker<T, T1, T2> : Baker<T> where T : EnableStructAuthorizer<T1, T2>
    where T1 : unmanaged, IComponentData, IEnableableComponent
    where T2 : unmanaged, IComponentData, IEnableableComponent
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponent(e, authoring.data1);
            AddComponent(e, authoring.data2);
            SetComponentEnabled<T1>(e, authoring.enabledByDefault1);
            SetComponentEnabled<T2>(e, authoring.enabledByDefault2);

        }
    }

    public class EnableStructAuthorizer<T1, T2, T3> : MonoBehaviour
    where T1 : unmanaged, IComponentData, IEnableableComponent
    where T2 : unmanaged, IComponentData, IEnableableComponent
    where T3 : unmanaged, IComponentData, IEnableableComponent
    {
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;

        public T2 data2;
        public T3 data3;
        [Header("Whether the component starts enabled on this entity.")]
        public bool enabledByDefault1 = true;
        public bool enabledByDefault2 = true;
        public bool enabledByDefault3 = true;
    }


    public abstract class EnableStructBaker<T, T1, T2, T3> : Baker<T> where T : EnableStructAuthorizer<T1, T2, T3>
    where T1 : unmanaged, IComponentData, IEnableableComponent
    where T2 : unmanaged, IComponentData, IEnableableComponent
    where T3 : unmanaged, IComponentData, IEnableableComponent
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponent(e, authoring.data1);
            AddComponent(e, authoring.data2);
            AddComponent(e, authoring.data3);
            SetComponentEnabled<T1>(e, authoring.enabledByDefault1);
            SetComponentEnabled<T2>(e, authoring.enabledByDefault2);
            SetComponentEnabled<T3>(e, authoring.enabledByDefault3);
        }
    }

    public class EnableStructAuthorizer<T1, T2, T3, T4> : MonoBehaviour
    where T1 : unmanaged, IComponentData, IEnableableComponent
    where T2 : unmanaged, IComponentData, IEnableableComponent
    where T3 : unmanaged, IComponentData, IEnableableComponent
    where T4 : unmanaged, IComponentData, IEnableableComponent
    {
        public TransformUsageFlags flags = TransformUsageFlags.None;
        public T1 data1;

        public T2 data2;
        public T3 data3;
        public T4 data4;

        [Header("Whether the component starts enabled on this entity.")]
        public bool enabledByDefault1 = true;
        public bool enabledByDefault2 = true;
        public bool enabledByDefault3 = true;
        public bool enabledByDefault4 = true;
    }


    public abstract class EnableStructBaker<T, T1, T2, T3, T4> : Baker<T> where T : EnableStructAuthorizer<T1, T2, T3, T4>
    where T1 : unmanaged, IComponentData, IEnableableComponent
    where T2 : unmanaged, IComponentData, IEnableableComponent
    where T3 : unmanaged, IComponentData, IEnableableComponent
    where T4 : unmanaged, IComponentData, IEnableableComponent
    {
        public override void Bake(T authoring)
        {
            var e = GetEntity(authoring.flags);
            AddComponent(e, authoring.data1);
            AddComponent(e, authoring.data2);
            AddComponent(e, authoring.data3);
            AddComponent(e, authoring.data4);
            SetComponentEnabled<T1>(e, authoring.enabledByDefault1);
            SetComponentEnabled<T2>(e, authoring.enabledByDefault2);
            SetComponentEnabled<T3>(e, authoring.enabledByDefault3);
            SetComponentEnabled<T4>(e, authoring.enabledByDefault4);
        }
    }
}
