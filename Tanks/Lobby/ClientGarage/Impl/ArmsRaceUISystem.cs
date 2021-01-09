namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientEntrance.API;

    public class ArmsRaceUISystem : ECSSystem
    {
        [OnEventFire]
        public void HideArmsRaceIntro(ButtonClickEvent e, SingleNode<ArmsRaceIntroCloseButtonCompoent> button, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            dialogs.component.Get<ArmsRaceIntroDialog>().Hide();
        }

        [OnEventFire]
        public void ShowArmsRaceIntro(ShowArmsRaceIntroEvent e, Node any, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] UserNode user)
        {
            ArmsRaceIntroDialog dialog = dialogs.component.Get<ArmsRaceIntroDialog>();
            dialog.screen1.SetActive(true);
            dialog.screen2.SetActive(false);
            if (user.userExperience.Experience <= 800L)
            {
                dialog.container.SetActive(false);
            }
            e.Dialog = dialog;
            dialog.Show(new List<Animator>());
        }

        public class UserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserExperienceComponent userExperience;
        }
    }
}

