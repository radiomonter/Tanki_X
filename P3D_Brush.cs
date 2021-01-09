using System;
using UnityEngine;

[Serializable]
public class P3D_Brush
{
    public static Action<Texture2D, P3D_Rect> OnPrePaint;
    public static Action<Texture2D, P3D_Rect> OnPostPaint;
    [Tooltip("The name of this brush (mainly used for saving/loading)")]
    public string Name = "Default";
    [Tooltip("The opacity of the brush (how solid it is)"), Range(0f, 1f)]
    public float Opacity = 1f;
    [Tooltip("The angle of the brush in radians"), Range(-3.141593f, 3.141593f)]
    public float Angle;
    [Tooltip("The amount of pixels the brush gets moved from the pain location")]
    public Vector2 Offset;
    [Tooltip("The size of the brush in pixels")]
    public Vector2 Size = new Vector2(10f, 10f);
    [Tooltip("The blend mode of the brush")]
    public P3D_BlendMode Blend;
    [Tooltip("The shape of the brush")]
    public Texture2D Shape;
    [Tooltip("The color of the brush")]
    public UnityEngine.Color Color = UnityEngine.Color.white;
    [Tooltip("The normal direction of the brush (used for NormalBlend)")]
    public Vector2 Direction;
    [Tooltip("The detail texture when painting")]
    public Texture2D Detail;
    [Tooltip("The scale of the detail texture, allowing you to tile it")]
    public Vector2 DetailScale = new Vector2(0.5f, 0.5f);
    private static Texture2D canvas;
    private static int canvasW;
    private static int canvasH;
    private static P3D_Rect rect;
    private static P3D_Matrix matrix;
    private static P3D_Matrix inverse;
    private static float opacity;
    private static UnityEngine.Color color;
    private static Vector2 direction;
    private static Texture2D shape;
    private static Texture2D detail;
    private static Vector2 detailScale;
    private static P3D_Brush tempInstance;

    private bool CalculateRect(ref P3D_Rect rect)
    {
        Vector2 vector = matrix.MultiplyPoint(0f, 0f);
        Vector2 vector2 = matrix.MultiplyPoint(1f, 0f);
        Vector2 vector3 = matrix.MultiplyPoint(0f, 1f);
        Vector2 vector4 = matrix.MultiplyPoint(1f, 1f);
        float f = Mathf.Min(Mathf.Min(vector.x, vector2.x), Mathf.Min(vector3.x, vector4.x));
        float num2 = Mathf.Max(Mathf.Max(vector.x, vector2.x), Mathf.Max(vector3.x, vector4.x));
        float num3 = Mathf.Min(Mathf.Min(vector.y, vector2.y), Mathf.Min(vector3.y, vector4.y));
        float num4 = Mathf.Max(Mathf.Max(vector.y, vector2.y), Mathf.Max(vector3.y, vector4.y));
        if ((f >= num2) || (num3 >= num4))
        {
            return false;
        }
        rect.XMin = Mathf.Clamp(Mathf.FloorToInt(f), 0, canvasW);
        rect.XMax = Mathf.Clamp(Mathf.CeilToInt(num2), 0, canvasW);
        rect.YMin = Mathf.Clamp(Mathf.FloorToInt(num3), 0, canvasH);
        rect.YMax = Mathf.Clamp(Mathf.CeilToInt(num4), 0, canvasH);
        return true;
    }

    public void CopyTo(P3D_Brush other)
    {
        if (other != null)
        {
            other.Name = this.Name;
            other.Opacity = this.Opacity;
            other.Angle = this.Angle;
            other.Offset = this.Offset;
            other.Size = this.Size;
            other.Blend = this.Blend;
            other.Color = this.Color;
            other.Direction = this.Direction;
            other.Shape = this.Shape;
            other.Detail = this.Detail;
            other.DetailScale = this.DetailScale;
        }
    }

