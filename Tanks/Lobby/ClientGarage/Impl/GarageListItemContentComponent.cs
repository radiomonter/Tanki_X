namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class GarageListItemContentComponent : LocalizedControl, Component
    {
        [SerializeField]
        private Text header;
        [SerializeField]
        private Text count;
        [SerializeField]
        private Text proficiencyLevel;
        [SerializeField]
        private Tanks.Lobby.ClientControls.API.ProgressBar progressBar;
        [SerializeField]
        private GameObject priceGameObject;
        [SerializeField]
        private GameObject xPriceGameObject;
        [SerializeField]
        private GameObject upgradeGameObject;
        [SerializeField]
        private GameObject arrow;
        [SerializeField]
        private Graphic unlockGraphic;
        [SerializeField]
        private GameObject previewContainer;
        [SerializeField]
        private GameObject previewPrefab;
        [SerializeField]
        private Text rareText;
        [SerializeField]
        private GameObject saleLabel;
        [SerializeField]
        private Text saleLabelText;

        public GarageListItemContentPreviewComponent AddPreview(string spriteUid)
        {
            GameObject obj2 = Instantiate<GameObject>(this.previewPrefab);
            obj2.transform.SetParent(this.previewContainer.transform, false);
            GarageListItemContentPreviewComponent component = obj2.GetComponent<GarageListItemContentPreviewComponent>();
            component.SetImage(spriteUid);
            return component;
        }

        public void AddPreview(string spriteUid, long count)
        {
            this.AddPreview(spriteUid).Count = count;
        }

        public void SetUpgradeColor(Color color)
        {
            this.unlockGraphic.color = color;
        }

        private void Unlock()
        {
            base.GetComponent<Animator>().SetTrigger("Unlock");
        }

        public Text Header =>
            this.header;

        public Text Count =>
            this.count;

        public Text UpgradeLevel =>
            this.proficiencyLevel;

        public GameObject PriceGameObject =>
            this.priceGameObject;

        public GameObject XPriceGameObject =>
            this.xPriceGameObject;

        public GameObject UpgradeGameObject =>
            this.upgradeGameObject;

        public Tanks.Lobby.ClientControls.API.ProgressBar ProgressBar =>
            this.progressBar;

        public GameObject Arrow =>
            this.arrow;

        public bool RareTextVisibility
        {
            set => 
                this.rareText.gameObject.SetActive(value);
        }

        public string RareText
        {
            set => 
                this.rareText.text = value;
        }

        public bool SaleLabelVisible
        {
            set => 
                this.saleLabel.SetActive(value);
        }

        public string SaleLabelText
        {
            set => 
                this.saleLabelText.text = value;
        }
    }
}

