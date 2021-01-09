namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class CheckboxComponent : EventMappingComponent
    {
        [SerializeField]
        private Text label;
        [SerializeField]
        private TextMeshProUGUI TMPLabel;
        [SerializeField]
        private Toggle toggle;
        private bool defaultValue;

        protected override void Awake()
        {
            base.Awake();
            this.defaultValue = this.toggle.isOn;
        }

        private void OnEnable()
        {
            this.toggle.isOn = this.defaultValue;
        }

        protected override void Subscribe()
        {
            this.toggle.onValueChanged.AddListener(delegate (bool isOn) {
                if (isOn)
                {
                    this.SendEvent<CheckedCheckboxEvent>();
                }
                else
                {
                    this.SendEvent<UncheckedCheckboxEvent>();
                }
            });
        }

        public virtual string LabelText
        {
            get => 
                (this.label == null) ? this.TMPLabel.text : this.label.text;
            set
            {
                if (this.label != null)
                {
                    this.label.text = value;
                }
                else
                {
                    this.TMPLabel.text = value;
                }
            }
        }

        public virtual bool IsChecked
        {
            get => 
                this.toggle.isOn;
            set => 
                this.toggle.isOn = value;
        }
    }
}

