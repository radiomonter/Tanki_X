namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class UidIndicatorComponent : UIBehaviour, Component
    {
        [SerializeField]
        private Text uidText;
        [SerializeField]
        private TextMeshProUGUI uidTMPText;
        public UserUidInited OnUserUidInited;

        private void Awake()
        {
            if (this.uidTMPText != null)
            {
                this.uidTMPText.text = string.Empty;
            }
        }

        private void OnDestroy()
        {
            this.OnUserUidInited.RemoveAllListeners();
        }

        public string Uid
        {
            get => 
                (this.uidText == null) ? this.uidTMPText.text : this.uidText.text;
            set
            {
                if (this.uidText != null)
                {
                    this.uidText.text = value;
                }
                else
                {
                    this.uidTMPText.text = value;
                }
                this.OnUserUidInited.Invoke();
            }
        }

        public UnityEngine.Color Color
        {
            get => 
                (this.uidText == null) ? this.uidTMPText.color : this.uidText.color;
            set
            {
                if (this.uidText != null)
                {
                    this.uidText.color = value;
                }
                else
                {
                    this.uidTMPText.color = value;
                }
            }
        }

        public FontStyles FontStyle
        {
            get
            {
                if (this.uidTMPText != null)
                {
                    return this.uidTMPText.fontStyle;
                }
                Debug.LogWarning("Only TextMeshProUGUI  supported!");
                return FontStyles.Normal;
            }
            set
            {
                if (this.uidTMPText != null)
                {
                    this.uidTMPText.fontStyle = value;
                }
                else
                {
                    Debug.LogWarning("Only TextMeshProUGUI  supported!");
                }
            }
        }

        [Serializable]
        public class UserUidInited : UnityEvent
        {
        }
    }
}

