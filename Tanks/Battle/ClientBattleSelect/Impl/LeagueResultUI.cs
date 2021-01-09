namespace Tanks.Battle.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientUserProfile.Impl;
    using TMPro;
    using UnityEngine;

    public class LeagueResultUI : ECSBehaviour
    {
        [SerializeField]
        private ImageSkin leagueIcon;
        [SerializeField]
        private TextMeshProUGUI leaguePointsTitle;
        [SerializeField]
        private TextMeshProUGUI leaguePointsValue;
        [SerializeField]
        private TextMeshProUGUI newLeague;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private NextLeagueTooltipShowComponent tooltip;
        [SerializeField]
        private LocalizedField leaguePointsText;
        [SerializeField]
        private LocalizedField placeText;
        [SerializeField]
        private LocalizedField youLeaguePointsText;
        [SerializeField]
        private AnimatedLong leaguePoints;
        [SerializeField]
        private Animator deltaAnimator;
        [SerializeField]
        private TextMeshProUGUI deltaText;
        private Entity previousLeague;
        private Entity currentLeague;
        private double points;
        private long place;
        private double delta;
        private bool unfairMM;
        private bool topLeague;
        private bool newLeagueUnlocked;
        private Dictionary<long, double> lastUserReputationInLeagues = new Dictionary<long, double>();

        public void DealWithReputationChange()
        {
            if (this.topLeague)
            {
                this.deltaAnimator.gameObject.SetActive(false);
            }
            else
            {
                this.SetDeltaAnimation();
                long num = (long) (this.points - this.delta);
                this.leaguePoints.SetImmediate(num);
                this.leaguePoints.Value = (long) this.points;
            }
        }

        private double GetReputationToEnter(Entity legue)
        {
            double reputationToEnter = legue.GetComponent<LeagueConfigComponent>().ReputationToEnter;
            return (!this.lastUserReputationInLeagues.ContainsKey(legue.Id) ? reputationToEnter : Math.Max(this.lastUserReputationInLeagues[legue.Id], reputationToEnter));
        }

        private void OnDisable()
        {
            this.previousLeague = null;
            this.newLeague.gameObject.SetActive(false);
        }

        public void PutReputationToEnter(long legueId, double reputation)
        {
            this.lastUserReputationInLeagues[legueId] = reputation;
            this.SetTooltip();
        }

        public void SetCurrentLeague(Entity currentLeague, double points, long place, bool topLeague, double delta, bool unfairMM)
        {
            this.currentLeague = currentLeague;
            this.points = points;
            this.place = place;
            this.topLeague = topLeague;
            this.delta = delta;
            this.unfairMM = unfairMM;
            this.SetTooltip();
            if (this.previousLeague == null)
            {
                this.SetLeagueInfo(currentLeague);
                this.newLeagueUnlocked = false;
            }
            else
            {
                int leagueIndex = currentLeague.GetComponent<LeagueConfigComponent>().LeagueIndex;
                this.newLeagueUnlocked = leagueIndex > this.previousLeague.GetComponent<LeagueConfigComponent>().LeagueIndex;
                this.SetLeagueInfo(!this.newLeagueUnlocked ? currentLeague : this.previousLeague);
            }
        }

        private void SetDeltaAnimation()
        {
            int num = ((int) this.points) - ((int) (this.points - this.delta));
            this.deltaAnimator.gameObject.SetActive(num != 0);
            this.deltaAnimator.SetTrigger((num < 0) ? "Down" : "Up");
            this.deltaText.text = num.ToString("+#;-#");
        }

        private void SetLeagueInfo(Entity league)
        {
            this.leagueIcon.SpriteUid = league.GetComponent<LeagueIconComponent>().SpriteUid;
            if (!this.topLeague)
            {
                this.leaguePointsTitle.text = this.leaguePointsText.Value;
            }
            else
            {
                this.leaguePointsTitle.text = this.placeText.Value;
                this.leaguePoints.SetImmediate(this.place);
            }
        }

        public void SetNewLeagueIcon()
        {
            this.SetLeagueInfo(this.currentLeague);
        }

        public void SetPreviousLeague(Entity previousLeague)
        {
            this.previousLeague = previousLeague;
        }

        private void SetTooltip()
        {
            this.animator.SetBool("CurrentLeagueIsMax", this.topLeague);
            this.tooltip.IsMaxLeague = this.topLeague;
            if (this.topLeague)
            {
                this.tooltip.TipText = string.Format((string) this.youLeaguePointsText, Math.Truncate(this.points));
            }
            else
            {
                GetLeagueByIndexEvent eventInstance = new GetLeagueByIndexEvent(this.currentLeague.GetComponent<LeagueConfigComponent>().LeagueIndex + 1);
                base.ScheduleEvent(eventInstance, this.currentLeague);
                Entity leagueEntity = eventInstance.leagueEntity;
                this.tooltip.SetNextLeagueTooltipData(this.GetReputationToEnter(leagueEntity) - Math.Truncate(this.points), leagueEntity.GetComponent<LeagueIconComponent>().SpriteUid, leagueEntity.GetComponent<LeagueNameComponent>().Name, (int) this.delta, this.unfairMM);
            }
        }

        public void ShowNewLeague()
        {
            if (this.newLeagueUnlocked)
            {
                this.animator.SetTrigger("NewLeagueUnlocked");
            }
        }
    }
}

