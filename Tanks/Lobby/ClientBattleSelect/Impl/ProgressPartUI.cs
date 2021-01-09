namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class ProgressPartUI : MonoBehaviour
    {
        [SerializeField]
        private ExperienceResultUI experienceResult;
        [SerializeField]
        private EquipmentResultUI turretResult;
        [SerializeField]
        private EquipmentResultUI hullResult;
        [SerializeField]
        private GameObject progressResult;
        [SerializeField]
        private GameObject energyResult;
        [SerializeField]
        private GameObject leagueResult;
        [SerializeField]
        private GameObject containerResult;

        private void OnDisable()
        {
            this.progressResult.SetActive(false);
            this.energyResult.SetActive(false);
            this.leagueResult.SetActive(false);
            this.containerResult.SetActive(false);
            this.progressResult.GetComponent<CanvasGroup>().alpha = 0f;
            this.energyResult.GetComponent<CanvasGroup>().alpha = 0f;
            this.leagueResult.GetComponent<CanvasGroup>().alpha = 0f;
            this.containerResult.GetComponent<CanvasGroup>().alpha = 0f;
        }

        public void SetExperienceResult(float expReward, int[] levels, BattleResultsTextTemplatesComponent textTemplates)
        {
            this.experienceResult.SetProgress(expReward, levels, textTemplates);
        }

        public void SetHullResult(Entity hull, float expReward, int previousUpgradeLevel, int[] levels, BattleResultsTextTemplatesComponent textTemplates)
        {
            this.hullResult.SetProgress(hull, expReward, previousUpgradeLevel, levels, textTemplates);
        }

        public void SetTurretResult(Entity turret, float expReward, int previousUpgradeLevel, int[] levels, BattleResultsTextTemplatesComponent textTemplates)
        {
            this.turretResult.SetProgress(turret, expReward, previousUpgradeLevel, levels, textTemplates);
        }

        public void ShowExperienceResult()
        {
            this.experienceResult.SetNewProgress();
            this.turretResult.SetNewProgress();
            this.hullResult.SetNewProgress();
        }
    }
}

