namespace UnityEngine.UI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("UI/Effects/Gradient")]
    public class Gradient : BaseMeshEffect
    {
        [SerializeField]
        private Type _gradientType;
        [SerializeField]
        private Blend _blendMode = Blend.Multiply;
        [SerializeField, Range(-1f, 1f)]
        private float _offset;
        [SerializeField]
        private Gradient _effectGradient;

        public Gradient()
        {
            Gradient gradient = new Gradient();
            gradient.colorKeys = new GradientColorKey[] { new GradientColorKey(Color.black, 0f), new GradientColorKey(Color.white, 1f) };
            this._effectGradient = gradient;
        }

        private Color BlendColor(Color colorA, Color colorB)
        {
            Blend blendMode = this.BlendMode;
            return ((blendMode == Blend.Add) ? (colorA + colorB) : ((blendMode == Blend.Multiply) ? (colorA * colorB) : colorB));
        }

        public override void ModifyMesh(VertexHelper helper)
        {
            if (this.IsActive() && (helper.currentVertCount != 0))
            {
                List<UIVertex> stream = new List<UIVertex>();
                helper.GetUIVertexStream(stream);
                int count = stream.Count;
                Type gradientType = this.GradientType;
                if (gradientType == Type.Horizontal)
                {
                    float x = stream[0].position.x;
                    float num3 = stream[0].position.x;
                    float num4 = 0f;
                    int num5 = count - 1;
                    while (true)
                    {
                        if (num5 < 1)
                        {
                            float num6 = 1f / (num3 - x);
                            UIVertex vertex = new UIVertex();
                            for (int i = 0; i < helper.currentVertCount; i++)
                            {
                                helper.PopulateUIVertex(ref vertex, i);
                                vertex.color = this.BlendColor((Color) vertex.color, this.EffectGradient.Evaluate(((vertex.position.x - x) * num6) - this.Offset));
                                helper.SetUIVertex(vertex, i);
                            }
                            break;
                        }
                        UIVertex vertex3 = stream[num5];
                        num4 = vertex3.position.x;
                        if (num4 > num3)
                        {
                            num3 = num4;
                        }
                        else if (num4 < x)
                        {
                            x = num4;
                        }
                        num5--;
                    }
                }
                else if (gradientType == Type.Vertical)
                {
                    float y = stream[0].position.y;
                    float num9 = stream[0].position.y;
                    float num10 = 0f;
                    int num11 = count - 1;
                    while (true)
                    {
                        if (num11 < 1)
                        {
                            float num12 = 1f / (num9 - y);
                            UIVertex vertex = new UIVertex();
                            for (int i = 0; i < helper.currentVertCount; i++)
                            {
                                helper.PopulateUIVertex(ref vertex, i);
                                vertex.color = this.BlendColor((Color) vertex.color, this.EffectGradient.Evaluate(((vertex.position.y - y) * num12) - this.Offset));
                                helper.SetUIVertex(vertex, i);
                            }
                            break;
                        }
                        UIVertex vertex7 = stream[num11];
                        num10 = vertex7.position.y;
                        if (num10 > num9)
                        {
                            num9 = num10;
                        }
                        else if (num10 < y)
                        {
                            y = num10;
                        }
                        num11--;
                    }
                }
            }
        }

        public Blend BlendMode
        {
            get => 
                this._blendMode;
            set => 
                this._blendMode = value;
        }

        public Gradient EffectGradient
        {
            get => 
                this._effectGradient;
            set => 
                this._effectGradient = value;
        }

        public Type GradientType
        {
            get => 
                this._gradientType;
            set => 
                this._gradientType = value;
        }

        public float Offset
        {
            get => 
                this._offset;
            set => 
                this._offset = value;
        }

        public enum Blend
        {
            Override,
            Add,
            Multiply
        }

        public enum Type
        {
            Horizontal,
            Vertical
        }
    }
}

