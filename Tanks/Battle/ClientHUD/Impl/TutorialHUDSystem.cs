namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class TutorialHUDSystem : ECSSystem
    {
        [OnEventFire]
        public void ShowOrHideKeymap(UpdateEvent e, TutorialKeymapNode tutorialKeymap)
        {
            if (InputManager.GetActionKeyDown(BattleActions.HELP))
            {
                tutorialKeymap.tutorialKeymap.Visible = !tutorialKeymap.tutorialKeymap.Visible;
            }
            if (Input.GetKeyDown(KeyCode.Escape) && tutorialKeymap.tutorialKeymap.Visible)
            {
                tutorialKeymap.tutorialKeymap.Visible = false;
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class TutorialKeymapNode : Node
        {
            public TutorialKeymapComponent tutorialKeymap;
        }

        public class UserNode : Node
        {
            public UserExperienceComponent userExperience;
            public SelfUserComponent selfUser;
        }
    }
}

