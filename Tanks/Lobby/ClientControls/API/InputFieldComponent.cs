namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class InputFieldComponent : EventMappingComponent
    {
        protected UnityEngine.UI.InputField inputField;
        protected TMP_InputField tmpInputField;
        [SerializeField]
        private UnityEngine.Animator animator;
        private string errorMessage;
        [SerializeField]
        private Text errorMessageLabel;
        [SerializeField]
        private TextMeshProUGUI errorMessageTMPLabel;
        private Text hint;

        [DebuggerHidden]
        private IEnumerator _ActivateInputField(Action onComplete) => 
            new <_ActivateInputField>c__Iterator0 { 
                onComplete = onComplete,
                $this = this
            };

        public void ActivateInputField(Action onComplete)
        {
            base.StartCoroutine(this._ActivateInputField(onComplete));
        }

        protected override void Awake()
        {
            base.Awake();
            if (this.TMPInputField != null)
            {
                this.TMPInputField.scrollSensitivity = 0f;
            }
            this.ExtractHint();
        }

        private void ExtractHint()
        {
            if (this.defaultInput)
            {
                Text placeholder = this.InputField.placeholder as Text;
                if (placeholder != null)
                {
                    this.hint = placeholder;
                    this.InputField.placeholder = null;
                }
            }
        }

        public static bool IsAnyInputFieldInFocus()
        {
            GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (currentSelectedGameObject == null)
            {
                return false;
            }
            UnityEngine.UI.InputField component = currentSelectedGameObject.GetComponent<UnityEngine.UI.InputField>();
            if (component != null)
            {
                return component.isFocused;
            }
            TMP_InputField field2 = currentSelectedGameObject.GetComponent<TMP_InputField>();
            return ((field2 != null) && field2.isFocused);
        }

        private void SendInputFieldValueChangedEvent(string s)
        {
            if (this.hint != null)
            {
                this.hint.gameObject.SetActive(string.IsNullOrEmpty(s));
            }
            this.SendEvent<InputFieldValueChangedEvent>();
        }

        public void SendValueChangedEvent()
        {
            this.SendInputFieldValueChangedEvent(this.Input);
        }

        protected override void Subscribe()
        {
            if (this.defaultInput)
            {
                this.InputField.onValueChanged.AddListener(s => this.SendInputFieldValueChangedEvent(s));
            }
            else
            {
                this.TMPInputField.onValueChanged.AddListener(s => this.SendInputFieldValueChangedEvent(s));
                this.TMPInputField.onSelect.AddListener(delegate (string s) {
                    if (!string.IsNullOrEmpty(s))
                    {
                        this.SendInputFieldValueChangedEvent(s);
                    }
                });
            }
        }

        public virtual UnityEngine.UI.InputField InputField
        {
            get
            {
                if (this.inputField == null)
                {
                    this.inputField = base.GetComponent<UnityEngine.UI.InputField>();
                }
                return this.inputField;
            }
        }

        public virtual TMP_InputField TMPInputField
        {
            get
            {
                if (this.tmpInputField == null)
                {
                    this.tmpInputField = base.GetComponent<TMP_InputField>();
                }
                return this.tmpInputField;
            }
        }

        private bool defaultInput =>
            this.InputField != null;

        public GameObject InputFieldGameObject =>
            !this.defaultInput ? this.TMPInputField.gameObject : this.InputField.gameObject;

        public UnityEngine.Animator Animator =>
            this.animator;

        public virtual string ErrorMessage
        {
            get => 
                this.errorMessage;
            set
            {
                this.errorMessage = value;
                bool flag = !string.IsNullOrEmpty(value);
                this.Animator.SetBool("HasMessage", flag);
                if (this.defaultInput)
                {
                    this.errorMessageLabel.text = this.errorMessage;
                }
                else
                {
                    this.errorMessageTMPLabel.text = this.errorMessage;
                }
            }
        }

        public virtual string Input
        {
            get => 
                !this.defaultInput ? this.TMPInputField.text : this.InputField.text;
            set
            {
                if (this.defaultInput)
                {
                    this.InputField.text = value;
                }
                else
                {
                    this.TMPInputField.text = value;
                }
            }
        }

        public virtual string Hint
        {
            set
            {
                if (this.hint == null)
                {
                    this.ExtractHint();
                }
                if (this.hint != null)
                {
                    this.hint.text = value;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <_ActivateInputField>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal Action onComplete;
            internal InputFieldComponent $this;
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
                        if (this.$this.tmpInputField != null)
                        {
                            this.$this.tmpInputField.Select();
                        }
                        else if (this.$this.inputField != null)
                        {
                            this.$this.inputField.Select();
                        }
                        this.onComplete();
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

