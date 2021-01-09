namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class MVPModuleContainer : MonoBehaviour
    {
        [SerializeField]
        private GameObject card;
        [SerializeField]
        private TextMeshProUGUI cardInfo;
        [SerializeField]
        private LocalizedField moduleLevelShortLocalizedField;

        public void SetupModuleCard(ModuleInfo m, float moduleSize)
        {
            this.card.AddComponent<EntityBehaviour>().handleAutomaticaly = false;
            this.card.GetComponent<ModuleCardView>().UpdateView(m.ModuleId, m.UpgradeLevel, false, false);
            this.cardInfo.text = $"{this.card.GetComponent<ModuleCardView>().name} ({this.moduleLevelShortLocalizedField.Value} {m.UpgradeLevel + 1L})";
            this.card.transform.localScale = new Vector3(moduleSize, moduleSize, moduleSize);
            this.card.transform.localPosition = new Vector3(0f, 0f, 20f);
        }
    }
}

