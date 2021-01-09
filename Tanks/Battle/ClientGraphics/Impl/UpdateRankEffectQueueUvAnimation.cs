namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class UpdateRankEffectQueueUvAnimation : MonoBehaviour
    {
        public int RowsFadeIn = 4;
        public int ColumnsFadeIn = 4;
        public int RowsLoop = 4;
        public int ColumnsLoop = 4;
        public float Fps = 20f;
        public bool IsBump;
        public Material NextMaterial;
        private int index;
        private int count;
        private int allCount;
        private float deltaTime;
        private bool isVisible;
        private bool isFadeHandle;

        private void InitDefaultTex(int rows, int colums)
        {
            this.count = rows * colums;
            this.index += colums - 1;
            Vector2 vector = new Vector2(1f / ((float) colums), 1f / ((float) rows));
            base.GetComponent<Renderer>().material.SetTextureScale("_MainTex", vector);
            if (this.IsBump)
            {
                base.GetComponent<Renderer>().material.SetTextureScale("_BumpMap", vector);
            }
        }

        private void OnBecameInvisible()
        {
            this.isVisible = false;
        }

        private void OnBecameVisible()
        {
            this.isVisible = true;
            base.StartCoroutine(this.UpdateTiling());
        }

        private void Start()
        {
            this.deltaTime = 1f / this.Fps;
            this.InitDefaultTex(this.RowsFadeIn, this.ColumnsFadeIn);
        }

        [DebuggerHidden]
        private IEnumerator UpdateTiling() => 
            new <UpdateTiling>c__Iterator0 { $this = this };

        [CompilerGenerated]
        private sealed class <UpdateTiling>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal Vector2 <offset>__1;
            internal UpdateRankEffectQueueUvAnimation $this;
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
                    case 1:
                        if (!this.$this.isVisible || (this.$this.allCount == this.$this.count))
                        {
                            this.$PC = -1;
                            break;
                        }
                        this.$this.allCount++;
                        this.$this.index++;
                        if (this.$this.index >= this.$this.count)
                        {
                            this.$this.index = 0;
                        }
                        this.<offset>__1 = this.$this.isFadeHandle ? new Vector2((((float) this.$this.index) / ((float) this.$this.ColumnsLoop)) - (this.$this.index / this.$this.ColumnsLoop), 1f - (((float) (this.$this.index / this.$this.ColumnsLoop)) / ((float) this.$this.RowsLoop))) : new Vector2((((float) this.$this.index) / ((float) this.$this.ColumnsFadeIn)) - (this.$this.index / this.$this.ColumnsFadeIn), 1f - (((float) (this.$this.index / this.$this.ColumnsFadeIn)) / ((float) this.$this.RowsFadeIn)));
                        if (!this.$this.isFadeHandle)
                        {
                            this.$this.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", this.<offset>__1);
                            if (this.$this.IsBump)
                            {
                                this.$this.GetComponent<Renderer>().material.SetTextureOffset("_BumpMap", this.<offset>__1);
                            }
                        }
                        else
                        {
                            this.$this.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", this.<offset>__1);
                            if (this.$this.IsBump)
                            {
                                this.$this.GetComponent<Renderer>().material.SetTextureOffset("_BumpMap", this.<offset>__1);
                            }
                        }
                        if (this.$this.allCount == this.$this.count)
                        {
                            this.$this.isFadeHandle = true;
                            this.$this.GetComponent<Renderer>().material = this.$this.NextMaterial;
                            this.$this.InitDefaultTex(this.$this.RowsLoop, this.$this.ColumnsLoop);
                        }
                        this.$current = new WaitForSeconds(this.$this.deltaTime);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    default:
                        break;
                }
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

