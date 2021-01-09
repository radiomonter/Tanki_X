namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientBattleSelect.Impl;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using TMPro;
    using UnityEngine;

    public class BattleResultsAwardsScreenComponent : BehaviourComponent
    {
        public GameObject openChestButton;
        public BattleResultSpecialOfferUiComponent specialOfferUI;
        public LeagueResultUI leagueResultUI;
        public LocalizedField tutorialCongratulationLocalizedField;
        public LocalizedField crysLocalizedField;
        public ImageSkin crysImageSkin;
        [Tooltip("WIN = 0, DEFEAT = 1, DRAW = 2"), SerializeField]
        private Color[] titleColors;
        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private TextMeshProUGUI subTitle;
        [SerializeField]
        private ImageListSkin rankSkin;
        [SerializeField]
        private ImageSkin containerSkin;
        [SerializeField]
        private CircleProgressBar rankPoints;
        [SerializeField]
        private CircleProgressBar containerPoints;
        [SerializeField]
        private TextMeshProUGUI rankPointsText;
        [SerializeField]
        private TextMeshProUGUI containerPointsText;
        [SerializeField]
        private TankPartInfoComponent weaponInfo;
        [SerializeField]
        private TankPartInfoComponent hullInfo;
        [SerializeField]
        private CircleProgressBar weaponPoints;
        [SerializeField]
        private CircleProgressBar hullPoints;
        [SerializeField]
        private TextMeshProUGUI weaponPointsText;
        [SerializeField]
        private TextMeshProUGUI hullPointsText;
        [SerializeField]
        private Color deltaColor;
        [SerializeField]
        private Color multColor;
        [SerializeField]
        private GameObject containerScoreParent;
        [SerializeField]
        private TextMeshProUGUI newContainersCountText;
        [SerializeField]
        private TooltipShowBehaviour rankProgressTooltip;
        [SerializeField]
        private TooltipShowBehaviour rankNameTooltip;
        [SerializeField]
        private TooltipShowBehaviour containerTooltip;
        [SerializeField]
        private TooltipShowBehaviour hullLevelTooltip;
        [SerializeField]
        private TooltipShowBehaviour turretLevelTooltip;
        [SerializeField]
        private LocalizedField rankPointsLocalizedField;
        [SerializeField]
        private LocalizedField containerPointsLocalizedField;
        [SerializeField]
        private LocalizedField winLocalizedField;
        [SerializeField]
        private LocalizedField defeatLocalizedField;
        [SerializeField]
        private LocalizedField drawLocalizedField;
        [SerializeField]
        private LocalizedField placeLocalizedField;
        [SerializeField]
        private LocalizedField arcadeLocalizedField;
        [SerializeField]
        private LocalizedField ratingLocalizedField;
        [SerializeField]
        private LocalizedField energyLocalizedField;
        [SerializeField]
        private LocalizedField rankNameTooltipLocalizedField;
        [SerializeField]
        private LocalizedField levelLocalizedField;
        [SerializeField]
        private LocalizedField containersAmountSingularText;
        [SerializeField]
        private LocalizedField containersAmountPlural1Text;
        [SerializeField]
        private LocalizedField containersAmountPlural2Text;
        [SerializeField]
        private TooltipShowBehaviour[] scoreTooltips;
        public int CardsCount;
        private BattleTypes currentBattleType;
        private List<ProgressResultParts> currentProgressScenario;
        private readonly Dictionary<BattleTypes, List<ProgressResultParts>> progressScenarios;

        public BattleResultsAwardsScreenComponent()
        {
            Dictionary<BattleTypes, List<ProgressResultParts>> dictionary = new Dictionary<BattleTypes, List<ProgressResultParts>> {
                { 
                    BattleTypes.None,
                    new List<ProgressResultParts>()
                }
            };
            List<ProgressResultParts> list = new List<ProgressResultParts> {
                ProgressResultParts.Experience,
                ProgressResultParts.League,
                ProgressResultParts.Container
            };
            dictionary.Add(BattleTypes.Ranked, list);
            list = new List<ProgressResultParts> {
                ProgressResultParts.Experience,
                ProgressResultParts.Energy,
                ProgressResultParts.League,
                ProgressResultParts.Container
            };
            dictionary.Add(BattleTypes.RankedWithCashback, list);
            list = new List<ProgressResultParts> {
                ProgressResultParts.Experience,
                ProgressResultParts.Energy
            };
            dictionary.Add(BattleTypes.Quick, list);
            list = new List<ProgressResultParts> {
                ProgressResultParts.Experience
            };
            dictionary.Add(BattleTypes.Arcade, list);
            dictionary.Add(BattleTypes.Custom, new List<ProgressResultParts>());
            list = new List<ProgressResultParts> {
                ProgressResultParts.Experience
            };
            dictionary.Add(BattleTypes.Tutorial, list);
            this.progressScenarios = dictionary;
        }

        public bool CanShowLeagueProgress() => 
            (this.currentProgressScenario != null) && this.currentProgressScenario.Contains(ProgressResultParts.League);

        public void HideLeagueProgress()
        {
            this.containerScoreParent.SetActive(false);
            this.leagueResultUI.transform.parent.gameObject.SetActive(false);
        }

        public void HideNotiffication()
        {
            if (this.CardsCount <= 0)
            {
                base.GetComponentInParent<Animator>().SetBool("cards", false);
                this.CardsCount = 0;
            }
        }

        private string Pluralize(int amount)
        {
            switch (CasesUtil.GetCase(amount))
            {
                case CaseType.DEFAULT:
                    return string.Format(this.containersAmountPlural1Text.Value, amount);

                case CaseType.ONE:
                    return string.Format(this.containersAmountSingularText.Value, amount);

                case CaseType.TWO:
                    return string.Format(this.containersAmountPlural2Text.Value, amount);
            }
            throw new Exception("Invalid case");
        }

        private void RestartProgressBar(CircleProgressBar bar)
        {
            bar.StopAnimation();
            bar.ResetProgressView();
            bar.ClearUpgradeAnimations();
        }

        public void SetBattleType(BattleTypes battleType)
        {
            this.currentBattleType = battleType;
            this.currentProgressScenario = new List<ProgressResultParts>(this.progressScenarios[battleType]);
        }

        public void SetHullExp(int initValue, int currentValue, int maxValue, int delta, int deltaWithoutMults, int hullLevel)
        {
            this.hullLevelTooltip.TipText = $"{this.levelLocalizedField.Value}: {hullLevel}";
            this.VisualUpdateProgressBar(this.hullPoints, initValue, maxValue, currentValue, deltaWithoutMults, delta, null);
            object[] objArray1 = new object[] { "<color=#", this.deltaColor.ToHexString(), ">+", delta };
            this.hullPointsText.text = string.Concat(objArray1);
        }

        public void SetTankInfo(long hullId, long turretId, List<ModuleInfo> modules, ModuleUpgradablePowerConfigComponent moduleConfig)
        {
            this.hullInfo.Set(hullId, modules, moduleConfig);
            this.weaponInfo.Set(turretId, modules, moduleConfig);
        }

        public void SetTurretExp(int initValue, int currentValue, int maxValue, int delta, int deltaWithoutMults, int turretLevel)
        {
            this.turretLevelTooltip.TipText = $"{this.levelLocalizedField.Value}: {turretLevel}";
            this.VisualUpdateProgressBar(this.weaponPoints, initValue, maxValue, currentValue, deltaWithoutMults, delta, null);
            object[] objArray1 = new object[] { "<color=#", this.deltaColor.ToHexString(), ">+", delta };
            this.weaponPointsText.text = string.Concat(objArray1);
        }

        public void SetupHeader(BattleMode battleMode, BattleType matchMakingModeType, TeamBattleResult resultType, string mapName, int selfUserPlace)
        {
            this.title.color = this.titleColors[(int) resultType];
            if (battleMode == BattleMode.DM)
            {
                this.title.text = string.Format((string) this.placeLocalizedField, selfUserPlace);
            }
            else if (resultType == TeamBattleResult.WIN)
            {
                this.title.text = this.winLocalizedField.Value;
            }
            else if (resultType == TeamBattleResult.DEFEAT)
            {
                this.title.text = this.defeatLocalizedField.Value;
            }
            else if (resultType == TeamBattleResult.DRAW)
            {
                this.title.text = this.drawLocalizedField.Value;
            }
            string str = string.Empty;
            if (matchMakingModeType == BattleType.ARCADE)
            {
                str = this.arcadeLocalizedField.Value;
            }
            else if (matchMakingModeType == BattleType.ENERGY)
            {
                str = this.energyLocalizedField.Value;
            }
            else if (matchMakingModeType == BattleType.RATING)
            {
                str = this.ratingLocalizedField.Value;
            }
            this.subTitle.text = $"{str}({battleMode}), {mapName}";
        }

        public void ShowContainerProgress(int currentValue, int delta, int deltaWithoutMults, int maxValue, string containerSpriteUID)
        {
            this.containerTooltip.TipText = $"{currentValue}/{maxValue}";
            this.containerSkin.SpriteUid = containerSpriteUID;
            this.VisualUpdateProgressBar(this.containerPoints, 0, maxValue, currentValue, deltaWithoutMults, delta, null);
            this.VisualUpdateProgressText(this.containerPointsText, this.containerPointsLocalizedField, deltaWithoutMults, delta);
            if ((currentValue - delta) >= 0)
            {
                this.newContainersCountText.text = string.Empty;
            }
            else
            {
                int num = (delta - currentValue) / maxValue;
                this.newContainersCountText.text = "+ " + this.Pluralize(num + 1).ToLower();
            }
        }

        public void ShowLeagueProgress()
        {
            this.containerScoreParent.SetActive(true);
            this.leagueResultUI.transform.parent.gameObject.SetActive(true);
        }

        public void ShowNotiffication()
        {
            if (this.CardsCount > 0)
            {
                base.GetComponentInParent<Animator>().SetBool("cards", true);
            }
        }

        public void ShowRankProgress(int initValue, int current, int maxValue, int delta, int deltaWithoutMults, int currentRank, string[] rankNames)
        {
            <ShowRankProgress>c__AnonStorey0 storey = new <ShowRankProgress>c__AnonStorey0 {
                currentRank = currentRank,
                $this = this
            };
            this.VisualUpdateProgressBar(this.rankPoints, initValue, maxValue, current, deltaWithoutMults, delta, new Action(storey.<>m__0));
            this.VisualUpdateProgressText(this.rankPointsText, this.rankPointsLocalizedField, deltaWithoutMults, delta);
            this.rankSkin.SelectSprite(((current - delta) >= initValue) ? storey.currentRank.ToString() : (storey.currentRank - 1).ToString());
            if ((storey.currentRank + 1) < rankNames.Length)
            {
                this.rankProgressTooltip.TipText = $"{current}/{maxValue}";
                this.rankNameTooltip.TipText = string.Format(this.rankNameTooltipLocalizedField.Value, rankNames[storey.currentRank], maxValue - current, rankNames[storey.currentRank + 1]);
            }
            else
            {
                this.rankProgressTooltip.TipText = current.ToString();
                this.rankNameTooltip.TipText = rankNames[storey.currentRank];
            }
        }

        private void VisualUpdateProgressBar(CircleProgressBar bar, int startValue, int maxValue, int currentValue, int cleanDelta, int totalDelta, Action onAllAnimationsComplete = null)
        {
            if (maxValue > 0)
            {
                int num = totalDelta - cleanDelta;
                this.RestartProgressBar(bar);
                if ((currentValue - totalDelta) >= startValue)
                {
                    bar.Progress = ((float) (currentValue - totalDelta)) / ((float) maxValue);
                    bar.AdditionalProgress = ((float) cleanDelta) / ((float) maxValue);
                    bar.AdditionalProgress1 = ((float) num) / ((float) maxValue);
                }
                else
                {
                    bar.Progress = 1f;
                    int num2 = ((totalDelta - currentValue) / maxValue) - 1;
                    int num3 = 0;
                    while (true)
                    {
                        if (num3 >= num2)
                        {
                            int num4 = currentValue - startValue;
                            if (num > num4)
                            {
                                bar.AddUpgradeAnimation(0f, 0f, ((float) num4) / ((float) maxValue));
                            }
                            else
                            {
                                bar.AddUpgradeAnimation(0f, ((float) (num4 - num)) / ((float) maxValue), ((float) num) / ((float) maxValue));
                            }
                            break;
                        }
                        bar.AddUpgradeAnimation(1f, 0f, 0f);
                        num3++;
                    }
                }
                if (onAllAnimationsComplete != null)
                {
                    bar.allAnimationComplete += onAllAnimationsComplete;
                }
            }
        }

        private void VisualUpdateProgressText(TextMeshProUGUI tmpText, LocalizedField mainString, int cleanDelta, int totalDelta)
        {
            float num = (totalDelta <= 0) ? 1f : (((float) totalDelta) / ((float) cleanDelta));
            foreach (TooltipShowBehaviour behaviour in this.scoreTooltips)
            {
                behaviour.enabled = num > 1f;
            }
            if (num <= 1f)
            {
                object[] objArray2 = new object[] { mainString.Value, "<color=#", this.deltaColor.ToHexString(), "> ", totalDelta, "</color>" };
                tmpText.text = string.Concat(objArray2);
            }
            else
            {
                object[] objArray1 = new object[10];
                objArray1[0] = mainString.Value;
                objArray1[1] = " <color=#";
                objArray1[2] = this.deltaColor.ToHexString();
                objArray1[3] = "> ";
                objArray1[4] = totalDelta;
                objArray1[5] = "</color> <color=#";
                objArray1[6] = this.multColor.ToHexString();
                objArray1[7] = "> (+";
                objArray1[8] = $"{(num * 100f) - 100f:0}";
                objArray1[9] = "%)</color>";
                tmpText.text = string.Concat(objArray1);
            }
        }

        [CompilerGenerated]
        private sealed class <ShowRankProgress>c__AnonStorey0
        {
            internal int currentRank;
            internal BattleResultsAwardsScreenComponent $this;

            internal void <>m__0()
            {
                this.$this.rankSkin.SelectSprite(this.currentRank.ToString());
            }
        }

        public enum BattleTypes
        {
            None,
            Ranked,
            RankedWithCashback,
            Quick,
            Arcade,
            Custom,
            Tutorial
        }

        public enum ProgressResultParts
        {
            None,
            Experience,
            League,
            Energy,
            Container,
            Buttons
        }
    }
}

