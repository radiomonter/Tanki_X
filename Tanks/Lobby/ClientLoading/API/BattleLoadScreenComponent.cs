namespace Tanks.Lobby.ClientLoading.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class BattleLoadScreenComponent : MonoBehaviour, Component, NoScaleScreen
    {
        public static string MAP_FLAVOR_ID_PREFS_KEY = "MAP_FLAVOR_ID_PREFS_KEY";
        public Image mapPreview;
        public TextMeshProUGUI mapName;
        public TextMeshProUGUI battleInfo;
        public ResourcesLoadProgressBarComponent progressBar;
        public TextMeshProUGUI flavorText;
        public TextMeshProUGUI initialization;
        public LocalizedField arcadeBattleText;
        public LocalizedField energyBattleText;
        public LocalizedField ratingBattleText;
        public LoadingStatusView loadingStatusView;
        private Entity battle;
        private Map map;
        private bool needUpdate;

        private void Awake()
        {
            this.mapName.text = string.Empty;
            this.battleInfo.text = string.Empty;
            this.flavorText.text = string.Empty;
            base.GetComponent<LoadBundlesTaskProviderComponent>().OnDataChange = new Action<LoadBundlesTaskComponent>(this.UpdateView);
        }

        private static string GetFlavorText(Map map)
        {
            if (map.FlavorTextList.Count <= 0)
            {
                return string.Empty;
            }
            int num = 0;
            string key = MAP_FLAVOR_ID_PREFS_KEY + map.Name;
            if (PlayerPrefs.HasKey(key))
            {
                int @int = PlayerPrefs.GetInt(key);
                num = ((@int + 1) < map.FlavorTextList.Count) ? (@int + 1) : 0;
            }
            PlayerPrefs.SetInt(key, num);
            PlayerPrefs.Save();
            return map.FlavorTextList[num];
        }

        private string GetTypeText() => 
            !this.battle.HasComponent<ArcadeBattleComponent>() ? (!this.battle.HasComponent<EnergyBattleComponent>() ? this.ratingBattleText.Value : this.energyBattleText.Value) : this.arcadeBattleText.Value;

        public void InitView(Entity battle, Map map)
        {
            this.needUpdate = true;
            this.mapName.text = map.Name.ToUpper();
            this.flavorText.text = GetFlavorText(map);
            this.battle = battle;
            this.map = map;
            this.Update();
        }

        private void Update()
        {
            if (this.needUpdate)
            {
                this.needUpdate = !this.UpdateBattleInfo();
                this.needUpdate &= !this.UpdateMapPreview();
            }
        }

        private bool UpdateBattleInfo()
        {
            if (!this.battle.HasComponent<EnergyBattleComponent>() && (!this.battle.HasComponent<ArcadeBattleComponent>() && !this.battle.HasComponent<RatingBattleComponent>()))
            {
                return false;
            }
            BattleMode battleMode = this.battle.GetComponent<BattleModeComponent>().BattleMode;
            object[] objArray1 = new object[] { this.GetTypeText(), " (", battleMode, ")" };
            this.battleInfo.text = string.Concat(objArray1);
            return true;
        }

        private bool UpdateMapPreview()
        {
            if (this.map.LoadPreview == null)
            {
                this.mapPreview.gameObject.SetActive(false);
                return false;
            }
            this.mapPreview.gameObject.SetActive(true);
            this.mapPreview.sprite = this.map.LoadPreview;
            return true;
        }

        private void UpdateView(LoadBundlesTaskComponent loadBundlesTaskComponent)
        {
            this.progressBar.UpdateView(loadBundlesTaskComponent);
            this.initialization.gameObject.SetActive((loadBundlesTaskComponent.BytesToLoad - loadBundlesTaskComponent.BytesLoaded) <= 0x500000);
            this.loadingStatusView.UpdateView(loadBundlesTaskComponent);
        }

        public bool isReadyToHide =>
            this.progressBar.ProgressBar.ProgressValue > 0f;
    }
}

