namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class PresetNameEditorComponent : UIBehaviour, Component, IPointerClickHandler, IEventSystemHandler
    {
        [SerializeField]
        private MainScreenComponent mainScreen;
        [SerializeField]
        private EntityBehaviour entityBehaviour;
        [SerializeField]
        private TMP_InputField inputField;
        [SerializeField]
        private Button editButton;
        private string nameBeforeEdit;
        [CompilerGenerated]
        private static Func<char, bool> <>f__mg$cache0;

        protected override void Awake()
        {
            this.editButton.onClick.AddListener(new UnityAction(this.OnBeginEdit));
            this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
        }

        public void DisableInput()
        {
            this.inputField.interactable = false;
        }

        public void EnableInput()
        {
            this.inputField.interactable = true;
        }

        [DebuggerHidden]
        private IEnumerator LateEndEdit() => 
            new <LateEndEdit>c__Iterator0 { $this = this };

        private void OnBeginEdit()
        {
            this.nameBeforeEdit = this.inputField.text;
            this.editButton.gameObject.SetActive(false);
            this.inputField.enabled = true;
            this.inputField.ActivateInputField();
        }

        private void OnEndEdit(string value)
        {
            this.editButton.gameObject.SetActive(true);
            base.StartCoroutine(this.LateEndEdit());
            if (!string.IsNullOrEmpty(value) && !value.Contains<char>('\n'))
            {
                if (<>f__mg$cache0 == null)
                {
                    <>f__mg$cache0 = new Func<char, bool>(char.IsWhiteSpace);
                }
                if (!value.All<char>(<>f__mg$cache0))
                {
                    if (!this.nameBeforeEdit.Equals(value))
                    {
                        EngineService.Engine.ScheduleEvent<PresetNameChangedEvent>(this.entityBehaviour.Entity);
                    }
                    return;
                }
            }
            this.inputField.text = this.nameBeforeEdit;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount > 1)
            {
                this.OnBeginEdit();
            }
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public string PresetName
        {
            get => 
                this.inputField.text;
            set => 
                this.inputField.text = value;
        }

        [CompilerGenerated]
        private sealed class <LateEndEdit>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal PresetNameEditorComponent $this;
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
                        this.$current = new WaitForEndOfFrame();
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$this.inputField.enabled = false;
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

