namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientPayment.Impl;
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientEntrance.Impl;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientPayment.API;
    using Tanks.Lobby.ClientPayment.Impl;

    public class TutorialBuildSystem : ECSSystem
    {
        [OnEventFire]
        public void AddTutorials(NodeAddedEvent e, SingleNode<TutorialConfigurationComponent> tutorialData, SingleNode<SelfUserComponent> selfUser)
        {
            if ((!Environment.CommandLine.Contains("disableTutorials") && (tutorialData.component.Tutorials != null)) && !selfUser.Entity.HasComponent<SkipAllTutorialsComponent>())
            {
                foreach (string str in tutorialData.component.Tutorials)
                {
                    base.CreateEntity<TutorialDataTemplate>(str);
                }
            }
        }

        [OnEventFire]
        public void CheckForSkipTutorials(CheckForSkipTutorial e, Node any, [JoinAll] SelfUser selfUSer)
        {
            e.canSkipTutorial = false;
        }

        [OnEventFire]
        public void CompleteTutorial(NodeAddedEvent e, TankRentOfferNode rentOffer, [JoinBy(typeof(SpecialOfferGroupComponent))] SingleNode<PersonalSpecialOfferPropertiesComponent> personalOffer, [Context, Combine] TutorialNode tutorial)
        {
            if (!tutorial.Entity.HasComponent<TutorialCompleteComponent>())
            {
                tutorial.Entity.AddComponent<TutorialCompleteComponent>();
            }
        }

        [OnEventFire]
        public void GetTutorialStepIndex(TutorialStepIndexEvent e, VisualTutorialStep tutorialStep, [JoinAll] ICollection<VisualTutorialStep> allSteps, [JoinAll] ICollection<CompletedVisualTutorialStep> allCompletedSteps)
        {
            e.CurrentStepNumber = allCompletedSteps.Count + 1;
            e.StepCountInTutorial = allSteps.Count;
        }

        [OnEventFire]
        public void TutorialAdded(NodeAddedEvent e, TutorialCompleteIdsNode userSteps, [Combine] TutorialNode tutorial)
        {
            bool flag = userSteps.registrationDate.RegistrationDate.UnityTime != 0f;
            bool flag2 = this.TutorialCompleted(tutorial.tutorialData.TutorialId, userSteps.tutorialCompleteIds.CompletedIds);
            bool flag3 = (!flag || !tutorial.tutorialData.ForNewPlayer) ? (!flag && tutorial.tutorialData.ForOldPlayer) : true;
            if (flag2 || !flag3)
            {
                if (!tutorial.Entity.HasComponent<TutorialCompleteComponent>())
                {
                    tutorial.Entity.AddComponent<TutorialCompleteComponent>();
                }
                if (!flag3)
                {
                    return;
                }
            }
            foreach (string str in tutorial.tutorialData.Steps.Keys)
            {
                Entity entity = base.CreateEntity(tutorial.tutorialData.Steps[str], tutorial.tutorialData.StepsPath + str);
                if (entity.GetComponent<TutorialStepDataComponent>().VisualStep)
                {
                    entity.AddComponent<TutorialVisualStepComponent>();
                }
                if (flag2)
                {
                    entity.AddComponent<TutorialStepCompleteComponent>();
                }
                tutorial.tutorialGroup.Attach(entity);
            }
        }

        public bool TutorialCompleted(long tutorialId, List<long> ids)
        {
            bool flag;
            using (List<long>.Enumerator enumerator = ids.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        long current = enumerator.Current;
                        if (tutorialId != current)
                        {
                            continue;
                        }
                        flag = true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                }
            }
            return flag;
        }

        public class CompletedVisualTutorialStep : TutorialBuildSystem.TutorialStep
        {
            public TutorialVisualStepComponent tutorialVisualStep;
            public TutorialStepCompleteComponent tutorialStepComplete;
        }

        public class SelfUser : Node
        {
            public SelfUserComponent selfUser;
            public UserComponent user;
            public UserGroupComponent userGroup;
        }

        public class TankRentOfferNode : Node
        {
            public SpecialOfferComponent specialOffer;
            public SpecialOfferGroupComponent specialOfferGroup;
            public LegendaryTankSpecialOfferComponent legendaryTankSpecialOffer;
            public GoodsPriceComponent goodsPrice;
        }

        public class TutorialCompletedNode : TutorialBuildSystem.TutorialNode
        {
            public TutorialCompleteComponent tutorialComplete;
        }

        public class TutorialCompleteIdsNode : Node
        {
            public SelfUserComponent selfUser;
            public TutorialCompleteIdsComponent tutorialCompleteIds;
            public RegistrationDateComponent registrationDate;
        }

        public class TutorialNode : Node
        {
            public TutorialDataComponent tutorialData;
            public TutorialGroupComponent tutorialGroup;
        }

        public class TutorialStep : Node
        {
            public TutorialStepDataComponent tutorialStepData;
            public TutorialGroupComponent tutorialGroup;
        }

        public class UserSkipALlTutorialsNode : Node
        {
            public SelfUserComponent selfUser;
            public SkipAllTutorialsComponent skipAllTutorials;
        }

        public class VisualTutorialStep : TutorialBuildSystem.TutorialStep
        {
            public TutorialVisualStepComponent tutorialVisualStep;
        }
    }
}

