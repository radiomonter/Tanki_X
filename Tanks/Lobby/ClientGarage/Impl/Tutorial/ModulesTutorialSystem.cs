namespace Tanks.Lobby.ClientGarage.Impl.Tutorial
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class ModulesTutorialSystem : ECSSystem
    {
        public static bool tutorialActive;

        [OnEventFire]
        public void StepComplete(NodeAddedEvent e, TutorialStepNode stepNode, [JoinByTutorial] TutorialNode tutorialNode, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            if (tutorialActive && (tutorialNode.tutorialData.TutorialId.Equals((long) (-1229949270L)) && stepNode.tutorialStepData.StepId.Equals((long) 0x5aeb2e04L)))
            {
                Debug.Log("Bingo");
                dialogs.component.Get<NewModulesScreenUIComponent>().Show(TankPartModuleType.WEAPON);
            }
        }

        public class TutorialNode : Node
        {
            public TutorialDataComponent tutorialData;
            public TutorialGroupComponent tutorialGroup;
        }

        public class TutorialStepNode : Node
        {
            public TutorialStepDataComponent tutorialStepData;
            public TutorialGroupComponent tutorialGroup;
            public TutorialStepCompleteComponent tutorialStepComplete;
        }
    }
}

