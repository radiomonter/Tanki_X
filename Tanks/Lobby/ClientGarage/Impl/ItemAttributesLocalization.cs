namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class ItemAttributesLocalization : LocalizedControl
    {
        [SerializeField]
        private Text upgradeLevelText;
        [SerializeField]
        private Text experienceToLevelUpText;

        public override string YamlKey =>
            "upgradeInfoText";

        public override string ConfigPath =>
            "ui/screen/garageitempropertyscreen";

        public virtual string UpgradeLevelText
        {
            get => 
                this.upgradeLevelText.text;
            set => 
                this.upgradeLevelText.text = value;
        }

        public virtual string ExperienceToLevelUpText
        {
            get => 
                this.experienceToLevelUpText.text;
            set => 
                this.experienceToLevelUpText.text = value;
        }
    }
}

