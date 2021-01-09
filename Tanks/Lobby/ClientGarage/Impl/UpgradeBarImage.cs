namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.Sprites;
    using UnityEngine.UI;

    public class UpgradeBarImage : MaskableGraphic
    {
        [SerializeField]
        private float spacing = 2f;
        [SerializeField]
        private int segmentsCount = 30;
        private int currentSegment;
        private int upgradedSegments;
        private float currentSegmentScale = 30f;
        private float currentSegmentProgress = 1f;
        [SerializeField]
        private Sprite sprite;
        [SerializeField]
        private PaletteColorField filledColor;
        [SerializeField]
        private PaletteColorField upgradeColor;
        [SerializeField]
        private PaletteColorField backgroundColor;
        [SerializeField]
        private PaletteColorField strokeColor;

        private void AddBar(VertexHelper vh, float fill, Vector4 pos, Rect rect, Vector4 border, Vector2 tileSize, Vector4 outerUVs, Vector4 innerUVs, Color color)
        {
            if (Mathf.Abs((float) (pos.x - rect.x)) < 2f)
            {
                this.AddLeftBar(vh, fill, pos, rect, border, tileSize, outerUVs, innerUVs, color);
            }
            else if (Mathf.Abs((float) (pos.z - rect.xMax)) < 2f)
            {
                this.AddRightBar(vh, fill, pos, rect, border, tileSize, outerUVs, innerUVs, color);
            }
            else
            {
                this.AddMidBar(vh, fill, pos, rect, border, tileSize, outerUVs, innerUVs, color);
            }
        }

        private void AddFill(VertexHelper vh, Vector4 pos, Vector4 border, float leftOffset, float rightOffset, float fill, Color color)
        {
            this.AddQuad(vh, new Vector4(pos.x + leftOffset, pos.y + border.y, (pos.z - rightOffset) - ((1f - fill) * (pos.z - pos.x)), pos.w - border.w), Vector4.zero, color);
        }

        private void AddLeftBar(VertexHelper vh, float fill, Vector4 pos, Rect rect, Vector4 border, Vector2 tileSize, Vector4 outerUVs, Vector4 innerUVs, Color color)
        {
            float num = pos.y + border.y;
            float b = pos.w - border.w;
            float y = innerUVs.y;
            float num4 = (innerUVs.w - innerUVs.y) / tileSize.y;
            while (num < b)
            {
                float num5 = num;
                float w = Mathf.Min(num5 + tileSize.y, b);
                float num7 = y;
                float num8 = Mathf.Min(innerUVs.w, num7 + ((w - num5) * num4));
                this.AddQuad(vh, new Vector4(pos.x, num5, pos.x + border.x, w), new Vector4(outerUVs.x, num7, innerUVs.x, num8), (Color) this.strokeColor);
                num = w;
            }
            this.AddTopAndBottomBorder(vh, pos, rect, border, tileSize, outerUVs, innerUVs, (Color) this.strokeColor);
            if (fill < 1f)
            {
                float num10;
                for (num = pos.y + border.y; num < b; num = num10)
                {
                    float num9 = num;
                    num10 = Mathf.Min(num9 + tileSize.y, b);
                    float num11 = y;
                    float w = Mathf.Min(innerUVs.w, num11 + ((num10 - num9) * num4));
                    this.AddQuad(vh, new Vector4(pos.z - border.z, num9, pos.z, num10), new Vector4(innerUVs.z, num11, outerUVs.z, w), (Color) this.strokeColor);
                }
            }
            this.AddFill(vh, pos, border, border.x, 0f, fill, color);
        }

        private void AddMidBar(VertexHelper vh, float fill, Vector4 pos, Rect rect, Vector4 border, Vector2 tileSize, Vector4 outerUVs, Vector4 innerUVs, Color color)
        {
            float num = pos.y + border.y;
            float b = pos.w - border.w;
            float y = innerUVs.y;
            float num4 = (innerUVs.w - innerUVs.y) / tileSize.y;
            if (fill < 1f)
            {
                float num6;
                for (num = pos.y + border.y; num < b; num = num6)
                {
                    float num5 = num;
                    num6 = Mathf.Min(num5 + tileSize.y, b);
                    float num7 = y;
                    float w = Mathf.Min(innerUVs.w, num7 + ((num6 - num5) * num4));
                    this.AddQuad(vh, new Vector4(pos.z - border.z, num5, pos.z, num6), new Vector4(innerUVs.z, num7, outerUVs.z, w), (Color) this.strokeColor);
                }
            }
            this.AddTopAndBottomBorder(vh, pos, rect, border, tileSize, outerUVs, innerUVs, (Color) this.strokeColor);
            this.AddFill(vh, pos, border, 0f, 0f, fill, color);
        }

        private void AddQuad(VertexHelper vh, Vector4 pos, Vector4 uv, Color color)
        {
            int currentVertCount = vh.currentVertCount;
            vh.AddVert((Vector3) new Vector2(pos.x, pos.y), color, new Vector2(uv.x, uv.y));
            vh.AddVert((Vector3) new Vector2(pos.x, pos.w), color, new Vector2(uv.x, uv.w));
            vh.AddVert((Vector3) new Vector2(pos.z, pos.w), color, new Vector2(uv.z, uv.w));
            vh.AddVert((Vector3) new Vector2(pos.z, pos.y), color, new Vector2(uv.z, uv.y));
            vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
            vh.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
        }

        private void AddRightBar(VertexHelper vh, float fill, Vector4 pos, Rect rect, Vector4 border, Vector2 tileSize, Vector4 outerUVs, Vector4 innerUVs, Color color)
        {
            float num = pos.y + border.y;
            float b = pos.w - border.w;
            float y = innerUVs.y;
            float num4 = (innerUVs.w - innerUVs.y) / tileSize.y;
            while (num < b)
            {
                float num5 = num;
                float w = Mathf.Min(num5 + tileSize.y, b);
                float num7 = y;
                float num8 = Mathf.Min(innerUVs.w, num7 + ((w - num5) * num4));
                this.AddQuad(vh, new Vector4(pos.z - border.z, num5, pos.z, w), new Vector4(innerUVs.z, num7, outerUVs.z, num8), (Color) this.strokeColor);
                num = w;
            }
            this.AddTopAndBottomBorder(vh, pos, rect, border, tileSize, outerUVs, innerUVs, (Color) this.strokeColor);
            this.AddFill(vh, pos, border, 0f, border.z, fill, color);
        }

        private void AddTopAndBottomBorder(VertexHelper vh, Vector4 pos, Rect rect, Vector4 border, Vector2 tileSize, Vector4 outerUVs, Vector4 innerUVs, Color color)
        {
            float x = pos.x;
            float z = pos.z;
            float num3 = (innerUVs.z - innerUVs.x) / tileSize.x;
            float num4 = ((pos.x - rect.xMin) * num3) % (tileSize.x * num3);
            while (x < z)
            {
                float num5 = x;
                float num6 = Mathf.Min(x + tileSize.x, z);
                float num7 = num4;
                float num8 = Mathf.Min(outerUVs.z, num7 + ((num6 - num5) * num3));
                this.AddQuad(vh, new Vector4(num5, pos.y, num6, pos.y + border.y), new Vector4(num7, outerUVs.y, num8, innerUVs.y), color);
                this.AddQuad(vh, new Vector4(num5, pos.w - border.w, num6, pos.w), new Vector4(num7, innerUVs.w, num8, outerUVs.w), color);
                x = num6;
            }
        }

        private void Border(VertexHelper vh)
        {
            Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
            float xMin = pixelAdjustedRect.xMin;
            float num2 = this.spacing / this.pixelsPerUnit;
            float num3 = (pixelAdjustedRect.width - (num2 * (this.segmentsCount - 1))) / ((this.segmentsCount - 1) + this.currentSegmentScale);
            Vector4 outerUV = DataUtility.GetOuterUV(this.sprite);
            Vector4 innerUV = DataUtility.GetInnerUV(this.sprite);
            Vector2 size = this.sprite.rect.size;
            Vector4 adjustedBorders = this.GetAdjustedBorders(this.sprite.border / this.pixelsPerUnit, pixelAdjustedRect);
            Vector2 tileSize = new Vector2(((size.x - adjustedBorders.x) - adjustedBorders.z) / this.pixelsPerUnit, ((size.y - adjustedBorders.y) - adjustedBorders.w) / this.pixelsPerUnit);
            for (int i = 0; xMin < pixelAdjustedRect.xMax; i++)
            {
                float num7 = (i != this.currentSegment) ? num3 : (num3 * this.currentSegmentScale);
                float x = Mathf.Min(xMin + num7, pixelAdjustedRect.xMax);
                float num9 = xMin;
                Vector2 bottomLeft = base.PixelAdjustPoint(new Vector2(num9, pixelAdjustedRect.yMin));
                Vector2 topRight = base.PixelAdjustPoint(new Vector2(x, pixelAdjustedRect.yMax));
                PaletteColorField field = (i >= this.upgradedSegments) ? ((i > this.currentSegment) ? this.backgroundColor : this.upgradeColor) : this.filledColor;
                this.AddBar(vh, (i != this.currentSegment) ? 1f : this.currentSegmentProgress, this.FromVector2(bottomLeft, topRight), pixelAdjustedRect, adjustedBorders, tileSize, outerUV, innerUV, (Color) field);
                xMin += num7 + num2;
            }
        }

        private Vector4 FromVector2(Vector2 bottomLeft, Vector2 topRight) => 
            new Vector4(bottomLeft.x, bottomLeft.y, topRight.x, topRight.y);

        private Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
        {
            for (int i = 0; i <= 1; i++)
            {
                float num2 = border[i] + border[i + 2];
                Vector2 size = rect.size;
                if ((size[i] < num2) && (num2 != 0.0))
                {
                    ref Vector4 vectorRef;
                    int num4;
                    int num5;
                    float num3 = rect.size[i] / num2;
                    (vectorRef = ref border)[num4 = i] = vectorRef[num4] * num3;
                    (vectorRef = ref border)[num5 = i + 2] = vectorRef[num5] * num3;
                }
            }
            return border;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if ((this.segmentsCount > 0) && (this.sprite != null))
            {
                this.Border(vh);
            }
        }

        protected override void UpdateMaterial()
        {
            base.UpdateMaterial();
            if (this.sprite == null)
            {
                base.canvasRenderer.SetAlphaTexture(null);
            }
            else
            {
                Texture2D associatedAlphaSplitTexture = this.sprite.associatedAlphaSplitTexture;
                if (associatedAlphaSplitTexture != null)
                {
                    base.canvasRenderer.SetAlphaTexture(associatedAlphaSplitTexture);
                }
            }
        }

        public void Validate()
        {
            this.UpdateGeometry();
        }

        public int SegmentsCount
        {
            get => 
                this.segmentsCount;
            set => 
                this.segmentsCount = value;
        }

        public int CurrentSegment
        {
            get => 
                this.currentSegment;
            set => 
                this.currentSegment = value;
        }

        public int UpgradedSegments
        {
            get => 
                this.upgradedSegments;
            set => 
                this.upgradedSegments = value;
        }

        public float CurrentSegmentProgress
        {
            get => 
                this.currentSegmentProgress;
            set => 
                this.currentSegmentProgress = value;
        }

        public float CurrentSegmentScale
        {
            get => 
                this.currentSegmentScale;
            set => 
                this.currentSegmentScale = value;
        }

        public override Texture mainTexture =>
            (this.sprite == null) ? base.mainTexture : this.sprite.texture;

        public float pixelsPerUnit
        {
            get
            {
                float pixelsPerUnit = 100f;
                if (this.sprite)
                {
                    pixelsPerUnit = this.sprite.pixelsPerUnit;
                }
                float referencePixelsPerUnit = 100f;
                if (base.canvas)
                {
                    referencePixelsPerUnit = base.canvas.referencePixelsPerUnit;
                }
                return (pixelsPerUnit / referencePixelsPerUnit);
            }
        }
    }
}

