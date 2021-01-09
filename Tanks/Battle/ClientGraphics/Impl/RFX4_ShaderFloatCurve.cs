namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class RFX4_ShaderFloatCurve : MonoBehaviour
    {
        public RFX4_ShaderProperties ShaderFloatProperty = RFX4_ShaderProperties._Cutoff;
        public AnimationCurve FloatCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public float GraphTimeMultiplier = 1f;
        public float GraphIntensityMultiplier = 1f;
        public bool IsLoop;
        public bool UseSharedMaterial;
        private bool canUpdate;
        private float startTime;
        private Material mat;
        private float startFloat;
        private int propertyID;
        private string shaderProperty;
        private bool isInitialized;

        private void Awake()
        {
            Renderer component = base.GetComponent<Renderer>();
            if (component != null)
            {
                this.mat = this.UseSharedMaterial ? component.sharedMaterial : component.material;
            }
            else
            {
                Projector projector = base.GetComponent<Projector>();
                if (projector != null)
                {
                    if (this.UseSharedMaterial)
                    {
                        this.mat = projector.material;
                    }
                    else
                    {
                        if (!projector.material.name.EndsWith("(Instance)"))
                        {
                            Material material = new Material(projector.material) {
                                name = projector.material.name + " (Instance)"
                            };
                            projector.material = material;
                        }
                        this.mat = projector.material;
                    }
                }
            }
            this.shaderProperty = this.ShaderFloatProperty.ToString();
            if (this.mat.HasProperty(this.shaderProperty))
            {
                this.propertyID = Shader.PropertyToID(this.shaderProperty);
            }
            this.startFloat = this.mat.GetFloat(this.propertyID);
            float num = this.FloatCurve.Evaluate(0f) * this.GraphIntensityMultiplier;
            this.mat.SetFloat(this.propertyID, num);
            this.isInitialized = true;
        }

        private void OnDestroy()
        {
            if (!this.UseSharedMaterial)
            {
                if (this.mat != null)
                {
                    DestroyImmediate(this.mat);
                }
                this.mat = null;
            }
        }

        private void OnDisable()
        {
            if (this.UseSharedMaterial)
            {
                this.mat.SetFloat(this.propertyID, this.startFloat);
            }
        }

        private void OnEnable()
        {
            this.startTime = Time.time;
            this.canUpdate = true;
            if (this.isInitialized)
            {
                float num = this.FloatCurve.Evaluate(0f) * this.GraphIntensityMultiplier;
                this.mat.SetFloat(this.propertyID, num);
            }
        }

        private void Update()
        {
            float num = Time.time - this.startTime;
            if (this.canUpdate)
            {
                float num2 = this.FloatCurve.Evaluate(num / this.GraphTimeMultiplier) * this.GraphIntensityMultiplier;
                this.mat.SetFloat(this.propertyID, num2);
            }
            if (num >= this.GraphTimeMultiplier)
            {
                if (this.IsLoop)
                {
                    this.startTime = Time.time;
                }
                else
                {
                    this.canUpdate = false;
                }
            }
        }
    }
}

