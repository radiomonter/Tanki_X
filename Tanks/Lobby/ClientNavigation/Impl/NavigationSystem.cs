namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class NavigationSystem : ECSSystem
    {
        private void ActivateShowingScreen(GameObject showingScreen, GameObject hidingScreen, ShowScreenData showScreenData, CurrentScreenComponent currentScreenComponent)
        {
            currentScreenComponent.showScreenData = showScreenData;
            showingScreen.GetComponent<EntityBehaviour>().enabled = false;
            showingScreen.SetActive(true);
            showingScreen.GetComponent<EntityBehaviour>().enabled = true;
            this.PlayAnimation(showingScreen, hidingScreen, showScreenData.AnimationDirection);
        }

        [OnEventFire]
        public void ClearHistory(NodeAddedEvent evt, SingleNode<ScreenComponent> screen, SingleNode<HistoryComponent> navigation)
        {
            ScreenHistoryCleaner component = screen.component.GetComponent<ScreenHistoryCleaner>();
            if (component != null)
            {
                component.ClearHistory(navigation.component.screens);
            }
        }

        [OnEventFire]
        public void CreateDialogsGroup(NodeAddedEvent e, SingleNode<Dialogs60Component> dialogs)
        {
            dialogs.Entity.CreateGroup<ScreenGroupComponent>();
        }

        [OnEventFire]
        public void CreateGroup(NodeAddedEvent e, SingleNode<ScreenComponent> node)
        {
            node.Entity.CreateGroup<ScreenGroupComponent>();
        }

        private GameObject GetActiveScreen(Type screenType, ScreensRegistryComponent screens) => 
            this.GetScreen(screenType, screens, true);

        private GameObject GetNonActiveScreen(Type screenType, ScreensRegistryComponent screens) => 
            this.GetScreen(screenType, screens, false);

        private GameObject GetScreen(Type screenType, ScreensRegistryComponent screens, bool active)
        {
            GameObject obj3;
            using (List<GameObject>.Enumerator enumerator = screens.screens.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        GameObject current = enumerator.Current;
                        if ((current.GetComponent(screenType) == null) || (current.activeSelf != active))
                        {
                            continue;
                        }
                        obj3 = current;
                    }
                    else
                    {
                        return null;
                    }
                    break;
                }
            }
            return obj3;
        }

        [OnEventFire]
        public void GoBack(GoBackRequestEvent e, Node any, [JoinAll] NavigationNode navigationNode, [JoinAll] ScreenNode screen)
        {
            if (navigationNode.history.screens.Count > 0)
            {
                ShowScreenData showScreenData = navigationNode.history.screens.Peek();
                ScreensRegistryComponent screensRegistry = navigationNode.screensRegistry;
                GameObject nonActiveScreen = this.GetNonActiveScreen(showScreenData.ScreenType, screensRegistry);
                if (nonActiveScreen != null)
                {
                    OverrideGoBack component = nonActiveScreen.GetComponent<OverrideGoBack>();
                    if (component != null)
                    {
                        showScreenData = component.ScreenData;
                        nonActiveScreen = this.GetNonActiveScreen(showScreenData.ScreenType, screensRegistry);
                    }
                    base.ScheduleEvent<GoBackEvent>(any.Entity);
                    navigationNode.history.screens.Pop();
                    CurrentScreenComponent currentScreen = navigationNode.currentScreen;
                    if (currentScreen.showScreenData.Context != null)
                    {
                        currentScreen.showScreenData.Context.RemoveComponent<ScreenGroupComponent>();
                    }
                    currentScreen.screen.RemoveComponent<ActiveScreenComponent>();
                    currentScreen.screen.AddComponent<ScreenHidingComponent>();
                    currentScreen.showScreenData.FreeContext();
                    this.ActivateShowingScreen(nonActiveScreen, this.GetActiveScreen(currentScreen.showScreenData.ScreenType, navigationNode.screensRegistry), showScreenData, currentScreen);
                    base.NewEvent<PostGoBackEvent>().Attach(navigationNode).ScheduleDelayed(0f);
                }
            }
        }

        private void JoinContext(Entity context, Entity key)
        {
            if (context.HasComponent<ScreenGroupComponent>())
            {
                context.RemoveComponent<ScreenGroupComponent>();
            }
            context.AddComponent(new ScreenGroupComponent(key));
        }

        [OnEventFire]
        public void JoinContextToScreen(NodeAddedEvent e, GroupScreenNode screenNode, [JoinAll, Mandatory] SingleNode<CurrentScreenComponent> currentScreen)
        {
            ShowScreenData showScreenData = currentScreen.component.showScreenData;
            if ((showScreenData != null) && (showScreenData.Context != null))
            {
                this.JoinContext(showScreenData.Context, screenNode.Entity);
            }
        }

        private void PlayAnimation(GameObject showingScreen, GameObject hidingScreen, AnimationDirection animationDirection)
        {
            if (hidingScreen != null)
            {
                this.PlayHideAnimation(hidingScreen.GetComponent<Animator>(), animationDirection);
            }
            this.PlayShowAnimation(showingScreen.GetComponent<Animator>(), animationDirection);
        }

        private void PlayHideAnimation(Animator screen, AnimationDirection dir)
        {
            screen.SetInteger("type", (int) dir);
            screen.SetTrigger("hide");
        }

        private void PlayShowAnimation(Animator screen, AnimationDirection dir)
        {
            screen.SetInteger("type", (int) dir);
            screen.SetTrigger("show");
        }

        [OnEventFire]
        public void RegisterScreens(NodeAddedEvent e, SingleNode<ScreensRegistryComponent> navigationNode, [Combine, Context] SingleNode<ScreensBundleComponent> screensBundleNode, SingleNode<ScreensLayerComponent> layerNode)
        {
            foreach (ScreenComponent component in screensBundleNode.component.Screens)
            {
                base.Log.InfoFormat("RegisterScreens {0}", component.gameObject.name);
                GameObject gameObject = component.gameObject;
                if (gameObject.GetComponent<NoScaleScreen>() == null)
                {
                }
                navigationNode.component.screens.Add(gameObject);
                component.transform.SetParent((gameObject.GetComponent<NoScaleScreen>() != null) ? layerNode.component.screens60Layer : layerNode.component.screensLayer, false);
            }
            if (screensBundleNode.component.Dialogs60 != null)
            {
                screensBundleNode.component.Dialogs60.transform.SetParent(layerNode.component.dialogs60Layer, false);
            }
            Object.Destroy(screensBundleNode.component.gameObject);
            base.ScheduleEvent<ScreensLoadedEvent>(navigationNode);
        }

        [OnEventFire]
        public void RequestShowScreen(ShowScreenEvent e, Node any, [JoinAll] SingleNode<ScreensRegistryComponent> navigationNode)
        {
            base.Log.InfoFormat("RequestShowScreen {0}", e.ShowScreenData.ScreenType.Name);
            RequestShowScreenComponent component = new RequestShowScreenComponent {
                Event = e
            };
            navigationNode.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void SetCurrentScreen(NodeAddedEvent e, SingleNode<ScreenComponent> screenNode, [JoinAll] SingleNode<CurrentScreenComponent> currentScreen)
        {
            currentScreen.component.screen = screenNode.Entity;
            screenNode.Entity.AddComponent<ActiveScreenComponent>();
        }

        [OnEventFire]
        public void TryShowRequestedScreen(NodeAddedEvent e, NavigationRequestNode navigationNode)
        {
            this.TryShowScreen(navigationNode);
        }

        [OnEventComplete]
        public void TryShowRequestedScreenAfterScreensLoad(ScreensLoadedEvent e, NavigationRequestNode navigationNode)
        {
            this.TryShowScreen(navigationNode);
        }

        private void TryShowScreen(NavigationRequestNode navigationNode)
        {
            ShowScreenData showScreenData = navigationNode.requestShowScreen.Event.ShowScreenData;
            ScreensRegistryComponent screensRegistry = navigationNode.screensRegistry;
            GameObject nonActiveScreen = this.GetNonActiveScreen(showScreenData.ScreenType, screensRegistry);
            if (nonActiveScreen == null)
            {
                if (this.GetActiveScreen(showScreenData.ScreenType, screensRegistry) != null)
                {
                    navigationNode.Entity.RemoveComponent<RequestShowScreenComponent>();
                }
                else
                {
                    base.Log.WarnFormat("Skip remove RequestShowScreenComponent {0}", navigationNode.requestShowScreen.Event.ShowScreenData.ScreenType.Name);
                }
            }
            else
            {
                navigationNode.Entity.RemoveComponent<RequestShowScreenComponent>();
                CurrentScreenComponent currentScreen = navigationNode.currentScreen;
                HistoryComponent history = navigationNode.history;
                GameObject hidingScreen = null;
                if (currentScreen.screen != null)
                {
                    hidingScreen = this.GetActiveScreen(currentScreen.showScreenData.ScreenType, navigationNode.screensRegistry);
                    currentScreen.screen.RemoveComponent<ActiveScreenComponent>();
                    currentScreen.screen.AddComponent<ScreenHidingComponent>();
                    if (hidingScreen.GetComponent<ScreenComponent>().LogInHistory)
                    {
                        ShowScreenData t = currentScreen.showScreenData.InvertAnimationDirection(showScreenData.AnimationDirection);
                        Stack<ShowScreenData> screens = history.screens;
                        if ((screens.Count > 0) && ReferenceEquals(screens.Peek().ScreenType, t.ScreenType))
                        {
                            screens.Pop();
                        }
                        screens.Push(t);
                        if (t.Context != null)
                        {
                            t.Context.RemoveComponent<ScreenGroupComponent>();
                        }
                    }
                }
                this.ActivateShowingScreen(nonActiveScreen, hidingScreen, showScreenData, currentScreen);
            }
        }

        public class GroupScreenNode : Node
        {
            public ScreenComponent screen;
            public ScreenGroupComponent screenGroup;
        }

        public class NavigationNode : Node
        {
            public CurrentScreenComponent currentScreen;
            public HistoryComponent history;
            public ScreensRegistryComponent screensRegistry;
        }

        public class NavigationRequestNode : Node
        {
            public ScreensRegistryComponent screensRegistry;
            public CurrentScreenComponent currentScreen;
            public HistoryComponent history;
            public RequestShowScreenComponent requestShowScreen;
        }

        [Not(typeof(LockedScreenComponent))]
        public class ScreenNode : Node
        {
            public ActiveScreenComponent activeScreen;
        }
    }
}

