namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class ItemAttributesComponent : BehaviourComponent, AttachToEntityListener
    {
        [SerializeField]
        private GameObject proficiency;
        [SerializeField]
        private GameObject experience;
        [SerializeField]
        private GameObject upgrade;
        [SerializeField]
        private Text upgradeLevelValue;
        [SerializeField]
        private Text nextUpgradeLevelValue;
        [SerializeField]
        private ProgressBar upgradeLevelProgress;
        [SerializeField]
        private Text proficiencyLevelValue;
        [SerializeField]
        private ProgressBar proficiencyLevelProgress;
        [SerializeField]
        private Text experienceValue;
        [SerializeField]
        private Text maxExperienceValue;
        [SerializeField]
        private ProgressBar remainingExperienceProgress;
        [SerializeField]
        private GameObject nextUpgrade;
        [SerializeField]
        private Image upgradeGlow;
        [SerializeField]
        private Image colorIcon;
        private Color upgradeColor;
        [SerializeField]
        private bool showNextUpgradeValue;
        private bool runtimeShowNextUpgradeValue;
        private long upgradeLevel;
        public Color nextValueOverUpgradeColor;
        public Color nextValueUpgradeColor;

        public void AnimateUpgrade(long level)
        {
            base.GetComponent<Animator>().SetTrigger("Upgrade");
            this.upgradeLevel = level;
            this.UpdateUpgradeColor();
            RectTransform component = this.upgradeGlow.GetComponent<RectTransform>();
            component.anchorMax = new Vector2(this.upgradeLevelProgress.ProgressValue, component.anchorMax.y);
            component.anchorMin = new Vector2(this.upgradeLevelProgress.ProgressValue, component.anchorMin.y);
        }

        private void ApplyUpdateColor()
        {
            this.nextUpgradeLevelValue.color = this.upgradeColor;
            this.upgradeGlow.color = this.upgradeColor;
            this.colorIcon.color = this.upgradeColor;
        }

        private void ApplyValues()
        {
            this.upgradeLevelValue.text = this.upgradeLevel.ToString();
            this.ShowNextUpgradeValue = this.ShowNextUpgradeValue && (this.upgradeLevel < UpgradablePropertiesUtils.MAX_LEVEL);
            this.nextUpgradeLevelValue.text = (this.upgradeLevel + 1L).ToString();
            this.upgradeLevelProgress.ProgressValue = ((float) this.upgradeLevel) / ((float) UpgradablePropertiesUtils.MAX_LEVEL);
            this.nextUpgradeLevelValue.color = this.upgradeColor;
        }

        public void AttachedToEntity(Entity entity)
        {
            this.runtimeShowNextUpgradeValue = true;
            this.upgradeLevelValue.text = string.Empty;
            this.proficiencyLevelValue.text = string.Empty;
            this.upgradeLevel = 0L;
            this.remainingExperienceProgress.ProgressValue = 1f;
        }

        private unsafe void Awake()
        {
            this.nextUpgrade.SetActive(this.showNextUpgradeValue);
            RectTransform component = this.upgradeGlow.GetComponent<RectTransform>();
            Vector2 sizeDelta = component.sizeDelta;
            Vector2* vectorPtr1 = &sizeDelta;
            vectorPtr1->x *= 100f / ((float) UpgradablePropertiesUtils.MAX_LEVEL);
            component.sizeDelta = sizeDelta;
        }

        private void ChangeChildrenVisibility(bool visible)
        {
            this.proficiency.SetActive(visible);
            this.experience.SetActive((visible && (this.remainingExperienceProgress.ProgressValue < 1f)) && (this.proficiencyLevelProgress.ProgressValue < 1f));
            this.upgrade.SetActive(visible);
        }

        public void Hide()
        {
            this.ChangeChildrenVisibility(false);
        }

        public void HideExperience()
        {
            this.experience.SetActive(false);
        }

        private void OnFinishAnimation()
        {
            this.ApplyUpdateColor();
        }

        public void SetExperience(int exp, int initLevelExp, int finalLevelExp)
        {
            this.experienceValue.text = ((finalLevelExp - exp) - initLevelExp).ToString();
            this.maxExperienceValue.text = (finalLevelExp - initLevelExp).ToString();
            this.remainingExperienceProgress.ProgressValue = ((float) ((finalLevelExp - exp) - initLevelExp)) / ((float) (finalLevelExp - initLevelExp));
        }

        public void SetProficiencyLevel(int level)
        {
            this.proficiencyLevelValue.text = level.ToString();
            this.proficiencyLevelProgress.ProgressValue = ((float) level) / ((float) UpgradablePropertiesUtils.MAX_LEVEL);
            this.UpdateUpgradeColor();
            this.ApplyUpdateColor();
        }

        public void SetUpgradeLevel(long level)
        {
            this.upgradeLevel = level;
            this.UpdateUpgradeColor();
            this.ApplyValues();
            this.ApplyUpdateColor();
        }

        public void Show()
        {
            this.ChangeChildrenVisibility(true);
        }

        private void UpdateUpgradeColor()
        {
            if (this.showNextUpgradeValue && ((this.upgradeLevel >= 0L) && !string.IsNullOrEmpty(this.proficiencyLevelValue.text)))
            {
                this.upgradeColor = this.nextValueUpgradeColor;
            }
        }

        public bool ShowNextUpgradeValue
        {
            get => 
                this.showNextUpgradeValue && this.runtimeShowNextUpgradeValue;
            set
            {
                this.runtimeShowNextUpgradeValue = value;
                this.nextUpgrade.SetActive(this.showNextUpgradeValue && this.runtimeShowNextUpgradeValue);
            }
        }
    }
}

