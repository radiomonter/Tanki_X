using System;
using UnityEngine;

[Serializable]
public class P3D_PaintableTexture
{
    [Tooltip("If your paintable has more than one texture then you can specify a group to select just one")]
    public P3D_Group Group;
    [Tooltip("The material index we want to paint")]
    public int MaterialIndex;
    [Tooltip("The texture we want to paint")]
    public string TextureName = "_MainTex";
    [Tooltip("The UV set used when painting this texture")]
    public P3D_CoordType Coord;
    [Tooltip("Should the material and texture get duplicated on awake? (useful for prefab clones)")]
    public bool DuplicateOnAwake;
    [Tooltip("Should the texture get created on awake? (useful for saving scene file size)")]
    public bool CreateOnAwake;
    [Tooltip("The width of the created texture")]
    public int CreateWidth = 0x200;
    [Tooltip("The height of the created texture")]
    public int CreateHeight = 0x200;
    [Tooltip("The pixel format of the created texture")]
    public P3D_Format CreateFormat;
    [Tooltip("The color of the created texture")]
    public Color CreateColor = Color.white;
    [Tooltip("Should the created etxture have mip maps?")]
    public bool CreateMipMaps = true;
    [Tooltip("Some shaders (e.g. Standard Shader) require you to enable keywords when adding new textures, you can specify that keyword here")]
    public string CreateKeyword;
    [SerializeField]
    private P3D_Painter painter;

    public void Awake(GameObject gameObject)
    {
        if (this.DuplicateOnAwake)
        {
            Material material = P3D_Helper.CloneMaterial(gameObject, this.MaterialIndex);
            if (material != null)
            {
                Texture o = material.GetTexture(this.TextureName);
                if (o != null)
                {
                    o = P3D_Helper.Clone<Texture>(o, true);
                    material.SetTexture(this.TextureName, o);
                }
            }
        }
        if (this.CreateOnAwake && ((this.CreateWidth > 0) && (this.CreateHeight > 0)))
        {
            Material material = P3D_Helper.GetMaterial(gameObject, this.MaterialIndex);
            if (material != null)
            {
                TextureFormat textureFormat = P3D_Helper.GetTextureFormat(this.CreateFormat);
                Texture2D textured = P3D_Helper.CreateTexture(this.CreateWidth, this.CreateHeight, textureFormat, this.CreateMipMaps);
                if (material.GetTexture(this.TextureName) != null)
                {
                    Debug.LogWarning("There is already a texture in this texture slot, maybe set it to null to save memory?", gameObject);
                }
                Texture texture2 = textured;
                P3D_Helper.ClearTexture(textured, this.CreateColor, true);
                material.SetTexture(this.TextureName, texture2);
                if (!string.IsNullOrEmpty(this.CreateKeyword))
                {
                    material.EnableKeyword(this.CreateKeyword);
                }
            }
        }
        this.UpdateTexture(gameObject);
    }

    public void Paint(P3D_Brush brush, Vector2 uv)
    {
        if (this.painter != null)
        {
            this.painter.Paint(brush, uv);
        }
    }

    public void UpdateTexture(GameObject gameObject)
    {
        this.painter ??= new P3D_Painter();
        this.painter.SetCanvas(gameObject, this.TextureName, this.MaterialIndex);
    }

    public P3D_Painter Painter =>
        this.painter;
}

