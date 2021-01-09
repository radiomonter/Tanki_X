using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class P3D_Helper
{
    public const string ComponentMenuPrefix = "Paint in 3D/P3D ";
    private static Material clearMaterial;

    public static Material AddMaterial(Renderer renderer, Shader shader, int materialIndex = -1)
    {
        if (renderer == null)
        {
            return null;
        }
        List<Material> list = new List<Material>(renderer.sharedMaterials);
        Material item = new Material(shader);
        if (materialIndex <= 0)
        {
            materialIndex = list.Count;
        }
        list.Insert(materialIndex, item);
        renderer.sharedMaterials = list.ToArray();
        return item;
    }

    public static Vector2 CalculatePixelFromCoord(Vector2 uv, Vector2 tiling, Vector2 offset, int width, int height)
    {
        uv.x = Mathf.Repeat((uv.x * tiling.x) + offset.x, 1f);
        uv.y = Mathf.Repeat((uv.y * tiling.y) + offset.y, 1f);
        uv.x = Mathf.Clamp(Mathf.RoundToInt(uv.x * width), 0, width - 1);
        uv.y = Mathf.Clamp(Mathf.RoundToInt(uv.y * height), 0, height - 1);
        return uv;
    }

    public static void ClearTexture(Texture2D texture2D, Color color, bool apply = true)
    {
        if (texture2D != null)
        {
            int y = texture2D.height - 1;
            while (true)
            {
                if (y < 0)
                {
                    if (apply)
                    {
                        texture2D.Apply();
                    }
                    break;
                }
                int x = texture2D.width - 1;
                while (true)
                {
                    if (x < 0)
                    {
                        y--;
                        break;
                    }
                    texture2D.SetPixel(x, y, color);
                    x--;
                }
            }
        }
    }

    public static T Clone<T>(T o, bool keepName = true) where T: Object
    {
        if (o == null)
        {
            return null;
        }
        T local = Object.Instantiate<T>(o);
        if ((local != null) && keepName)
        {
            local.name = o.name;
        }
        return local;
    }

    public static Material CloneMaterial(GameObject gameObject, int materialIndex = 0)
    {
        if ((gameObject != null) && (materialIndex >= 0))
        {
            Renderer component = gameObject.GetComponent<Renderer>();
            if (component != null)
            {
                Material[] sharedMaterials = component.sharedMaterials;
                if (materialIndex < sharedMaterials.Length)
                {
                    Material material = Clone<Material>(sharedMaterials[materialIndex], true);
                    sharedMaterials[materialIndex] = material;
                    component.sharedMaterials = sharedMaterials;
                    return material;
                }
            }
        }
        return null;
    }

    public static float ClosestBarycentric(Vector2 point, Vector2 start, Vector2 end)
    {
        Vector2 vector = end - start;
        float sqrMagnitude = vector.sqrMagnitude;
        return ((sqrMagnitude <= 0f) ? 0.5f : Mathf.Clamp01(Vector2.Dot(point - start, vector / sqrMagnitude)));
    }

    public static float ClosestBarycentric(Vector3 point, P3D_Triangle triangle, out Vector3 weights)
    {
        Vector3 pointA = triangle.PointA;
        Vector3 pointB = triangle.PointB;
        Vector3 pointC = triangle.PointC;
        Quaternion quaternion = Quaternion.Inverse(Quaternion.LookRotation(-Vector3.Cross(pointA - pointB, pointA - pointC)));
        Vector3 a = (Vector3) (quaternion * pointA);
        Vector3 b = (Vector3) (quaternion * pointB);
        Vector3 vector6 = (Vector3) (quaternion * pointC);
        Vector3 p = (Vector3) (quaternion * point);
        if (PointLeftOfLine(a, b, p))
        {
            float y = ClosestBarycentric(p, a, b);
            weights = new Vector3(1f - y, y, 0f);
        }
        else if (PointLeftOfLine(b, vector6, p))
        {
            float z = ClosestBarycentric(p, b, vector6);
            weights = new Vector3(0f, 1f - z, z);
        }
        else if (PointLeftOfLine(vector6, a, p))
        {
            float x = ClosestBarycentric(p, vector6, a);
            weights = new Vector3(x, 0f, 1f - x);
        }
        else
        {
            Vector3 lhs = b - a;
            Vector3 rhs = vector6 - a;
            Vector3 vector10 = p - a;
            float num4 = Vector2.Dot(lhs, lhs);
            float num5 = Vector2.Dot(lhs, rhs);
            float num6 = Vector2.Dot(rhs, rhs);
            float num7 = Vector2.Dot(vector10, lhs);
            float num8 = Vector2.Dot(vector10, rhs);
            float num9 = Reciprocal((num4 * num6) - (num5 * num5));
            weights.y = ((num6 * num7) - (num5 * num8)) * num9;
            weights.z = ((num4 * num8) - (num5 * num7)) * num9;
            weights.x = (1f - weights.y) - weights.z;
        }
        Vector3 vector11 = (Vector3) (((weights.x * pointA) + (weights.y * pointB)) + (weights.z * pointC));
        return (point - vector11).sqrMagnitude;
    }

    public static bool ClosestBarycentric(Vector3 point, P3D_Triangle triangle, ref Vector3 weights, ref float distanceSqr)
    {
        Vector3 pointA = triangle.PointA;
        Vector3 pointB = triangle.PointB;
        Vector3 pointC = triangle.PointC;
        Quaternion quaternion = Quaternion.Inverse(Quaternion.LookRotation(-Vector3.Cross(pointA - pointB, pointA - pointC)));
        Vector3 a = (Vector3) (quaternion * pointA);
        Vector3 b = (Vector3) (quaternion * pointB);
        Vector3 vector6 = (Vector3) (quaternion * pointC);
        Vector3 p = (Vector3) (quaternion * point);
        if (!PointRightOfLine(a, b, p) || (!PointRightOfLine(b, vector6, p) || !PointRightOfLine(vector6, a, p)))
        {
            return false;
        }
        Vector3 lhs = b - a;
        Vector3 rhs = vector6 - a;
        Vector3 vector10 = p - a;
        float num = Vector2.Dot(lhs, lhs);
        float num2 = Vector2.Dot(lhs, rhs);
        float num3 = Vector2.Dot(rhs, rhs);
        float num4 = Vector2.Dot(vector10, lhs);
        float num5 = Vector2.Dot(vector10, rhs);
        float num6 = Reciprocal((num * num3) - (num2 * num2));
        weights.y = ((num3 * num4) - (num2 * num5)) * num6;
        weights.z = ((num * num5) - (num2 * num4)) * num6;
        weights.x = (1f - weights.y) - weights.z;
        Vector3 vector11 = (Vector3) (((weights.x * pointA) + (weights.y * pointB)) + (weights.z * pointC));
        distanceSqr = (point - vector11).sqrMagnitude;
        return true;
    }

    public static P3D_Matrix CreateMatrix(Vector2 position, Vector2 size, float angle)
    {
        P3D_Matrix matrix3 = P3D_Matrix.Translation(size.x * -0.5f, size.y * -0.5f);
        P3D_Matrix matrix4 = P3D_Matrix.Scaling(size.x, size.y);
        return (((P3D_Matrix.Translation(position.x, position.y) * P3D_Matrix.Rotation(angle)) * matrix3) * matrix4);
    }

    public static Texture2D CreateTexture(int width, int height, TextureFormat format, bool mipMaps) => 
        ((width <= 0) || (height <= 0)) ? null : new Texture2D(width, height, format, mipMaps);

    public static float Dampen(float current, float target, float dampening, float elapsed, float minStep = 0f)
    {
        float num = DampenFactor(dampening, elapsed);
        float maxDelta = (Mathf.Abs((float) (target - current)) * num) + (minStep * elapsed);
        return Mathf.MoveTowards(current, target, maxDelta);
    }

    public static Vector3 Dampen3(Vector3 current, Vector3 target, float dampening, float elapsed, float minStep = 0f)
    {
        float num = DampenFactor(dampening, elapsed);
        float maxDistanceDelta = ((target - current).magnitude * num) + (minStep * elapsed);
        return Vector3.MoveTowards(current, target, maxDistanceDelta);
    }

    public static float DampenFactor(float dampening, float elapsed) => 
        1f - Mathf.Pow(2.718282f, -dampening * elapsed);

    public static T Destroy<T>(T o) where T: Object
    {
        Object.Destroy(o);
        return null;
    }

    private static void DestroyMesh(ref Mesh mesh)
    {
        if (mesh != null)
        {
            Destroy<Mesh>(mesh);
            mesh = null;
        }
    }

    public static float Divide(float a, float b) => 
        Zero(b) ? 0f : (a / b);

    public static Material GetMaterial(GameObject gameObject, int materialIndex = 0)
    {
        if ((gameObject != null) && (materialIndex >= 0))
        {
            Renderer component = gameObject.GetComponent<Renderer>();
            if (component != null)
            {
                Material[] sharedMaterials = component.sharedMaterials;
                if (materialIndex < sharedMaterials.Length)
                {
                    return sharedMaterials[materialIndex];
                }
            }
        }
        return null;
    }

    public static Mesh GetMesh(GameObject gameObject, ref Mesh bakedMesh)
    {
        Mesh sharedMesh = null;
        if (gameObject != null)
        {
            MeshFilter component = gameObject.GetComponent<MeshFilter>();
            if (component != null)
            {
                sharedMesh = component.sharedMesh;
            }
            else
            {
                SkinnedMeshRenderer renderer = gameObject.GetComponent<SkinnedMeshRenderer>();
                if (renderer != null)
                {
                    sharedMesh = renderer.sharedMesh;
                    if (sharedMesh != null)
                    {
                        if (bakedMesh == null)
                        {
                            bakedMesh = new Mesh();
                            bakedMesh.name = "Baked Mesh";
                        }
                        renderer.BakeMesh(bakedMesh);
                        return bakedMesh;
                    }
                }
            }
        }
        DestroyMesh(ref bakedMesh);
        return sharedMesh;
    }

    public static TextureFormat GetTextureFormat(P3D_Format format) => 
        (format == P3D_Format.TruecolorRGBA) ? TextureFormat.RGBA32 : ((format == P3D_Format.TruecolorRGB) ? TextureFormat.RGB24 : ((format == P3D_Format.TruecolorA) ? TextureFormat.Alpha8 : ((TextureFormat) 0)));

    public static float GetUniformScale(Transform transform)
    {
        Vector3 lossyScale = transform.lossyScale;
        return (((lossyScale.x + lossyScale.y) + lossyScale.z) / 3f);
    }

    public static Vector2 GetUV(RaycastHit hit, P3D_CoordType coord)
    {
        if (coord == P3D_CoordType.UV1)
        {
            return hit.textureCoord;
        }
        if (coord == P3D_CoordType.UV2)
        {
            return hit.textureCoord2;
        }
        return new Vector2();
    }

    public static bool IndexInMask(int index, LayerMask mask)
    {
        mask &= 1 << (index & 0x1f);
        return (mask != 0);
    }

    public static bool IntersectBarycentric(Vector3 start, Vector3 end, P3D_Triangle triangle, out Vector3 weights, out float distance01)
    {
        Vector3 vector = new Vector3();
        weights = vector;
        distance01 = 0f;
        Vector3 lhs = triangle.Edge1;
        Vector3 rhs = triangle.Edge2;
        Vector3 vector4 = end - start;
        Vector3 vector5 = Vector3.Cross(vector4, rhs);
        float f = Vector3.Dot(lhs, vector5);
        if (Mathf.Abs(f) < float.Epsilon)
        {
            return false;
        }
        float num2 = 1f / f;
        Vector3 vector6 = start - triangle.PointA;
        weights.x = Vector3.Dot(vector6, vector5) * num2;
        if ((weights.x < -1.401298E-45f) || (weights.x > 1f))
        {
            return false;
        }
        Vector3 vector7 = Vector3.Cross(vector6, lhs);
        weights.y = Vector3.Dot(vector4, vector7) * num2;
        float num3 = weights.x + weights.y;
        if ((weights.y < -1.401298E-45f) || (num3 > 1f))
        {
            return false;
        }
        weights = new Vector3(1f - num3, weights.x, weights.y);
        distance01 = Vector3.Dot(rhs, vector7) * num2;
        return ((distance01 >= 0f) && (distance01 <= 1f));
    }

    public static bool IsWritableFormat(TextureFormat format)
    {
        switch (format)
        {
            case TextureFormat.Alpha8:
                return true;

            case TextureFormat.RGB24:
                return true;

            case TextureFormat.RGBA32:
                return true;

            case TextureFormat.ARGB32:
                return true;
        }
        return (format == TextureFormat.BGRA32);
    }

    public static bool PointLeftOfLine(Vector2 a, Vector2 b, Vector2 p) => 
        (((b.x - a.x) * (p.y - a.y)) - ((p.x - a.x) * (b.y - a.y))) >= 0f;

    public static bool PointRightOfLine(Vector2 a, Vector2 b, Vector2 p) => 
        (((b.x - a.x) * (p.y - a.y)) - ((p.x - a.x) * (b.y - a.y))) == 0f;

    public static float Reciprocal(float a) => 
        Zero(a) ? 0f : (1f / a);

    public static unsafe Rect SplitHorizontal(ref Rect rect, int separation)
    {
        Rect rect2 = rect;
        Rect* rectPtr1 = &rect2;
        rectPtr1.xMax -= (rect.width / 2f) + separation;
        Rect rect3 = rect;
        Rect* rectPtr2 = &rect3;
        rectPtr2.xMin += (rect.width / 2f) + separation;
        rect = rect2;
        return rect3;
    }

    public static unsafe Rect SplitVertical(ref Rect rect, int separation)
    {
        Rect rect2 = rect;
        Rect* rectPtr1 = &rect2;
        rectPtr1.yMax -= (rect.height / 2f) + separation;
        Rect rect3 = rect;
        Rect* rectPtr2 = &rect3;
        rectPtr2.yMin += (rect.height / 2f) + separation;
        rect = rect2;
        return rect3;
    }

    public static bool Zero(float v) => 
        v == 0f;

    public static Material ClearMaterial
    {
        get
        {
            if (clearMaterial == null)
            {
                clearMaterial = new Material(Shader.Find("Transparent/Diffuse"));
                clearMaterial.color = Color.clear;
            }
            return clearMaterial;
        }
    }
}

