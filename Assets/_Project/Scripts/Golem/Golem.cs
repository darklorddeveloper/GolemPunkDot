using System.Collections.Generic;
using UnityEngine;

namespace DarkLordGame
{
    public class Golem : MonoBehaviour
    {
        public Transform skinnedRoot;
        public SkinnedMeshRenderer originalSkinnedMesh;
        public List<GolemPart> attachedParts;

        public void Init()
        {
            for (int i = 0, length = attachedParts.Count; i < length; i++)
            {
                attachedParts[i] = SetupPart(attachedParts[i]);
            }
        }

        void OnDestroy()
        {

            for (int i = 0, length = attachedParts.Count; i < length; i++)
            {
                if (attachedParts[i] != null && attachedParts[i].isInstance)//isinstance is prevent destroying original source
                {
                    ScriptableObject.Destroy(attachedParts[i]);
                }
            }
        }


        private GolemPart SetupPart(GolemPart part)
        {
            var instance = ScriptableObject.Instantiate(part);
            instance.isInstance = true;
            var obj = new GameObject();
            obj.transform.SetParent(skinnedRoot);
            var skinned = obj.AddComponent<SkinnedMeshRenderer>();
            skinned.sharedMaterials = instance.materials;
            skinned.sharedMesh = instance.mesh;
            skinned.rootBone = originalSkinnedMesh.rootBone;
            skinned.bones = originalSkinnedMesh.bones;
            // skinned.sharedMesh.RecalculateBounds();
            instance.runtimSkinnedObject = obj;
            return instance;
        }

        public bool ChangePart(GolemPart golemPart)
        {
            for (int i = 0, length = attachedParts.Count; i < length; i++)
            {
                if (attachedParts[i].partType == golemPart.partType)
                {
                    if (attachedParts[i].runtimSkinnedObject != null)
                    {
                        GameObject.Destroy(attachedParts[i].runtimSkinnedObject);
                    }
                    if(attachedParts[i].isInstance)
                    {
                        ScriptableObject.Destroy(attachedParts[i]);
                    }
                    attachedParts[i] = SetupPart(golemPart);
                    return true;
                }
            }
            return false;
        }

        public void AddPart(GolemPart part)
        {
            if (ChangePart(part))
            {
                return;
            }
            SetupPart(part);
            attachedParts.Add(part);
        }
    }
}
