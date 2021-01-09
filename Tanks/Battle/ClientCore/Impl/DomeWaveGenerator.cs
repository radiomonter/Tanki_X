namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class DomeWaveGenerator : MonoBehaviour
    {
        public float radius;
        public float speed;
        private Material mat;
        private int waveIndex;
        private const int MAX_WAVE_COUNT = 5;
        private List<Coroutine> waveCoroutines;

        public DomeWaveGenerator()
        {
            List<Coroutine> list = new List<Coroutine> { 
                null,
                null,
                null,
                null,
                null
            };
            this.waveCoroutines = list;
        }

        public void GenerateWave(Vector3 hitPoint)
        {
            if (this.waveCoroutines[this.waveIndex] != null)
            {
                base.StopCoroutine(this.waveCoroutines[this.waveIndex]);
            }
            Coroutine coroutine = base.StartCoroutine(this.GenerateWave(this.waveIndex, hitPoint));
            this.waveCoroutines[this.waveIndex] = coroutine;
            this.waveIndex = (this.waveIndex != 4) ? (this.waveIndex + 1) : 0;
        }

        [DebuggerHidden]
        private IEnumerator GenerateWave(int waveIndex, Vector3 hitPoint) => 
            new <GenerateWave>c__Iterator0 { 
                waveIndex = waveIndex,
                hitPoint = hitPoint,
                $this = this
            };

        public void Init()
        {
            this.mat = base.GetComponent<Renderer>().material;
            this.mat.SetFloat("_Radius0", 0.3f);
            this.mat.SetFloat("_Radius1", 0.3f);
            this.mat.SetFloat("_Radius2", 0.3f);
            this.mat.SetFloat("_Radius3", 0.3f);
            this.mat.SetFloat("_Radius4", 0.3f);
            this.waveIndex = 0;
        }

        private void OnDestroy()
        {
            base.StopAllCoroutines();
        }

        [CompilerGenerated]
        private sealed class <GenerateWave>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal float <f>__1;
            internal int waveIndex;
            internal Vector3 hitPoint;
            internal DomeWaveGenerator $this;
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
                        this.<f>__1 = 0f;
                        break;

                    case 1:
                        this.<f>__1 += this.$this.radius / this.$this.speed;
                        break;

                    default:
                        goto TR_0000;
                }
                if (this.<f>__1 <= this.$this.radius)
                {
                    this.$this.mat.SetFloat("_Radius" + this.waveIndex, this.<f>__1);
                    this.$this.mat.SetVector("_pos" + this.waveIndex, this.hitPoint);
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.$this.waveCoroutines[this.waveIndex] = null;
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

