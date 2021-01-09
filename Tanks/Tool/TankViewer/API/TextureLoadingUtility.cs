namespace Tanks.Tool.TankViewer.API
{
    using System;
    using UnityEngine;

    public class TextureLoadingUtility
    {
        public static Texture2D CreateNormalMap(Texture2D source)
        {
            Texture2D textured = new Texture2D(source.width, source.height, TextureFormat.ARGB32, true, true);
            Color color = new Color();
            int x = 0;
            while (x < source.width)
            {
                int y = 0;
                while (true)
                {
                    if (y >= source.height)
                    {
                        x++;
                        break;
                    }
                    Color pixel = source.GetPixel(x, y);
                    float g = pixel.g;
                    color.r = g;
                    color.g = g;
                    color.b = g;
                    Color color3 = source.GetPixel(x, y);
                    color.a = color3.r;
                    textured.SetPixel(x, y, color);
                    y++;
                }
            }
            textured.Apply();
            return textured;
        }

        public static Texture2D CreateTextureWithGamma(Texture2D texture)
        {
            Texture2D textured = new Texture2D(texture.width, texture.height, texture.format, texture.mipmapCount > 1, false);
            textured.LoadRawTextureData(texture.GetRawTextureData());
            textured.Apply();
            return textured;
        }
    }
}