    public P3D_Brush GetTempClone()
    {
        this.CopyTo(TempInstance);
        return tempInstance;
    }

    private static bool IsInsideShape(P3D_Matrix inverseMatrix, int x, int y, ref Vector2 shapeCoord)
    {
        shapeCoord = inverseMatrix.MultiplyPoint((float) x, (float) y);
        return ((shapeCoord.x >= 0f) && ((shapeCoord.x < 1f) && ((shapeCoord.y >= 0f) && (shapeCoord.y < 1f))));
    }

    public void Paint(Texture2D newCanvas, P3D_Matrix newMatrix)
    {
        canvas = newCanvas;
        canvasW = newCanvas.width;
        canvasH = newCanvas.height;
        matrix = newMatrix;
        if (this.CalculateRect(ref rect))
        {
            inverse = newMatrix.Inverse;
            opacity = this.Opacity;
            color = this.Color;
            direction = this.Direction;
            shape = this.Shape;
            detail = this.Detail;
            detailScale = this.DetailScale;
            if (OnPrePaint != null)
            {
                OnPrePaint(canvas, rect);
            }
            switch (this.Blend)
            {
                case P3D_BlendMode.AlphaBlend:
                    AlphaBlend.Paint();
                    break;

                case P3D_BlendMode.AlphaBlendRgb:
                    AlphaBlendRGB.Paint();
                    break;

                case P3D_BlendMode.AlphaErase:
                    AlphaErase.Paint();
                    break;

                case P3D_BlendMode.AdditiveBlend:
                    AdditiveBlend.Paint();
                    break;

                case P3D_BlendMode.SubtractiveBlend:
                    SubtractiveBlend.Paint();
                    break;

                case P3D_BlendMode.NormalBlend:
                    NormalBlend.Paint();
                    break;

                case P3D_BlendMode.Replace:
                    Replace.Paint();
                    break;

                default:
                    break;
            }
            if (OnPostPaint != null)
            {
                OnPostPaint(canvas, rect);
            }
        }
    }

    private static UnityEngine.Color SampleRepeat(Texture2D texture, float u, float v) => 
        texture.GetPixelBilinear(u % 1f, v % 1f);

    public static P3D_Brush TempInstance
    {
        get
        {
            tempInstance ??= new P3D_Brush();
            return tempInstance;
        }
    }

    private static class AdditiveBlend
    {
        private static unsafe Color Blend(Color old, Color add)
        {
            Color* colorPtr1 = &old;
            colorPtr1->r += add.r;
            Color* colorPtr2 = &old;
            colorPtr2->g += add.g;
            Color* colorPtr3 = &old;
            colorPtr3->b += add.b;
            Color* colorPtr4 = &old;
            colorPtr4->a += add.a;
            return old;
        }

        public static unsafe void Paint()
        {
            Vector2 shapeCoord = new Vector2();
            float num = P3D_Helper.Reciprocal(P3D_Brush.canvasW * P3D_Brush.detailScale.x);
            float num2 = P3D_Helper.Reciprocal(P3D_Brush.canvasH * P3D_Brush.detailScale.y);
            Color* colorPtr1 = &P3D_Brush.color;
            colorPtr1->a *= P3D_Brush.opacity;
            int xMin = P3D_Brush.rect.XMin;
            while (xMin < P3D_Brush.rect.XMax)
            {
                int yMin = P3D_Brush.rect.YMin;
                while (true)
                {
                    if (yMin >= P3D_Brush.rect.YMax)
                    {
                        xMin++;
                        break;
                    }
                    if (P3D_Brush.IsInsideShape(P3D_Brush.inverse, xMin, yMin, ref shapeCoord))
                    {
                        Color pixel = P3D_Brush.canvas.GetPixel(xMin, yMin);
                        Color color = P3D_Brush.color;
                        if (P3D_Brush.shape != null)
                        {
                            color *= P3D_Brush.shape.GetPixelBilinear(shapeCoord.x, shapeCoord.y);
                        }
                        if (P3D_Brush.detail != null)
                        {
                            color *= P3D_Brush.SampleRepeat(P3D_Brush.detail, num * xMin, num2 * yMin);
                        }
                        P3D_Brush.canvas.SetPixel(xMin, yMin, Blend(pixel, color));
                    }
                    yMin++;
                }
            }
        }
    }

