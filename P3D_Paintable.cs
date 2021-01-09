using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class P3D_Paintable : MonoBehaviour
{
    public static List<P3D_Paintable> AllPaintables = new List<P3D_Paintable>();
    [Tooltip("The submesh in the attached renderer we want to paint to")]
    public int SubMeshIndex;
    [Tooltip("The amount of seconds it takes for the mesh data to be updated (useful for animated meshes). -1 = No updates")]
    public float UpdateInterval = -1f;
    [Tooltip("The amount of seconds it takes for texture modifications to get applied")]
    public float ApplyInterval = 0.01f;
    [Tooltip("All the textures this paintable is associated with")]
    public List<P3D_PaintableTexture> Textures;
    private P3D_Tree tree;
    private Mesh bakedMesh;
    private float updateCooldown;
    private float applyCooldown;

    [ContextMenu("Add Texture")]
    public void AddTexture()
    {
        P3D_PaintableTexture item = new P3D_PaintableTexture();
        this.Textures ??= new List<P3D_PaintableTexture>();
        this.Textures.Add(item);
    }

    protected virtual void Awake()
    {
        if (this.Textures != null)
        {
            for (int i = this.Textures.Count - 1; i >= 0; i--)
            {
                P3D_PaintableTexture texture = this.Textures[i];
                if (texture != null)
                {
                    texture.Awake(base.gameObject);
                }
            }
        }
        this.UpdateTree();
    }

    private bool CheckTree()
    {
        if (this.tree == null)
        {
            return false;
        }
        if ((this.UpdateInterval >= 0f) && (this.updateCooldown < 0f))
        {
            this.updateCooldown = this.UpdateInterval;
            this.UpdateTree();
        }
        return true;
    }

    public P3D_Tree GetTree()
    {
        if ((this.tree != null) && ((this.UpdateInterval >= 0f) && (this.updateCooldown < 0f)))
        {
            this.updateCooldown = this.UpdateInterval;
            this.UpdateTree();
        }
        return this.tree;
    }

    protected virtual void OnDisable()
    {
        AllPaintables.Remove(this);
    }

    protected virtual void OnEnable()
    {
        AllPaintables.Add(this);
    }

    public void Paint(P3D_Brush brush, P3D_Result result, int groupMask = -1)
    {
        if ((result != null) && (this.Textures != null))
        {
            for (int i = this.Textures.Count - 1; i >= 0; i--)
            {
                P3D_PaintableTexture texture = this.Textures[i];
                if ((texture != null) && P3D_Helper.IndexInMask((int) texture.Group, groupMask))
                {
                    texture.Paint(brush, result.GetUV(texture.Coord));
                }
            }
        }
    }

    public void Paint(P3D_Brush brush, List<P3D_Result> results, int groupMask = -1)
    {
        if (results != null)
        {
            for (int i = 0; i < results.Count; i++)
            {
                this.Paint(brush, results[i], groupMask);
            }
        }
    }

    public void Paint(P3D_Brush brush, RaycastHit hit, int groupMask = -1)
    {
        if (this.Textures != null)
        {
            for (int i = this.Textures.Count - 1; i >= 0; i--)
            {
                P3D_PaintableTexture texture = this.Textures[i];
                if ((texture != null) && P3D_Helper.IndexInMask((int) texture.Group, groupMask))
                {
                    texture.Paint(brush, P3D_Helper.GetUV(hit, texture.Coord));
                }
            }
        }
    }

    public void Paint(P3D_Brush brush, Vector2 uv, int groupMask = -1)
    {
        if (this.Textures != null)
        {
            for (int i = this.Textures.Count - 1; i >= 0; i--)
            {
                P3D_PaintableTexture texture = this.Textures[i];
                if ((texture != null) && P3D_Helper.IndexInMask((int) texture.Group, groupMask))
                {
                    texture.Paint(brush, uv);
                }
            }
        }
    }

    public void PaintBetweenAll(P3D_Brush brush, Vector3 startPosition, Vector3 endPosition, int groupMask = -1)
    {
        if (this.CheckTree())
        {
            Vector3 startPoint = base.transform.InverseTransformPoint(startPosition);
            List<P3D_Result> results = this.tree.FindBetweenAll(startPoint, base.transform.InverseTransformPoint(endPosition));
            this.Paint(brush, results, groupMask);
        }
    }

    public void PaintBetweenNearest(P3D_Brush brush, Vector3 startPosition, Vector3 endPosition, int groupMask = -1)
    {
        if (this.CheckTree())
        {
            Vector3 startPoint = base.transform.InverseTransformPoint(startPosition);
            P3D_Result result = this.tree.FindBetweenNearest(startPoint, base.transform.InverseTransformPoint(endPosition));
            this.Paint(brush, result, groupMask);
        }
    }

    public void PaintNearest(P3D_Brush brush, Vector3 position, float maxDistance, int groupMask = -1)
    {
        if (this.CheckTree())
        {
            float uniformScale = P3D_Helper.GetUniformScale(base.transform);
            if (uniformScale != 0f)
            {
                Vector3 point = base.transform.InverseTransformPoint(position);
                P3D_Result result = this.tree.FindNearest(point, maxDistance / uniformScale);
                this.Paint(brush, result, groupMask);
            }
        }
    }

    public void PaintPerpendicularAll(P3D_Brush brush, Vector3 position, float maxDistance, int groupMask = -1)
    {
        if (this.CheckTree())
        {
            float uniformScale = P3D_Helper.GetUniformScale(base.transform);
            if (uniformScale != 0f)
            {
                Vector3 point = base.transform.InverseTransformPoint(position);
                List<P3D_Result> results = this.tree.FindPerpendicularAll(point, maxDistance / uniformScale);
                this.Paint(brush, results, groupMask);
            }
        }
    }

    public void PaintPerpendicularNearest(P3D_Brush brush, Vector3 position, float maxDistance, int groupMask = -1)
    {
        if (this.CheckTree())
        {
            float uniformScale = P3D_Helper.GetUniformScale(base.transform);
            if (uniformScale != 0f)
            {
                Vector3 point = base.transform.InverseTransformPoint(position);
                P3D_Result result = this.tree.FindPerpendicularNearest(point, maxDistance / uniformScale);
                this.Paint(brush, result, groupMask);
            }
        }
    }

    public static void ScenePaintBetweenAll(P3D_Brush brush, Vector3 startPosition, Vector3 endPosition, int layerMask = -1, int groupMask = -1)
    {
        for (int i = AllPaintables.Count - 1; i >= 0; i--)
        {
            P3D_Paintable paintable = AllPaintables[i];
            if (P3D_Helper.IndexInMask(paintable.gameObject.layer, layerMask))
            {
                paintable.PaintBetweenAll(brush, startPosition, endPosition, groupMask);
            }
        }
    }

    public static void ScenePaintBetweenNearest(P3D_Brush brush, Vector3 startPosition, Vector3 endPosition, int layerMask = -1, int groupMask = -1)
    {
        float num = Vector3.Distance(startPosition, endPosition);
        if (num != 0f)
        {
            P3D_Paintable paintable = null;
            P3D_Result result = null;
            for (int i = AllPaintables.Count - 1; i >= 0; i--)
            {
                P3D_Paintable paintable2 = AllPaintables[i];
                if (P3D_Helper.IndexInMask(paintable2.gameObject.layer, layerMask))
                {
                    P3D_Tree tree = paintable2.GetTree();
                    if (tree != null)
                    {
                        Transform transform = paintable2.transform;
                        Vector3 startPoint = transform.InverseTransformPoint(startPosition);
                        P3D_Result result2 = tree.FindBetweenNearest(startPoint, startPoint + ((transform.InverseTransformPoint(endPosition) - startPoint).normalized * num));
                        if (result2 != null)
                        {
                            paintable = paintable2;
                            result = result2;
                            num *= result2.Distance01;
                        }
                    }
                }
            }
            if ((paintable != null) && (result != null))
            {
                paintable.Paint(brush, result, groupMask);
            }
        }
    }

    public static void ScenePaintBetweenNearestRaycast(P3D_Brush brush, Vector3 startPosition, Vector3 endPosition, int layerMask = -1, int groupMask = -1)
    {
        float maxDistance = Vector3.Distance(startPosition, endPosition);
        if (maxDistance != 0f)
        {
            P3D_Paintable component = null;
            RaycastHit hitInfo = new RaycastHit();
            P3D_Result result = null;
            if (Physics.Raycast(startPosition, endPosition - startPosition, out hitInfo, maxDistance, layerMask))
            {
                component = hitInfo.collider.GetComponent<P3D_Paintable>();
                maxDistance = hitInfo.distance;
            }
            for (int i = AllPaintables.Count - 1; i >= 0; i--)
            {
                P3D_Paintable paintable2 = AllPaintables[i];
                if (P3D_Helper.IndexInMask(paintable2.gameObject.layer, layerMask))
                {
                    P3D_Tree tree = paintable2.GetTree();
                    if (tree != null)
                    {
                        Transform transform = paintable2.transform;
                        Vector3 startPoint = transform.InverseTransformPoint(startPosition);
                        P3D_Result result2 = tree.FindBetweenNearest(startPoint, startPoint + ((transform.InverseTransformPoint(endPosition) - startPoint).normalized * maxDistance));
                        if (result2 != null)
                        {
                            component = paintable2;
                            result = result2;
                            maxDistance *= result2.Distance01;
                        }
                    }
                }
            }
            if (component != null)
            {
                if (result != null)
                {
                    component.Paint(brush, result, groupMask);
                }
                else
                {
                    component.Paint(brush, hitInfo, groupMask);
                }
            }
        }
    }

    public static void ScenePaintNearest(P3D_Brush brush, Vector3 position, float maxDistance, int layerMask = -1, int groupMask = -1)
    {
        P3D_Paintable paintable = null;
        P3D_Result result = null;
        for (int i = AllPaintables.Count - 1; i >= 0; i--)
        {
            P3D_Paintable paintable2 = AllPaintables[i];
            if (P3D_Helper.IndexInMask(paintable2.gameObject.layer, layerMask))
            {
                P3D_Tree tree = paintable2.GetTree();
                if (tree != null)
                {
                    Transform transform = paintable2.transform;
                    if (P3D_Helper.GetUniformScale(transform) != 0f)
                    {
                        P3D_Result result2 = tree.FindNearest(transform.InverseTransformPoint(position), maxDistance);
                        if (result2 != null)
                        {
                            paintable = paintable2;
                            result = result2;
                            maxDistance *= result2.Distance01;
                        }
                    }
                }
            }
        }
        if (paintable != null)
        {
            paintable.Paint(brush, result, groupMask);
        }
    }

    public static void ScenePaintPerpedicularAll(P3D_Brush brush, Vector3 position, float maxDistance, int layerMask = -1, int groupMask = -1)
    {
        for (int i = AllPaintables.Count - 1; i >= 0; i--)
        {
            P3D_Paintable paintable = AllPaintables[i];
            if (P3D_Helper.IndexInMask(paintable.gameObject.layer, layerMask))
            {
                paintable.PaintPerpendicularAll(brush, position, maxDistance, groupMask);
            }
        }
    }

    public static void ScenePaintPerpedicularNearest(P3D_Brush brush, Vector3 position, float maxDistance, int layerMask = -1, int groupMask = -1)
    {
        P3D_Paintable paintable = null;
        P3D_Result result = null;
        for (int i = AllPaintables.Count - 1; i >= 0; i--)
        {
            P3D_Paintable paintable2 = AllPaintables[i];
            if (P3D_Helper.IndexInMask(paintable2.gameObject.layer, layerMask))
            {
                P3D_Tree tree = paintable2.GetTree();
                if (tree != null)
                {
                    Transform transform = paintable2.transform;
                    if (P3D_Helper.GetUniformScale(transform) != 0f)
                    {
                        P3D_Result result2 = tree.FindPerpendicularNearest(transform.InverseTransformPoint(position), maxDistance);
                        if (result2 != null)
                        {
                            paintable = paintable2;
                            result = result2;
                            maxDistance *= result2.Distance01;
                        }
                    }
                }
            }
        }
        if (paintable != null)
        {
            paintable.Paint(brush, result, groupMask);
        }
    }

    protected virtual void Update()
    {
        this.applyCooldown -= Time.deltaTime;
        if ((this.applyCooldown <= 0f) && (this.Textures != null))
        {
            this.applyCooldown = this.ApplyInterval;
            for (int i = this.Textures.Count - 1; i >= 0; i--)
            {
                P3D_PaintableTexture texture = this.Textures[i];
                if ((texture != null) && (texture.Painter != null))
                {
                    texture.Painter.Apply();
                }
            }
        }
        this.updateCooldown -= Time.deltaTime;
    }

    [ContextMenu("Update Tree")]
    public void UpdateTree()
    {
        bool forceUpdate = false;
        Mesh newMesh = P3D_Helper.GetMesh(base.gameObject, ref this.bakedMesh);
        if (this.bakedMesh != null)
        {
            forceUpdate = true;
        }
        this.tree ??= new P3D_Tree();
        this.tree.SetMesh(newMesh, this.SubMeshIndex, forceUpdate);
    }

    public bool IsReady =>
        (this.tree != null) && this.tree.IsReady;
}

