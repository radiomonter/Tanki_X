namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientProfile.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class UserExperienceIndicatorSystem : ECSSystem
    {
        [Mandatory, OnEventFire]
        public void GetUserLevelInfo(GetUserLevelInfoEvent e, UserNode user, [JoinAll] SingleNode<RanksExperiencesConfigComponent> ranksExperiencesConfig)
        {
            e.Info = LevelInfo.Get(user.userExperience.Experience, ranksExperiencesConfig.component.RanksExperiences);
        }

        [OnEventFire]
        public void SetCurrentAndNextRankExperience(NodeAddedEvent e, CurrentAndNextRankExperienceNode currentAndNextRankExperience, [Context, JoinByUser] UserNode user, [JoinAll] SingleNode<RanksExperiencesConfigComponent> ranksExperiencesConfig)
        {
            int[] ranksExperiences = ranksExperiencesConfig.component.RanksExperiences;
            Text text = currentAndNextRankExperience.currentAndNextRankExperience.Text;
            if (user.userRank.Rank.Equals((int) (ranksExperiences.Length + 1)))
            {
                text.text = user.userExperience.Experience.ToStringSeparatedByThousands();
            }
            else
            {
                int num3 = ranksExperiences[user.userRank.Rank];
                text.text = user.userExperience.Experience.ToStringSeparatedByThousands() + "/" + num3.ToStringSeparatedByThousands();
            }
        }

        [OnEventFire]
        public void SetUserExperienceProgress(NodeAddedEvent e, UserExperienceProgressBarNode userExperienceProgressBar, [Context, JoinByUser] UserNode user, [JoinAll] SingleNode<RanksExperiencesConfigComponent> ranksExperiencesConfig)
        {
            int[] ranksExperiences = ranksExperiencesConfig.component.RanksExperiences;
            if (user.userRank.Rank.Equals((int) (ranksExperiences.Length + 1)))
            {
                userExperienceProgressBar.userExperienceProgressBar.SetProgress(1f);
            }
            else
            {
                int num3 = ranksExperiences[user.userRank.Rank];
                float num4 = 0f;
                if (user.userRank.Rank.Equals(1))
                {
                    num4 = ((float) user.userExperience.Experience) / ((float) num3);
                }
                else
                {
                    int num6 = ranksExperiences[user.userRank.Rank - 1];
                    num4 = ((float) (user.userExperience.Experience - num6)) / ((float) (num3 - num6));
                }
                num4 = Mathf.Clamp01(num4);
                userExperienceProgressBar.userExperienceProgressBar.SetProgress(num4);
            }
        }

        public class CurrentAndNextRankExperienceNode : Node
        {
            public CurrentAndNextRankExperienceComponent currentAndNextRankExperience;
            public UserGroupComponent userGroup;
        }

        public class UserExperienceProgressBarNode : Node
        {
            public UserExperienceProgressBarComponent userExperienceProgressBar;
            public UserGroupComponent userGroup;
        }

        public class UserNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
            public UserRankComponent userRank;
            public UserExperienceComponent userExperience;
        }
    }
}

