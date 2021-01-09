namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class UpdateRankEffectRandomRotate : MonoBehaviour
    {
        public bool isRotate = true;
        public int fps = 30;
        public int x = 100;
        public int y = 200;
        public int z = 300;
        private float rangeX;
        private float rangeY;
        private float rangeZ;
        private float deltaTime;
        private bool isVisible;

        private void OnBecameInvisible()
        {
            this.isVisible = false;
        }

        private void OnBecameVisible()
        {
            this.isVisible = true;
            base.StartCoroutine(this.UpdateRotation());
        }

        private void Start()
        {
            this.deltaTime = 1f / ((float) this.fps);
            this.rangeX = Random.Range(0, 10);
            this.rangeY = Random.Range(0, 10);
            this.rangeZ = Random.Range(0, 10);
        }

        [DebuggerHidden]
        private IEnumerator UpdateRotation() => 
            new <UpdateRotation>c__Iterator0 { $this = this };

        [CompilerGenerated]
        private sealed class <UpdateRotation>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal UpdateRankEffectRandomRotate $this;
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
                        if (!this.$this.isVisible)
                        {
                            this.$PC = -1;
                            break;
                        }
                        if (this.$this.isRotate)
                        {
                            this.$this.transform.Rotate((float) ((this.$this.deltaTime * Mathf.Sin(Time.time + this.$this.rangeX)) * this.$this.x), (this.$this.deltaTime * Mathf.Sin(Time.time + this.$this.rangeY)) * this.$this.y, (float) ((this.$this.deltaTime * Mathf.Sin(Time.time + this.$this.rangeZ)) * this.$this.z));
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

