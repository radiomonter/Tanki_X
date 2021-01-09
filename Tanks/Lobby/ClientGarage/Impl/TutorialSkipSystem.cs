namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;
    using tanks.modules.lobby.ClientGarage.Impl;
    using UnityEngine.UI;

    public class TutorialSkipSystem : ECSSystem
    {
        private void CompleteActiveTutorial(Entity any)
        {
            base.ScheduleEvent<CompleteActiveTutorialEvent>(any);
        }

        [OnEventFire]
        public void CompleteResearchModuleStep(CompleteActiveTutorialEvent e, Node any, [JoinAll] SingleNode<EquipModulesTutorStepHandler> stepHandler)
        {
            stepHandler.component.OnSkipTutorial();
        }

        [OnEventFire]
        public void CompleteResearchModuleStep(CompleteActiveTutorialEvent e, Node any, [JoinAll] SingleNode<ModulesTutorialStep4Handler> stepHandler)
        {
            stepHandler.component.OnSkipTutorial();
        }

        [OnEventFire]
        public void CompleteResearchModuleStep(CompleteActiveTutorialEvent e, Node any, [JoinAll] SingleNode<ModulesTutorStep7Handler> stepHandler)
        {
            stepHandler.component.OnSkipTutorial();
        }

        [OnEventFire]
        public void CompleteResearchModuleStep(CompleteActiveTutorialEvent e, Node any, [JoinAll] SingleNode<ModulesTutorStep8Handler> stepHandler)
        {
            stepHandler.component.OnSkipTutorial();
        }

        [OnEventFire]
        public void CompleteResearchModuleStep(CompleteActiveTutorialEvent e, Node any, [JoinAll] SingleNode<SelectModuleForResearchTutorStepHandler> stepHandler)
        {
            stepHandler.component.OnSkipTutorial();
        }

        [OnEventFire]
        public void CompleteUpgradeModuleStep(CompleteActiveTutorialEvent e, Node any, [JoinAll] SingleNode<UpgradeModuleTutorStep7Handler> stepHandler)
        {
            stepHandler.component.OnSkipTutorial();
        }

        [OnEventFire]
        public void ConfirmOnDialog(DialogConfirmEvent e, SingleNode<SkipTutorialConfirmWindowComponent> skipDialog, [JoinAll] SelfUserNode selfUser, [JoinAll] ICollection<TutorialStepNode> tutorials)
        {
            this.CompleteActiveTutorial(skipDialog.Entity);
        }

        [OnEventComplete]
        public void ShowButton(NodeAddedEvent e, SingleNode<TutorialScreenComponent> active, [JoinAll] SingleNode<MainScreenComponent> mainScreen, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            TutorialCanvas instance = TutorialCanvas.Instance;
            instance.SkipTutorialButton.SetActive(true);
            foreach (Selectable selectable in instance.GetComponentInChildren<SkipTutorialConfirmWindowComponent>(true).GetComponentsInChildren<Selectable>())
            {
                instance.AddAllowSelectable(selectable);
                selectable.interactable = true;
            }
        }

        private void ShowConfirmationDialog([JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            List<Animator> animators = new List<Animator>();
            if (screens.IsPresent())
            {
                animators = screens.Get().component.Animators;
            }
            TutorialCanvas.Instance.GetComponentInChildren<SkipTutorialConfirmWindowComponent>(true).Show(animators);
        }

        [OnEventFire]
        public void SkipTutorialByButton(ButtonClickEvent e, SingleNode<SkipTutorialButtonComponent> SkipTutorial, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            this.CompleteActiveTutorial(SkipTutorial.Entity);
        }

        [OnEventFire]
        public void SkipTutorialByEsc(UpdateEvent e, SingleNode<SkipTutorialButtonComponent> SkipTutorial)
        {
            if (InputMapping.Cancel)
            {
                this.CompleteActiveTutorial(SkipTutorial.Entity);
            }
        }

        [OnEventFire]
        public void TutorialComplete(TutorialCompleteEvent e, TutorialNode tutorial, [JoinByTutorial] ICollection<TutorialStepNode> steps, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            if (!tutorial.Entity.HasComponent<TutorialCompleteComponent>())
            {
                tutorial.Entity.AddComponent<TutorialCompleteComponent>();
            }
            foreach (TutorialStepNode node in steps)
            {
                if (!node.Entity.HasComponent<TutorialStepCompleteComponent>())
                {
                    base.ScheduleEvent(new TutorialActionEvent(tutorial.tutorialData.TutorialId, node.tutorialStepData.StepId, TutorialAction.START), session);
                    base.ScheduleEvent(new TutorialActionEvent(tutorial.tutorialData.TutorialId, node.tutorialStepData.StepId, TutorialAction.END), session);
                    node.Entity.AddComponent<TutorialStepCompleteComponent>();
                }
            }
            TutorialCanvas.Instance.Hide();
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
        }

        [Not(typeof(TutorialCompleteComponent))]
        public class TutorialNode : Node
        {
            public TutorialDataComponent tutorialData;
            public TutorialGroupComponent tutorialGroup;
            public TutorialRequiredCompletedTutorialsComponent tutorialRequiredCompletedTutorials;
        }

        [Not(typeof(TutorialStepCompleteComponent))]
        public class TutorialStepNode : Node
        {
            public TutorialStepDataComponent tutorialStepData;
        }
    }
}