    private static class AlphaBlend
    {
        private static Color Blend(Color old, Color add)
        {
            if (add.a > 0f)
            {
                float a = add.a;
                float num2 = 1f - a;
                float num3 = old.a;
                float num4 = a + (num3 * num2);
                old.r = ((add.r * a) + ((old.r * num3) * num2)) / num4;
                old.g = ((add.g * a) + ((old.g * num3) * num2)) / num4;
                old.b = ((add.b * a) + ((old.b * num3) * num2)) / num4;
                old.a = num4;
            }
            return old;
        }

        public static unsafe void Paint()
        {
            Vector2 shapeCoord = new Vector2();
            float num = P3D_Helper.Reciprocal(P3D_Brush.canvasW * P3D_Brush.detailScale.x);
            float num2 = P3D_Helper.Reciprocal(P3D_Brush.canvasH * P3D_Brush.detailScale.y);
            Color* colorPtr1 = &P3D_Brush.color;
            colorPtr1->a *= P3D_Brush.opacity;
            int xMin = P3D_Brush.rect.XMin;
            while (xMin < P3D_Brush.rect.XMax)
            {
                int yMin = P3D_Brush.rect.YMin;
                while (true)
                {
                    if (yMin >= P3D_Brush.rect.YMax)
                    {
                        xMin++;
                        break;
                    }
                    if (P3D_Brush.IsInsideShape(P3D_Brush.inverse, xMin, yMin, ref shapeCoord))
                    {
                        Color pixel = P3D_Brush.canvas.GetPixel(xMin, yMin);
                        Color color = P3D_Brush.color;
                        if (P3D_Brush.shape != null)
                        {
                            color *= P3D_Brush.shape.GetPixelBilinear(shapeCoord.x, shapeCoord.y);
                        }
                        if (P3D_Brush.detail != null)
                        {
                            color *= P3D_Brush.SampleRepeat(P3D_Brush.detail, num * xMin, num2 * yMin);
                        }
                        P3D_Brush.canvas.SetPixel(xMin, yMin, Blend(pixel, color));
                    }
                    yMin++;
                }
            }
        }
    }

    private static class AlphaBlendRGB
    {
        private static Color Blend(Color old, Color add)
        {
            if ((old.a > 0f) && (add.a > 0f))
            {
                float a = add.a;
                float num2 = 1f - a;
                float num3 = old.a;
                float num4 = a + (num3 * num2);
                old.r = ((add.r * a) + ((old.r * num3) * num2)) / num4;
                old.g = ((add.g * a) + ((old.g * num3) * num2)) / num4;
                old.b = ((add.b * a) + ((old.b * num3) * num2)) / num4;
            }
            return old;
        }

        public static unsafe void Paint()
        {
            Vector2 shapeCoord = new Vector2();
            float num = P3D_Helper.Reciprocal(P3D_Brush.canvasW * P3D_Brush.detailScale.x);
            float num2 = P3D_Helper.Reciprocal(P3D_Brush.canvasH * P3D_Brush.detailScale.y);
            Color* colorPtr1 = &P3D_Brush.color;
            colorPtr1->a *= P3D_Brush.opacity;
            int xMin = P3D_Brush.rect.XMin;
            while (xMin < P3D_Brush.rect.XMax)
            {
                int yMin = P3D_Brush.rect.YMin;
                while (true)
                {
                    if (yMin >= P3D_Brush.rect.YMax)
                    {
                        xMin++;
                        break;
                    }
                    if (P3D_Brush.IsInsideShape(P3D_Brush.inverse, xMin, yMin, ref shapeCoord))
                    {
                        Color pixel = P3D_Brush.canvas.GetPixel(xMin, yMin);
                        Color color = P3D_Brush.color;
                        if (P3D_Brush.shape != null)
                        {
                            color *= P3D_Brush.shape.GetPixelBilinear(shapeCoord.x, shapeCoord.y);
                        }
                        if (P3D_Brush.detail != null)
                        {
                            color *= P3D_Brush.SampleRepeat(P3D_Brush.detail, num * xMin, num2 * yMin);
                        }
                        P3D_Brush.canvas.SetPixel(xMin, yMin, Blend(pixel, color));
                    }
                    yMin++;
                }
            }
        }
    }

