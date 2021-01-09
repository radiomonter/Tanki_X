namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class GarageModulesScreenComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject modulesListItemPrefab;
        [SerializeField]
        private GameObject resourcePriceLabelPrefab;
        [SerializeField]
        private RectTransform modulesListRoot;
        [SerializeField]
        private Text placeholderText;
        [SerializeField]
        private Animator fadable;

        public void FadeIn()
        {
            this.fadable.SetBool("Visible", true);
        }

        public void FadeOut()
        {
            this.fadable.SetBool("Visible", false);
        }

        public GameObject ModulesListItemPrefab
        {
            get => 
                this.modulesListItemPrefab;
            set => 
                this.modulesListItemPrefab = value;
        }

        public GameObject ResourcePriceLabelPrefab
        {
            get => 
                this.resourcePriceLabelPrefab;
            set => 
                this.resourcePriceLabelPrefab = value;
        }

        public Text PlaceholderText =>
            this.placeholderText;

        public RectTransform ModulesListRoot
        {
            get => 
                this.modulesListRoot;
            set => 
                this.modulesListRoot = value;
        }
    }
}

