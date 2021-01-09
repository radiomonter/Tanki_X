namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class KeyboardSettingsInputComponent : MonoBehaviour, Component
    {
        [SerializeField]
        public InputActionContainer[] inputActions;
        [SerializeField]
        private GameObject selectionBorder;
        public int id;
        private InputField inputField;
        private bool selected;
        private bool _wrongKeyState;

        private void AssignKeyText(KeyCode keyCode)
        {
            this.inputField.text = KeyboardSettingsUtil.KeyCodeToString(keyCode);
        }

        private void AssignNewKey(KeyCode newKey)
        {
            foreach (InputActionContainer container in this.inputActions)
            {
                InputManager.ChangeInputActionKey(container.actionId, container.contextId, this.id, newKey);
            }
            this.CheckKeys();
        }

        private void CheckKeys()
        {
            base.GetComponentInParent<KeyboardSettingsScreenComponent>().CheckForOneKeyOnFewActions();
        }

        private bool CurrentKeyAllow(KeyCode code)
        {
            KeyCode[] source = new KeyCode[] { KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2, KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.KeypadEnter, KeyCode.Return, KeyCode.LeftWindows, KeyCode.RightWindows, KeyCode.Space };
            return !source.Contains<KeyCode>(code);
        }

        private void DeleteKeyBinding()
        {
            foreach (InputActionContainer container in this.inputActions)
            {
                InputManager.DeleteKeyBinding(container.actionId, container.contextId, this.id);
            }
            this.CheckKeys();
        }

        private void DeselectInputField()
        {
            EventSystem.current.SetSelectedGameObject(base.transform.parent.gameObject);
        }

        public KeyCode LoadAction()
        {
            InputAction action = InputManager.GetAction(this.inputActions[0].actionId, this.inputActions[0].contextId);
            return ((this.id < action.keys.Length) ? action.keys[this.id] : KeyCode.None);
        }

        public void OnDeselect()
        {
            this.selected = false;
            base.GetComponent<Animator>().SetBool("selected", false);
            if ((this.selectionBorder != null) && !this.wrongKeyState)
            {
                this.selectionBorder.SetActive(false);
            }
        }

        public void OnSelect()
        {
            this.selected = true;
            base.GetComponent<Animator>().SetBool("selected", true);
            if (this.selectionBorder != null)
            {
                this.selectionBorder.SetActive(true);
            }
        }

        public void ResetWrongKeyTrigger()
        {
            base.GetComponent<Animator>().ResetTrigger("wrongKeyPressed");
        }

        public void SetInputState(bool wrongKey)
        {
            this.wrongKeyState = wrongKey;
            if (this.selectionBorder != null)
            {
                if (wrongKey)
                {
                    this.selectionBorder.SetActive(true);
                }
                else if (!this.selected)
                {
                    this.selectionBorder.SetActive(false);
                }
            }
        }

        public void SetText()
        {
            KeyCode keyCode = this.LoadAction();
            if (keyCode != KeyCode.None)
            {
                this.AssignKeyText(keyCode);
            }
            else
            {
                this.inputField.text = string.Empty;
            }
        }

        private void Start()
        {
            this.inputField = base.GetComponent<InputField>();
            this.SetText();
            this.inputField.customCaretColor = true;
            this.inputField.caretColor = Color.clear;
            this.inputField.selectionColor = Color.clear;
        }

        private void Update()
        {
            if (this.selected)
            {
                InputKeyCode currentKeyPressed = InputManager.GetCurrentKeyPressed();
                if (currentKeyPressed != null)
                {
                    KeyCode keyCode = currentKeyPressed.keyCode;
                    if ((keyCode == KeyCode.Delete) || (keyCode == KeyCode.Backspace))
                    {
                        this.DeleteKeyBinding();
                        this.SetText();
                    }
                    else if ((keyCode == KeyCode.Mouse0) || ((keyCode == KeyCode.Mouse1) || (keyCode == KeyCode.Escape)))
                    {
                        this.OnDeselect();
                    }
                    else if (this.CurrentKeyAllow(keyCode))
                    {
                        this.AssignKeyText(keyCode);
                        this.AssignNewKey(keyCode);
                        this.OnDeselect();
                    }
                    else if (keyCode != KeyCode.Mouse0)
                    {
                        base.GetComponent<Animator>().SetTrigger("wrongKeyPressed");
                        this.SetText();
                    }
                }
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        private bool wrongKeyState
        {
            get => 
                this._wrongKeyState;
            set
            {
                this._wrongKeyState = value;
                base.GetComponent<Animator>().SetBool("wrongKeyState", value);
            }
        }
    }
}

