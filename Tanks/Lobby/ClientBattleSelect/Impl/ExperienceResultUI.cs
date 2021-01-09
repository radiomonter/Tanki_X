namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientProfile.API;

    public class ExperienceResultUI : ProgressResultUI
    {
        public void SetNewLevel()
        {
            base.SetResidualProgress();
        }

        public void SetProgress(float expReward, int[] levels, BattleResultsTextTemplatesComponent textTemplates)
        {
            LevelInfo currentLevelInfo = this.SendEvent<GetUserLevelInfoEvent>(SelfUserComponent.SelfUser).Info;
            base.SetProgress(expReward, levels, currentLevelInfo, textTemplates);
        }
    }
}

