namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class MainHUDVersionSwitcher : MonoBehaviour
    {
        [SerializeField]
        private RectTransform mainInfoRect;
        [SerializeField]
        private RectTransform inventoryRect;
        [SerializeField]
        private RectTransform effectsRect;
        [SerializeField]
        private RectTransform chatRect;
        [SerializeField]
        private GameObject playerAvatar;
        [SerializeField]
        private GameObject hpBarV1;
        [SerializeField]
        private GameObject hpBarV2;
        [SerializeField]
        private GameObject energyBarV1;
        [SerializeField]
        private GameObject energyBarV2;
        [SerializeField]
        private GameObject effectsTopImage;
        [SerializeField]
        private GameObject inventoryTopImage;
        [SerializeField]
        private GameObject bottomLongLineImage;
        [SerializeField]
        private GameObject killAssistLogV1;
        [SerializeField]
        private GameObject killAssistLogV2;
        [SerializeField]
        private GameObject battleChatInput;
        private Vector2 mainInfoV1Position = new Vector2(325f, 65f);
        private Vector2 mainInfoV2Position = new Vector2(0f, 40f);
        private Vector2 inventoryV1Position = new Vector2(-230f, 65f);
        private Vector2 inventoryV2Position = new Vector2(0f, -2.5f);
        private Vector2 effectsV1Position = new Vector2(0f, -14f);
        private Vector2 effectsV2Position = new Vector2(0f, -4.6f);
        private Vector2 chatV1Position = new Vector2(80f, 270f);
        private Vector2 chatV2Position = new Vector2(40f, 100f);
        private string key = "BattleHudVersion";
        public bool specMode;

        private int GetBattleHudVersion()
        {
            if (!PlayerPrefs.HasKey(this.key))
            {
                PlayerPrefs.SetInt(this.key, 2);
            }
            return PlayerPrefs.GetInt(this.key);
        }

        public void SetCurrentHud()
        {
            this.SetHudVersion(!this.specMode ? this.GetBattleHudVersion() : 1, !this.specMode);
        }

        private void SetHudVersion(int v, bool saveToPlayerPrefs = true)
        {
            if (saveToPlayerPrefs)
            {
                PlayerPrefs.SetInt(this.key, v);
            }
            Vector2 vector = (v != 1) ? new Vector2(0.5f, 0f) : Vector2.zero;
            this.mainInfoRect.anchorMax = vector;
            this.mainInfoRect.anchorMin = vector;
            this.mainInfoRect.anchoredPosition = (v != 1) ? this.mainInfoV2Position : this.mainInfoV1Position;
            vector = (v != 1) ? new Vector2(0.5f, 0f) : new Vector2(1f, 0f);
            this.inventoryRect.anchorMax = vector;
            this.inventoryRect.anchorMin = vector;
            this.inventoryRect.anchoredPosition = (v != 1) ? this.inventoryV2Position : this.inventoryV1Position;
            this.effectsRect.anchoredPosition = (v != 1) ? this.effectsV2Position : this.effectsV1Position;
            this.chatRect.anchoredPosition = (v != 1) ? this.chatV2Position : this.chatV1Position;
            this.playerAvatar.SetActive(v == 1);
            this.hpBarV1.SetActive(v == 1);
            this.energyBarV1.SetActive(v == 1);
            this.hpBarV2.SetActive(v == 2);
            this.energyBarV2.SetActive(v == 2);
            this.bottomLongLineImage.SetActive(v == 2);
            this.effectsTopImage.SetActive(v == 1);
            this.effectsRect.rotation = (v != 1) ? Quaternion.identity : Quaternion.Euler(0f, -20f, 0f);
            this.inventoryTopImage.SetActive(v == 1);
            this.killAssistLogV1.SetActive(false);
            this.killAssistLogV2.SetActive(false);
            if (v == 1)
            {
                this.killAssistLogV1.SetActive(true);
            }
            else
            {
                this.killAssistLogV2.SetActive(true);
            }
        }

        private void SwitchHud()
        {
            if (!this.specMode)
            {
                int battleHudVersion = this.GetBattleHudVersion();
                this.SetHudVersion((battleHudVersion != 1) ? 1 : 2, true);
            }
        }

        private void Update()
        {
            if (InputManager.GetActionKeyDown(BattleActions.CHANGEHUD) && !this.battleChatInput.activeSelf)
            {
                this.SwitchHud();
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }
    }
}

