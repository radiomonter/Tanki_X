namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Mask
    {
        private readonly Texture2D texture;
        private List<Vector2> markedPixels;

        public Mask(Texture2D texture, float blackThreshold, bool whiteAsEmpty)
        {
            this.texture = texture;
            this.markedPixels = new List<Vector2>();
            int i = 0;
            while (i < texture.width)
            {
                int j = 0;
                while (true)
                {
                    if (j >= texture.height)
                    {
                        i++;
                        break;
                    }
                    if (this.IsMaskPixel(i, j, blackThreshold, whiteAsEmpty))
                    {
                        this.markedPixels.Add(new Vector2((float) i, (float) j));
                    }
                    j++;
                }
            }
        }

        private bool IsMaskPixel(int i, int j, float blackThreshold, bool whiteAsEmpty)
        {
            Color pixel = this.texture.GetPixel(i, j);
            return (!whiteAsEmpty ? (pixel.r > blackThreshold) : (pixel.r <= blackThreshold));
        }

        public int Width =>
            this.texture.width;

        public int Height =>
            this.texture.height;

        public List<Vector2> MarkedPixels =>
            this.markedPixels;
    }
}

