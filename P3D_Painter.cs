using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public class P3D_Painter
{
    public bool Dirty;
    public Texture2D Canvas;
    public Vector2 Tiling = Vector2.one;
    public Vector2 Offset;

    public void Apply()
    {
        if ((this.Canvas != null) && this.Dirty)
        {
            this.Dirty = false;
            this.Canvas.Apply();
        }
    }

    public bool Paint(P3D_Brush brush, P3D_Matrix matrix)
    {
        if ((this.Canvas == null) || (brush == null))
        {
            return false;
        }
        brush.Paint(this.Canvas, matrix);
        this.Dirty = true;
        return true;
    }

    public bool Paint(P3D_Brush brush, Vector2 uv)
    {
        if (this.Canvas == null)
        {
            return false;
        }
        Vector2 vector = P3D_Helper.CalculatePixelFromCoord(uv, this.Tiling, this.Offset, this.Canvas.width, this.Canvas.height);
        return this.Paint(brush, vector.x, vector.y);
    }

    public bool Paint(P3D_Brush brush, P3D_Result result, P3D_CoordType coord = 0) => 
        (result != null) && this.Paint(brush, result.GetUV(coord));

    public bool Paint(P3D_Brush brush, List<P3D_Result> results, P3D_CoordType coord = 0)
    {
        bool flag = false;
        if (results != null)
        {
            for (int i = 0; i < results.Count; i++)
            {
                flag |= this.Paint(brush, results[i], coord);
            }
        }
        return flag;
    }

    public bool Paint(P3D_Brush brush, float x, float y)
    {
        if (brush == null)
        {
            return false;
        }
        Vector2 vector = new Vector2(x, y);
        P3D_Matrix matrix = P3D_Helper.CreateMatrix(vector + brush.Offset, brush.Size, brush.Angle);
        return this.Paint(brush, matrix);
    }

    public void SetCanvas(Texture newTexture)
    {
        this.SetCanvas(newTexture, Vector2.one, Vector2.zero);
    }

    public void SetCanvas(GameObject gameObject, string textureName = "_MainTex", int newMaterialIndex = 0)
    {
        Material material = P3D_Helper.GetMaterial(gameObject, newMaterialIndex);
        if (material != null)
        {
            this.SetCanvas(material.GetTexture(textureName), material.GetTextureScale(textureName), material.GetTextureOffset(textureName));
        }
        else
        {
            this.SetCanvas(null, Vector2.zero, Vector2.zero);
        }
    }

    public void SetCanvas(Texture newTexture, Vector2 newTiling, Vector2 newOffset)
    {
        Texture2D textured = newTexture as Texture2D;
        if ((textured == null) || ((newTiling.x == 0f) || (newTiling.y == 0f)))
        {
            this.Canvas = null;
        }
        else
        {
            if (!P3D_Helper.IsWritableFormat(textured.format))
            {
                throw new Exception("Trying to paint a non-writable texture");
            }
            this.Canvas = textured;
            this.Tiling = newTiling;
            this.Offset = newOffset;
        }
    }

    public bool IsReady =>
        this.Canvas != null;
}

