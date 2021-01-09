namespace Tanks.Battle.ClientBattleSelect.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class NextLeagueTooltipShowComponent : TooltipShowBehaviour
    {
        [SerializeField]
        private LocalizedField unfairMatchText;
        private NextLeagueTooltipData leagueData;

        public void SetNextLeagueTooltipData(double points, string icon, string name, int delta, bool unfairMM)
        {
            string str = !unfairMM ? string.Empty : this.unfairMatchText.Value;
            this.leagueData = new NextLeagueTooltipData(points, icon, name, delta, str);
        }

        public override void ShowTooltip(Vector3 mousePosition)
        {
            CheckForTutorialEvent eventInstance = new CheckForTutorialEvent();
            TooltipShowBehaviour.EngineService.Engine.ScheduleEvent(eventInstance, TooltipShowBehaviour.EngineService.EntityStub);
            if (!eventInstance.TutorialIsActive)
            {
                base.tooltipShowed = true;
                if (this.IsMaxLeague)
                {
                    TooltipController.Instance.ShowTooltip(mousePosition, base.tipText, null, true);
                }
                else
                {
                    TooltipController.Instance.ShowTooltip(mousePosition, this.leagueData, base.customPrefab, true);
                }
            }
        }

        public bool IsMaxLeague { get; set; }
    }
}