    private static class AlphaErase
    {
        private static unsafe Color Blend(Color old, float sub)
        {
            Color* colorPtr1 = &old;
            colorPtr1->a -= sub;
            return old;
        }

        public static unsafe void Paint()
        {
            Vector2 shapeCoord = new Vector2();
            float num = P3D_Helper.Reciprocal(P3D_Brush.canvasW * P3D_Brush.detailScale.x);
            float num2 = P3D_Helper.Reciprocal(P3D_Brush.canvasH * P3D_Brush.detailScale.y);
            Color* colorPtr1 = &P3D_Brush.color;
            colorPtr1->a *= P3D_Brush.opacity;
            int xMin = P3D_Brush.rect.XMin;
            while (xMin < P3D_Brush.rect.XMax)
            {
                int yMin = P3D_Brush.rect.YMin;
                while (true)
                {
                    if (yMin >= P3D_Brush.rect.YMax)
                    {
                        xMin++;
                        break;
                    }
                    if (P3D_Brush.IsInsideShape(P3D_Brush.inverse, xMin, yMin, ref shapeCoord))
                    {
                        Color pixel = P3D_Brush.canvas.GetPixel(xMin, yMin);
                        float opacity = P3D_Brush.opacity;
                        if (P3D_Brush.shape != null)
                        {
                            opacity *= P3D_Brush.shape.GetPixelBilinear(shapeCoord.x, shapeCoord.y).a;
                        }
                        if (P3D_Brush.detail != null)
                        {
                            opacity *= P3D_Brush.SampleRepeat(P3D_Brush.detail, num * xMin, num2 * yMin).a;
                        }
                        P3D_Brush.canvas.SetPixel(xMin, yMin, Blend(pixel, opacity));
                    }
                    yMin++;
                }
            }
        }
    }

    private static class NormalBlend
    {
        private static Vector3 ColorToNormalXY(Color c)
        {
            Vector3 vector = new Vector3();
            vector.x = (c.r * 2f) - 1f;
            vector.y = (c.g * 2f) - 1f;
            return vector;
        }

        private static unsafe Vector3 CombineNormalsXY(Vector3 a, Vector3 b)
        {
            Vector3* vectorPtr1 = &a;
            vectorPtr1->x += b.x;
            Vector3* vectorPtr2 = &a;
            vectorPtr2->y += b.y;
            return a;
        }

        private static unsafe Vector3 CombineNormalsXY(Vector3 a, Vector2 b, float c)
        {
            Vector3* vectorPtr1 = &a;
            vectorPtr1->x += b.x * c;
            Vector3* vectorPtr2 = &a;
            vectorPtr2->y += b.y * c;
            return a;
        }

        private static unsafe Vector3 CombineNormalsXY(Vector3 a, Vector3 b, float c)
        {
            Vector3* vectorPtr1 = &a;
            vectorPtr1->x += b.x * c;
            Vector3* vectorPtr2 = &a;
            vectorPtr2->y += b.y * c;
            return a;
        }

        private static Vector3 ComputeZ(Vector3 a)
        {
            a.z = Mathf.Sqrt((1f - (a.x * a.x)) + (a.y * a.y));
            return a;
        }

