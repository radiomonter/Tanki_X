namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CameraOffsetBehaviour : BehaviourComponent
    {
        [SerializeField]
        private float offsetX;
        [SerializeField]
        private float offsetY;
        [SerializeField]
        private float animationDuration = 0.2f;
        private Camera camera;
        private Coroutine coroutine;

        public void AnimateOffset(float offset)
        {
            if (this.coroutine != null)
            {
                base.StopCoroutine(this.coroutine);
                this.coroutine = null;
            }
            this.coroutine = base.StartCoroutine(this.AnimateTo(offset));
        }

        [DebuggerHidden]
        private IEnumerator AnimateTo(float offset) => 
            new <AnimateTo>c__Iterator0 { 
                offset = offset,
                $this = this
            };

        private void Awake()
        {
            this.camera = base.GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            this.camera.transform.localPosition = new Vector3(this.offsetX, this.offsetY, 0f);
            this.camera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        private void OnEnable()
        {
            Cursor.visible = true;
        }

        public void SetOffset(float offset)
        {
            this.offsetX = offset;
        }

        public float Offset =>
            this.offsetX;

        [CompilerGenerated]
        private sealed class <AnimateTo>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal float <time>__0;
            internal float <initOffset>__0;
            internal float offset;
            internal CameraOffsetBehaviour $this;
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
                        this.<time>__0 = 0f;
                        this.<initOffset>__0 = this.$this.offsetX;
                        break;

                    case 1:
                        this.<time>__0 += Time.deltaTime;
                        break;

                    default:
                        goto TR_0000;
                }
                if (this.<time>__0 < this.$this.animationDuration)
                {
                    this.$this.offsetX = Mathf.Lerp(this.<initOffset>__0, this.offset, this.<time>__0 / this.$this.animationDuration);
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.$this.offsetX = this.offset;
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

