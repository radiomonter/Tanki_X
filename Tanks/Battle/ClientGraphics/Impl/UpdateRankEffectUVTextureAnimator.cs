namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class UpdateRankEffectUVTextureAnimator : MonoBehaviour
    {
        public Material[] AnimatedMaterialsNotInstance;
        public int Rows = 4;
        public int Columns = 4;
        public float Fps = 20f;
        public int OffsetMat;
        public Vector2 SelfTiling;
        public bool IsLoop;
        public bool IsReverse;
        public bool IsRandomOffsetForInctance;
        public bool IsBump;
        public bool IsHeight;
        public bool IsCutOut;
        private bool isInizialised;
        private int index;
        private int count;
        private int allCount;
        private float deltaFps;
        private bool isVisible;
        private bool isCorutineStarted;
        private Renderer currentRenderer;
        private Material instanceMaterial;

        public UpdateRankEffectUVTextureAnimator()
        {
            Vector2 vector = new Vector2();
            this.SelfTiling = vector;
            this.IsLoop = true;
        }

        private void InitDefaultVariables()
        {
            this.allCount = 0;
            this.deltaFps = 1f / this.Fps;
            this.count = this.Rows * this.Columns;
            this.index = this.Columns - 1;
            Vector2 vector = new Vector2((((float) this.index) / ((float) this.Columns)) - (this.index / this.Columns), 1f - (((float) (this.index / this.Columns)) / ((float) this.Rows)));
            this.OffsetMat = this.IsRandomOffsetForInctance ? Random.Range(0, this.count) : (this.OffsetMat - ((this.OffsetMat / this.count) * this.count));
            Vector2 vector2 = !(this.SelfTiling == Vector2.zero) ? this.SelfTiling : new Vector2(1f / ((float) this.Columns), 1f / ((float) this.Rows));
            if (this.AnimatedMaterialsNotInstance.Length > 0)
            {
                foreach (Material material in this.AnimatedMaterialsNotInstance)
                {
                    material.SetTextureScale("_MainTex", vector2);
                    material.SetTextureOffset("_MainTex", Vector2.zero);
                    if (this.IsBump)
                    {
                        material.SetTextureScale("_BumpMap", vector2);
                        material.SetTextureOffset("_BumpMap", Vector2.zero);
                    }
                    if (this.IsHeight)
                    {
                        material.SetTextureScale("_HeightMap", vector2);
                        material.SetTextureOffset("_HeightMap", Vector2.zero);
                    }
                    if (this.IsCutOut)
                    {
                        material.SetTextureScale("_CutOut", vector2);
                        material.SetTextureOffset("_CutOut", Vector2.zero);
                    }
                }
            }
            else if (this.instanceMaterial != null)
            {
                this.instanceMaterial.SetTextureScale("_MainTex", vector2);
                this.instanceMaterial.SetTextureOffset("_MainTex", vector);
                if (this.IsBump)
                {
                    this.instanceMaterial.SetTextureScale("_BumpMap", vector2);
                    this.instanceMaterial.SetTextureOffset("_BumpMap", vector);
                }
                if (this.IsBump)
                {
                    this.instanceMaterial.SetTextureScale("_HeightMap", vector2);
                    this.instanceMaterial.SetTextureOffset("_HeightMap", vector);
                }
                if (this.IsCutOut)
                {
                    this.instanceMaterial.SetTextureScale("_CutOut", vector2);
                    this.instanceMaterial.SetTextureOffset("_CutOut", vector);
                }
            }
            else if (this.currentRenderer != null)
            {
                this.currentRenderer.material.SetTextureScale("_MainTex", vector2);
                this.currentRenderer.material.SetTextureOffset("_MainTex", vector);
                if (this.IsBump)
                {
                    this.currentRenderer.material.SetTextureScale("_BumpMap", vector2);
                    this.currentRenderer.material.SetTextureOffset("_BumpMap", vector);
                }
                if (this.IsHeight)
                {
                    this.currentRenderer.material.SetTextureScale("_HeightMap", vector2);
                    this.currentRenderer.material.SetTextureOffset("_HeightMap", vector);
                }
                if (this.IsCutOut)
                {
                    this.currentRenderer.material.SetTextureScale("_CutOut", vector2);
                    this.currentRenderer.material.SetTextureOffset("_CutOut", vector);
                }
            }
        }

        private void InitMaterial()
        {
            if (base.GetComponent<Renderer>() != null)
            {
                this.currentRenderer = base.GetComponent<Renderer>();
            }
            else
            {
                Projector component = base.GetComponent<Projector>();
                if (component != null)
                {
                    if (!component.material.name.EndsWith("(Instance)"))
                    {
                        Material material = new Material(component.material) {
                            name = component.material.name + " (Instance)"
                        };
                        component.material = material;
                    }
                    this.instanceMaterial = component.material;
                }
            }
        }

        private void OnBecameInvisible()
        {
            this.isVisible = false;
        }

        private void OnBecameVisible()
        {
            this.isVisible = true;
            if (!this.isCorutineStarted)
            {
                base.StartCoroutine(this.UpdateCorutine());
            }
        }

        private void OnDisable()
        {
            this.isCorutineStarted = false;
            this.isVisible = false;
            base.StopAllCoroutines();
        }

        private void OnEnable()
        {
            if (this.isInizialised)
            {
                this.InitDefaultVariables();
                this.isVisible = true;
                if (!this.isCorutineStarted)
                {
                    base.StartCoroutine(this.UpdateCorutine());
                }
            }
        }

        public void SetInstanceMaterial(Material mat, Vector2 offsetMat)
        {
            this.instanceMaterial = mat;
            this.InitDefaultVariables();
        }

        private void Start()
        {
            this.InitMaterial();
            this.InitDefaultVariables();
            this.isInizialised = true;
            this.isVisible = true;
            base.StartCoroutine(this.UpdateCorutine());
        }

        [DebuggerHidden]
        private IEnumerator UpdateCorutine() => 
            new <UpdateCorutine>c__Iterator0 { $this = this };

        private void UpdateCorutineFrame()
        {
            if ((this.currentRenderer != null) || ((this.instanceMaterial != null) || (this.AnimatedMaterialsNotInstance.Length != 0)))
            {
                this.allCount++;
                this.index = !this.IsReverse ? (this.index + 1) : (this.index - 1);
                if (this.index >= this.count)
                {
                    this.index = 0;
                }
                if (this.AnimatedMaterialsNotInstance.Length > 0)
                {
                    for (int i = 0; i < this.AnimatedMaterialsNotInstance.Length; i++)
                    {
                        int num2 = ((i * this.OffsetMat) + this.index) + this.OffsetMat;
                        num2 -= (num2 / this.count) * this.count;
                        Vector2 vector = new Vector2((((float) num2) / ((float) this.Columns)) - (num2 / this.Columns), 1f - (((float) (num2 / this.Columns)) / ((float) this.Rows)));
                        this.AnimatedMaterialsNotInstance[i].SetTextureOffset("_MainTex", vector);
                        if (this.IsBump)
                        {
                            this.AnimatedMaterialsNotInstance[i].SetTextureOffset("_BumpMap", vector);
                        }
                        if (this.IsHeight)
                        {
                            this.AnimatedMaterialsNotInstance[i].SetTextureOffset("_HeightMap", vector);
                        }
                        if (this.IsCutOut)
                        {
                            this.AnimatedMaterialsNotInstance[i].SetTextureOffset("_CutOut", vector);
                        }
                    }
                }
                else
                {
                    Vector2 vector2;
                    if (!this.IsRandomOffsetForInctance)
                    {
                        vector2 = new Vector2((((float) this.index) / ((float) this.Columns)) - (this.index / this.Columns), 1f - (((float) (this.index / this.Columns)) / ((float) this.Rows)));
                    }
                    else
                    {
                        int num3 = this.index + this.OffsetMat;
                        vector2 = new Vector2((((float) num3) / ((float) this.Columns)) - (num3 / this.Columns), 1f - (((float) (num3 / this.Columns)) / ((float) this.Rows)));
                    }
                    if (this.instanceMaterial != null)
                    {
                        this.instanceMaterial.SetTextureOffset("_MainTex", vector2);
                        if (this.IsBump)
                        {
                            this.instanceMaterial.SetTextureOffset("_BumpMap", vector2);
                        }
                        if (this.IsHeight)
                        {
                            this.instanceMaterial.SetTextureOffset("_HeightMap", vector2);
                        }
                        if (this.IsCutOut)
                        {
                            this.instanceMaterial.SetTextureOffset("_CutOut", vector2);
                        }
                    }
                    else if (this.currentRenderer != null)
                    {
                        this.currentRenderer.material.SetTextureOffset("_MainTex", vector2);
                        if (this.IsBump)
                        {
                            this.currentRenderer.material.SetTextureOffset("_BumpMap", vector2);
                        }
                        if (this.IsHeight)
                        {
                            this.currentRenderer.material.SetTextureOffset("_HeightMap", vector2);
                        }
                        if (this.IsCutOut)
                        {
                            this.currentRenderer.material.SetTextureOffset("_CutOut", vector2);
                        }
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateCorutine>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal UpdateRankEffectUVTextureAnimator $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$this.isCorutineStarted = true;
                        break;

                    case 1:
                        break;

                    default:
                        goto TR_0000;
                }
                if (this.$this.isVisible && (this.$this.IsLoop || (this.$this.allCount != this.$this.count)))
                {
                    this.$this.UpdateCorutineFrame();
                    if (this.$this.IsLoop || (this.$this.allCount != this.$this.count))
                    {
                        this.$current = new WaitForSeconds(this.$this.deltaFps);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                    }
                }
                this.$this.isCorutineStarted = false;
                this.$PC = -1;
            TR_0000:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

