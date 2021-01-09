namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class LineRendererEffectBehaviour : MonoBehaviour
    {
        [SerializeField]
        private LineRendererEffect[] effects;
        public float duration = 2f;
        public bool invertAlpha;
        private Material[] materials;
        private float elapsed;

        private void Awake()
        {
            this.LastScale = new float[this.effects.Length];
        }

        public void Init(float[] offsets, params Vector3[] vertices)
        {
            float num = 0f;
            for (int i = 1; i < vertices.Length; i++)
            {
                num += Vector3.Distance(vertices[i - 1], vertices[i]);
            }
            this.materials = new Material[this.effects.Length];
            int index = 0;
            while (index < this.effects.Length)
            {
                LineRenderer lineRenderer = this.effects[index].lineRenderer;
                lineRenderer.material = Instantiate<Material>(lineRenderer.material);
                this.materials[index] = lineRenderer.material;
                Vector2 mainTextureOffset = this.materials[index].mainTextureOffset;
                if (this.effects[index].adjustTextureScale)
                {
                    Vector2 mainTextureScale = this.materials[index].mainTextureScale;
                    mainTextureScale.x = num / this.effects[index].fragmentLength;
                    this.materials[index].mainTextureScale = mainTextureScale;
                    this.LastScale[index] = mainTextureScale.x;
                    mainTextureOffset.x = offsets[index] % 1f;
                }
                this.materials[index].mainTextureOffset = mainTextureOffset;
                int num4 = 0;
                while (true)
                {
                    if (num4 >= vertices.Length)
                    {
                        index++;
                        break;
                    }
                    lineRenderer.SetPosition(num4, vertices[num4]);
                    num4++;
                }
            }
        }

        private void OnEnable()
        {
            this.elapsed = 0f;
        }

        private void Update()
        {
            this.elapsed = (this.elapsed + Time.deltaTime) % this.duration;
            float time = this.elapsed / this.duration;
            for (int i = 0; i < this.effects.Length; i++)
            {
                float end = this.effects[i].width.Evaluate(time);
                float start = this.effects[i].widthEnd.Evaluate(time);
                Color color = this.effects[i].color.Evaluate(time);
                Color color2 = this.effects[i].colorEnd.Evaluate(time);
                if (this.invertAlpha)
                {
                    this.effects[i].lineRenderer.SetWidth(start, end);
                    this.effects[i].lineRenderer.SetColors(color2, color);
                }
                else
                {
                    this.effects[i].lineRenderer.SetWidth(end, start);
                    this.effects[i].lineRenderer.SetColors(color, color2);
                }
                Vector2 mainTextureOffset = this.effects[i].lineRenderer.sharedMaterial.mainTextureOffset;
                mainTextureOffset.x = (mainTextureOffset.x + (this.effects[i].textureOffset.Evaluate(this.elapsed) * Time.deltaTime)) % 1f;
                this.effects[i].lineRenderer.sharedMaterial.mainTextureOffset = mainTextureOffset;
            }
        }

        public float[] LastScale { get; private set; }

        [Serializable]
        private class LineRendererEffect
        {
            public LineRenderer lineRenderer;
            public AnimationCurve width;
            public AnimationCurve widthEnd;
            public Gradient color;
            public Gradient colorEnd;
            public float fragmentLength;
            public AnimationCurve textureOffset;
            public bool adjustTextureScale;
        }
    }
}

