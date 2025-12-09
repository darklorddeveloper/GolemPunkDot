using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[RequireComponent(typeof(MeshFilter))]
public class VertIdxToUv : MonoBehaviour
{
    public int channel = 1;
    public string bakePath = "Assets/BakedAnimationTex/baked";
    private string assetformat = ".asset";

    // Start is called before the first frame update
    void Start()
    {
        var mesh = GetComponent<MeshFilter>().mesh;
        var uv = new List<Vector2>();
        var count = mesh.vertexCount;
        for (var i = 0; i < count; i++)
            uv.Add(new Vector2(i + 0.5f, 0) / Mathf.NextPowerOfTwo(count));
        mesh.SetUVs(channel, uv);

#if UNITY_EDITOR
        string path = bakePath + assetformat;
        Mesh loaded = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        if(loaded != null)
        {
            AssetDatabase.DeleteAsset(path);
            
        }
        mesh.hideFlags = HideFlags.None;
        AssetDatabase.CreateAsset(mesh, bakePath + assetformat);
#endif
    }
}