        private static Color NormalToColor(Vector3 n)
        {
            Color color = new Color();
            color.r = (n.x * 0.5f) + 0.5f;
            color.g = (n.y * 0.5f) + 0.5f;
            color.b = (n.z * 0.5f) + 0.5f;
            color.a = color.r;
            return color;
        }

        public static unsafe void Paint()
        {
            Vector2 shapeCoord = new Vector2();
            float num = P3D_Helper.Reciprocal(P3D_Brush.canvasW * P3D_Brush.detailScale.x);
            float num2 = P3D_Helper.Reciprocal(P3D_Brush.canvasH * P3D_Brush.detailScale.y);
            Color* colorPtr1 = &P3D_Brush.color;
            colorPtr1->a *= P3D_Brush.opacity;
            if ((P3D_Brush.shape != null) && (P3D_Brush.shape.format != TextureFormat.Alpha8))
            {
                int xMin = P3D_Brush.rect.XMin;
                while (xMin < P3D_Brush.rect.XMax)
                {
                    int yMin = P3D_Brush.rect.YMin;
                    while (true)
                    {
                        if (yMin >= P3D_Brush.rect.YMax)
                        {
                            xMin++;
                            break;
                        }
                        if (P3D_Brush.IsInsideShape(P3D_Brush.inverse, xMin, yMin, ref shapeCoord))
                        {
                            Vector3 a = ColorToNormalXY(P3D_Brush.canvas.GetPixel(xMin, yMin));
                            Vector3 vector4 = ColorToNormalXY(P3D_Brush.shape.GetPixelBilinear(shapeCoord.x, shapeCoord.y));
                            if (P3D_Brush.detail != null)
                            {
                                vector4 = CombineNormalsXY(vector4, ColorToNormalXY(P3D_Brush.SampleRepeat(P3D_Brush.detail, num * xMin, num2 * yMin)));
                            }
                            P3D_Brush.canvas.SetPixel(xMin, yMin, NormalToColor(Vector3.Normalize(ComputeZ(CombineNormalsXY(a, vector4, P3D_Brush.opacity)))));
                        }
                        yMin++;
                    }
                }
            }
            else
            {
                int xMin = P3D_Brush.rect.XMin;
                while (xMin < P3D_Brush.rect.XMax)
                {
                    int yMin = P3D_Brush.rect.YMin;
                    while (true)
                    {
                        if (yMin >= P3D_Brush.rect.YMax)
                        {
                            xMin++;
                            break;
                        }
                        if (P3D_Brush.IsInsideShape(P3D_Brush.inverse, xMin, yMin, ref shapeCoord))
                        {
                            Vector3 a = ColorToNormalXY(P3D_Brush.canvas.GetPixel(xMin, yMin));
                            Vector2 direction = P3D_Brush.direction;
                            float opacity = P3D_Brush.opacity;
                            if (P3D_Brush.shape != null)
                            {
                                opacity *= P3D_Brush.shape.GetPixelBilinear(shapeCoord.x, shapeCoord.y).a;
                            }
                            if (P3D_Brush.detail != null)
                            {
                                Vector3 b = ColorToNormalXY(P3D_Brush.SampleRepeat(P3D_Brush.detail, num * xMin, num2 * yMin));
                                direction = CombineNormalsXY((Vector3) direction, b);
                            }
                            P3D_Brush.canvas.SetPixel(xMin, yMin, NormalToColor(Vector3.Normalize(ComputeZ(CombineNormalsXY(a, direction, opacity)))));
                        }
                        yMin++;
                    }
                }
            }
        }
    }

    private static class Replace
    {
        private static Color Blend(Color old, Color add)
        {
            if (add.a > 0f)
            {
                float a = add.a;
                float num2 = 1f - a;
                float num3 = old.a;
                float num4 = a + (num3 * num2);
                old.r = ((add.r * a) + ((old.r * num3) * num2)) / num4;
                old.g = ((add.g * a) + ((old.g * num3) * num2)) / num4;
                old.b = ((add.b * a) + ((old.b * num3) * num2)) / num4;
                old.a = num4;
            }
            return old;
        }

