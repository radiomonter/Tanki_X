namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.Serialization;
    using UnityEngine.UI;

    public class ScreenComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private bool logInHistory = true;
        [SerializeField]
        private bool showHangar = true;
        [SerializeField]
        private bool rotateHangarCamera = true;
        [SerializeField]
        private bool showItemNotifications = true;
        [SerializeField, HideInInspector, FormerlySerializedAs("visibleTopPanelItems")]
        private List<string> visibleCommonScreenElements = new List<string>();
        [SerializeField]
        private bool showNotifications = true;
        [Tooltip("Элемент экрана, который должен быть выбран по умолчанию"), SerializeField]
        private Selectable defaultControl;
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            this.canvasGroup = base.GetComponent<CanvasGroup>();
            if (this.canvasGroup == null)
            {
                this.canvasGroup = base.gameObject.AddComponent<CanvasGroup>();
            }
        }

        [DebuggerHidden]
        private IEnumerator DelayFocus() => 
            new <DelayFocus>c__Iterator0 { $this = this };

        public void Lock()
        {
            this.canvasGroup.blocksRaycasts = false;
        }

        private void OnEnable()
        {
            base.StartCoroutine(this.DelayFocus());
        }

        private void Reset()
        {
            this.visibleCommonScreenElements.Add(0.ToString());
        }

        public void Unlock()
        {
            this.canvasGroup.blocksRaycasts = true;
        }

        public List<string> VisibleCommonScreenElements =>
            this.visibleCommonScreenElements;

        public bool LogInHistory =>
            this.logInHistory;

        public bool ShowHangar =>
            this.showHangar;

        public bool RotateHangarCamera =>
            this.rotateHangarCamera;

        public bool ShowItemNotifications =>
            this.showItemNotifications;

        public bool ShowNotifications =>
            this.showNotifications;

        [CompilerGenerated]
        private sealed class <DelayFocus>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal ScreenComponent $this;
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
                        this.$current = new WaitForSeconds(0f);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        if (this.$this.defaultControl != null)
                        {
                            EventSystem.current.SetSelectedGameObject(null);
                            this.$this.defaultControl.Select();
                        }
                        this.$PC = -1;
                        break;

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

