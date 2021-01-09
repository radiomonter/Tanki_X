namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ModulePropertyView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI propertyName;
        [SerializeField]
        private TextMeshProUGUI currentParam;
        [SerializeField]
        private TextMeshProUGUI nextParam;
        [SerializeField]
        private ImageSkin icon;
        [SerializeField]
        private GameObject Progress;
        [SerializeField]
        private Image currentProgress;
        [SerializeField]
        private Image nextProgress;
        private float current;
        private float next;
        private string units;
        private string format;
        public GameObject FillNext;
        public GameObject NextString;

        public float CurentProgressBar
        {
            set => 
                this.current = value;
        }

        public float nextProgressBar
        {
            set => 
                this.next = value;
        }

        public string Units
        {
            set => 
                this.units = value;
        }

        public string PropertyName
        {
            set => 
                this.propertyName.text = value;
        }

        public string Format
        {
            get => 
                this.format ?? "{0:0}";
            set => 
                this.format = !string.IsNullOrEmpty(value) ? ("{0:" + value + "}") : "{0:0}";
        }

        public string CurrentParamString
        {
            set => 
                this.currentParam.text = string.Format(this.Format, value);
        }

        public string NextParamString
        {
            set => 
                this.nextParam.text = string.Format(this.Format, value);
        }

        public float CurrentParam
        {
            set
            {
                this.currentParam.text = string.Format(this.Format, value) + " " + this.units;
                this.current = value;
            }
        }

        public float NextParam
        {
            set
            {
                this.nextParam.text = string.Format(this.Format, value) + " " + this.units;
                if (this.nextParam.text == this.currentParam.text)
                {
                    this.nextParam.text = string.Format(this.Format, " ");
                }
                this.next = value;
            }
        }

        public bool ProgressBar
        {
            set => 
                this.Progress.SetActive(value);
        }

        public string SpriteUid
        {
            set => 
                this.icon.SpriteUid = value;
        }

        public float MaxParam
        {
            set
            {
                this.currentProgress.fillAmount = this.current / value;
                this.nextProgress.fillAmount = this.next / value;
            }
        }
    }
}