        public static unsafe void Paint()
        {
            Vector2 shapeCoord = new Vector2();
            float num = P3D_Helper.Reciprocal(P3D_Brush.canvasW * P3D_Brush.detailScale.x);
            float num2 = P3D_Helper.Reciprocal(P3D_Brush.canvasH * P3D_Brush.detailScale.y);
            Color* colorPtr1 = &P3D_Brush.color;
            colorPtr1->a *= P3D_Brush.opacity;
            int xMin = P3D_Brush.rect.XMin;
            while (xMin < P3D_Brush.rect.XMax)
            {
                int yMin = P3D_Brush.rect.YMin;
                while (true)
                {
                    if (yMin >= P3D_Brush.rect.YMax)
                    {
                        xMin++;
                        break;
                    }
                    if (P3D_Brush.IsInsideShape(P3D_Brush.inverse, xMin, yMin, ref shapeCoord))
                    {
                        Color pixel = P3D_Brush.canvas.GetPixel(xMin, yMin);
                        Color color = P3D_Brush.color;
                        float opacity = P3D_Brush.opacity;
                        if (P3D_Brush.shape != null)
                        {
                            opacity *= P3D_Brush.shape.GetPixelBilinear(shapeCoord.x, shapeCoord.y).a;
                        }
                        if (P3D_Brush.detail != null)
                        {
                            color *= P3D_Brush.SampleRepeat(P3D_Brush.detail, num * xMin, num2 * yMin);
                        }
                        P3D_Brush.canvas.SetPixel(xMin, yMin, Color.Lerp(pixel, color, opacity));
                    }
                    yMin++;
                }
            }
        }
    }

    private static class SubtractiveBlend
    {
        private static unsafe Color Blend(Color old, Color sub)
        {
            Color* colorPtr1 = &old;
            colorPtr1->r -= sub.r;
            Color* colorPtr2 = &old;
            colorPtr2->g -= sub.g;
            Color* colorPtr3 = &old;
            colorPtr3->b -= sub.b;
            Color* colorPtr4 = &old;
            colorPtr4->a -= sub.a;
            return old;
        }

        public static unsafe void Paint()
        {
            Vector2 shapeCoord = new Vector2();
            float num = P3D_Helper.Reciprocal(P3D_Brush.canvasW * P3D_Brush.detailScale.x);
            float num2 = P3D_Helper.Reciprocal(P3D_Brush.canvasH * P3D_Brush.detailScale.y);
            Color* colorPtr1 = &P3D_Brush.color;
            colorPtr1->a *= P3D_Brush.opacity;
            int xMin = P3D_Brush.rect.XMin;
            while (xMin < P3D_Brush.rect.XMax)
            {
                int yMin = P3D_Brush.rect.YMin;
                while (true)
                {
                    if (yMin >= P3D_Brush.rect.YMax)
                    {
                        xMin++;
                        break;
                    }
                    if (P3D_Brush.IsInsideShape(P3D_Brush.inverse, xMin, yMin, ref shapeCoord))
                    {
                        Color pixel = P3D_Brush.canvas.GetPixel(xMin, yMin);
                        Color color = P3D_Brush.color;
                        if (P3D_Brush.shape != null)
                        {
                            color *= P3D_Brush.shape.GetPixelBilinear(shapeCoord.x, shapeCoord.y);
                        }
                        if (P3D_Brush.detail != null)
                        {
                            color *= P3D_Brush.SampleRepeat(P3D_Brush.detail, num * xMin, num2 * yMin);
                        }
                        P3D_Brush.canvas.SetPixel(xMin, yMin, Blend(pixel, color));
                    }
                    yMin++;
                }
            }
        }
    }
}

