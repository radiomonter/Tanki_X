using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class P3D_BrushPreview : MonoBehaviour
{
    private static List<P3D_BrushPreview> AllPreviews = new List<P3D_BrushPreview>();
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Material material;
    private int age;
    private Material[] materials = new Material[1];

    public static void Mark()
    {
        for (int i = AllPreviews.Count - 1; i >= 0; i--)
        {
            P3D_BrushPreview preview = AllPreviews[i];
            if (preview != null)
            {
                preview.age = 5;
            }
        }
    }

    protected virtual void OnDestroy()
    {
        P3D_Helper.Destroy<Material>(this.material);
    }

    protected virtual void OnDisable()
    {
        AllPreviews.Remove(this);
    }

    protected virtual void OnEnable()
    {
        AllPreviews.Add(this);
    }

    public static void Show(Mesh mesh, int submeshIndex, Transform transform, float opacity, P3D_Matrix paintMatrix, Vector2 canvasResolution, Texture2D shape, Vector2 tiling, Vector2 offset)
    {
        for (int i = AllPreviews.Count - 1; i >= 0; i--)
        {
            P3D_BrushPreview preview = AllPreviews[i];
            if ((preview != null) && (preview.age > 0))
            {
                preview.UpdateShow(mesh, submeshIndex, transform, opacity, paintMatrix, canvasResolution, shape, tiling, offset);
                return;
            }
        }
        P3D_BrushPreview preview2 = new GameObject("P3D_BrushPreview") { hideFlags = HideFlags.HideAndDontSave }.AddComponent<P3D_BrushPreview>();
        preview2.hideFlags = HideFlags.HideAndDontSave;
        preview2.UpdateShow(mesh, submeshIndex, transform, opacity, paintMatrix, canvasResolution, shape, tiling, offset);
    }

    public static void Sweep()
    {
        for (int i = AllPreviews.Count - 1; i >= 0; i--)
        {
            P3D_BrushPreview preview = AllPreviews[i];
            if ((preview != null) && (preview.age > 1))
            {
                AllPreviews.RemoveAt(i);
                P3D_Helper.Destroy<GameObject>(preview.gameObject);
            }
        }
    }

    protected virtual void Update()
    {
        if (this.age >= 2)
        {
            P3D_Helper.Destroy<GameObject>(base.gameObject);
        }
        else
        {
            this.age++;
        }
    }

    private void UpdateShow(Mesh mesh, int submeshIndex, Transform target, float opacity, P3D_Matrix paintMatrix, Vector2 canvasResolution, Texture2D shape, Vector2 tiling, Vector2 offset)
    {
        if (target != null)
        {
            if (this.meshRenderer == null)
            {
                this.meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
            }
            if (this.meshFilter == null)
            {
                this.meshFilter = base.gameObject.AddComponent<MeshFilter>();
            }
            if (this.material == null)
            {
                this.material = new Material(Shader.Find("Hidden/P3D_BrushPreview"));
            }
            base.transform.position = target.position;
            base.transform.rotation = target.rotation;
            base.transform.localScale = target.lossyScale;
            this.material.hideFlags = HideFlags.HideAndDontSave;
            this.material.SetMatrix("_WorldMatrix", target.localToWorldMatrix);
            this.material.SetMatrix("_PaintMatrix", paintMatrix.Matrix4x4);
            this.material.SetVector("_CanvasResolution", canvasResolution);
            this.material.SetVector("_Tiling", tiling);
            this.material.SetVector("_Offset", offset);
            this.material.SetColor("_Color", new Color(1f, 1f, 1f, opacity));
            this.material.SetTexture("_Shape", shape);
            if (this.materials.Length != (submeshIndex + 1))
            {
                this.materials = new Material[submeshIndex + 1];
            }
            int index = 0;
            while (true)
            {
                if (index >= submeshIndex)
                {
                    this.materials[submeshIndex] = this.material;
                    this.meshRenderer.sharedMaterials = this.materials;
                    this.meshFilter.sharedMesh = mesh;
                    this.age = 0;
                    break;
                }
                this.materials[index] = P3D_Helper.ClearMaterial;
                index++;
            }
        }
    }
}

