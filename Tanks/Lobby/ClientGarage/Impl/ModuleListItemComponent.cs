namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class ModuleListItemComponent : UIBehaviour, Component
    {
        [SerializeField]
        private GameObject moduleEffectsInfoPrefab;
        [SerializeField]
        private RectTransform moduleIconRoot;
        [SerializeField]
        private RectTransform moduleNameRoot;
        [SerializeField]
        private TextMeshProUGUI craftableLabelText;
        [SerializeField]
        private RectTransform moduleEffectsInfoRoot;
        [SerializeField]
        private TextMeshProUGUI moduleNameText;
        [SerializeField]
        private PaletteColorField exceptionalColor;
        [SerializeField]
        private PaletteColorField epicColor;
        [SerializeField]
        private Image moduleIcon;
        [SerializeField]
        private TextMeshProUGUI moduleText;
        [SerializeField]
        private Color craftableTextColor;
        [SerializeField]
        private Color notCraftableTextColor;
        [SerializeField]
        private GameObject mountedSelection;

        public GameObject ModuleEffectsInfoPrefab =>
            this.moduleEffectsInfoPrefab;

        public RectTransform ModuleEffectsInfoRoot =>
            this.moduleEffectsInfoRoot;

        public string IconUid
        {
            set
            {
                this.moduleIconRoot.gameObject.SetActive(true);
                this.moduleIconRoot.GetComponent<ImageSkin>().SpriteUid = value;
            }
        }

        public string Name
        {
            set
            {
                this.moduleNameRoot.gameObject.SetActive(true);
                this.moduleNameText.text = value;
            }
        }

        public string CraftableText
        {
            set
            {
                this.craftableLabelText.gameObject.SetActive(true);
                this.craftableLabelText.text = value;
            }
        }

        public Color TextColor
        {
            set => 
                this.craftableLabelText.color = value;
        }

        public Color CraftableTextColor =>
            this.craftableTextColor;

        public Color NotCraftableTextColor =>
            this.notCraftableTextColor;

        public Color ExceptionalColor =>
            (Color) this.exceptionalColor;

        public Color EpicColor =>
            (Color) this.epicColor;

        public Image ModuleIcon =>
            this.moduleIcon;

        public Graphic ModuleText =>
            this.moduleText;

        public bool MountedSelectionActivity
        {
            get => 
                this.mountedSelection.activeSelf;
            set => 
                this.mountedSelection.SetActive(value);
        }
    }
}

