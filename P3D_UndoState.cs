using System;
using UnityEngine;

[Serializable]
public class P3D_UndoState
{
    public Texture2D Texture;
    public int Width;
    public int Height;
    public Color32[] Pixels;

    public P3D_UndoState(Texture2D newTexture)
    {
        if (newTexture != null)
        {
            this.Texture = newTexture;
            this.Width = newTexture.width;
            this.Height = newTexture.height;
            this.Pixels = newTexture.GetPixels32();
        }
    }

    public void Perform()
    {
        if (this.Texture != null)
        {
            if ((this.Texture.width != this.Width) || (this.Texture.height != this.Height))
            {
                this.Texture.Resize(this.Width, this.Height);
            }
            this.Texture.SetPixels32(this.Pixels);
            this.Texture.Apply();
        }
    }
}

