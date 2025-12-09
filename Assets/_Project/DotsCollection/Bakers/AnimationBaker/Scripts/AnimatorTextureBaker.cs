using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class AnimatorTextureBaker : MonoBehaviour
{
    public ComputeShader infoTexGen;
    public Shader loopPlayShader;
    public Shader nonloopPlayShader;

    public bool useIDTOUV1 = true;
    public struct VertInfo
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector3 tangent;
    }

    // Use this for initialization
    IEnumerator Start()
    {
        var animator = GetComponent<Animator>();
        var clips = animator.runtimeAnimatorController.animationClips;

        var skin = GetComponentInChildren<SkinnedMeshRenderer>();
        var vCount = skin.sharedMesh.vertexCount;
        var texWidth = Mathf.NextPowerOfTwo(vCount);
        var mesh = new Mesh();
        var targetName = name + "_" + skin.name;

        foreach (var c in clips)
        {
            var frames = Mathf.NextPowerOfTwo((int)(c.length / 0.05f));
            var infoList = new List<VertInfo>();

            var pRt = new RenderTexture(texWidth, frames, 0, RenderTextureFormat.ARGBHalf);
            pRt.name = string.Format("{0}.{1}.posTex", targetName, c.name);
            var nRt = new RenderTexture(texWidth, frames, 0, RenderTextureFormat.ARGBHalf);
            nRt.name = string.Format("{0}.{1}.normTex", targetName, c.name);
            var tRt = new RenderTexture(texWidth, frames, 0, RenderTextureFormat.ARGBHalf);
            tRt.name = string.Format("{0}.{1}.tangentTex", targetName, c.name);
            foreach (var rt in new[] { pRt, nRt, tRt })
            {
                rt.enableRandomWrite = true;
                rt.Create();
                RenderTexture.active = rt;
                GL.Clear(true, true, Color.clear);
            }

            animator.Play(c.name);
            animator.speed = 0;
            yield return 0;
            for (var i = 0; i <= frames; i++)
            {
                animator.Play(c.name, 0, (float)i / (float)frames);
                yield return 0;
                skin.BakeMesh(mesh, true);

                infoList.AddRange(Enumerable.Range(0, vCount)
                    .Select(idx => new VertInfo()
                    {
                        position = mesh.vertices[idx],
                        normal = mesh.normals[idx],
                        tangent = mesh.tangents[idx]
                    })
                );


            }
            var buffer = new ComputeBuffer(infoList.Count, System.Runtime.InteropServices.Marshal.SizeOf(typeof(VertInfo)));
            buffer.SetData(infoList.ToArray());

            var kernel = infoTexGen.FindKernel("CSMain");
            uint x, y, z;
            infoTexGen.GetKernelThreadGroupSizes(kernel, out x, out y, out z);

            infoTexGen.SetInt("VertCount", vCount);
            infoTexGen.SetBuffer(kernel, "Info", buffer);
            infoTexGen.SetTexture(kernel, "OutPosition", pRt);
            infoTexGen.SetTexture(kernel, "OutNormal", nRt);
            infoTexGen.SetTexture(kernel, "OutTangent", tRt);
            infoTexGen.Dispatch(kernel, vCount / (int)x + 1, frames / (int)y + 1, 1);

            buffer.Release();


#if UNITY_EDITOR
            var folderName = "BakedAnimationTex";
            var folderPath = Path.Combine("Assets", folderName);
            if (!AssetDatabase.IsValidFolder(folderPath))
                AssetDatabase.CreateFolder("Assets", folderName);

            var subFolder = targetName;
            var subFolderPath = Path.Combine(folderPath, subFolder);
            if (!AssetDatabase.IsValidFolder(subFolderPath))
                AssetDatabase.CreateFolder(folderPath, subFolder);

            var posTex = RenderTextureToTexture2D.Convert(pRt);
            var normTex = RenderTextureToTexture2D.Convert(nRt);
            var tanTex = RenderTextureToTexture2D.Convert(tRt);
            Graphics.CopyTexture(pRt, posTex);
            Graphics.CopyTexture(nRt, normTex);
            Graphics.CopyTexture(tRt, tanTex);

            var mat = c.isLooping ? new Material(loopPlayShader) : new Material(nonloopPlayShader);
            if (skin.sharedMaterial.mainTexture != null)
            {
                mat.SetTexture("_MainTex", skin.sharedMaterial.mainTexture);
            }
            else
            {
                var texture = skin.sharedMaterial.GetTexture("_Texture2D");
                mat.SetTexture("_MainTex", texture);
            }
            mat.SetTexture("_PosTex", posTex);
            mat.SetTexture("_NmlTex", normTex);
            mat.SetFloat("_Length", c.length);
            if (c.isLooping == false)
            {
                mat.SetFloat("_LastFrameWhenNotLooped", 0.95f);
            }

            var go = new GameObject(targetName + "." + c.name);
            go.AddComponent<MeshRenderer>().sharedMaterial = mat;
            go.AddComponent<MeshFilter>().sharedMesh = skin.sharedMesh;

            AssetDatabase.CreateAsset(posTex, Path.Combine(subFolderPath, pRt.name + ".asset"));
            AssetDatabase.CreateAsset(normTex, Path.Combine(subFolderPath, nRt.name + ".asset"));
            AssetDatabase.CreateAsset(tanTex, Path.Combine(subFolderPath, tRt.name + ".asset"));

            AssetDatabase.CreateAsset(mat, Path.Combine(subFolderPath, string.Format("{0}.{1}.animTex.asset", name, c.name)));
            PrefabUtility.SaveAsPrefabAsset(go, Path.Combine(folderPath, go.name + ".prefab").Replace("\\", "/"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        if (useIDTOUV1)
        {
            mesh = skin.sharedMesh;
            mesh.hideFlags = HideFlags.None;
            var uv = new List<Vector2>();
            var count = mesh.vertexCount;
            for (var i = 0; i < count; i++)
                uv.Add(new Vector2(i + 0.5f, 0) / Mathf.NextPowerOfTwo(count));
            mesh.SetUVs(1, uv);

#if UNITY_EDITOR
            var folderName2 = "BakedAnimationTex";
            var folderPath2 = Path.Combine("Assets", folderName2);
            if (!AssetDatabase.IsValidFolder(folderPath2))
                AssetDatabase.CreateFolder("Assets", folderName2);

            var subFolder2 = name;
            var subFolderPath2 = Path.Combine(folderPath2, subFolder2);
            if (!AssetDatabase.IsValidFolder(subFolderPath2))
                AssetDatabase.CreateFolder(folderPath2, subFolder2);

            string path = Path.Combine(subFolderPath2, "_Mesh.asset");
            Mesh loaded = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (loaded != null)
            {
                AssetDatabase.DeleteAsset(path);

            }
            mesh.hideFlags = HideFlags.None;
            var existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (existing)
            {
                EditorUtility.CopySerialized(mesh, existing);
                EditorUtility.SetDirty(existing);
            }
            else
            {
                AssetDatabase.CreateAsset(mesh, path);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

    }
}
