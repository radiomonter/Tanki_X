namespace Tanks.Lobby.ClientControls.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using Tanks.Lobby.ClientControls.API;

    public class LoadGearSystem : ECSSystem
    {
        [OnEventFire]
        public void HideLoadGear(HideLoadGearEvent e, SingleNode<LoadGearComponent> loadGear)
        {
            LoadGearComponent component = loadGear.component;
            if (component.gameObject.activeInHierarchy)
            {
                component.Animator.SetTrigger("hide");
            }
        }

        [OnEventFire]
        public void HideLoadGear(HideLoadGearEvent e, ForegroundActiveLoadGearNode loadGear)
        {
            loadGear.Entity.RemoveComponent<ActiveGearComponent>();
            base.ScheduleEvent<HideScreenForegroundEvent>(loadGear);
        }

        [OnEventComplete]
        public void HideLoadGear(HideLoadGearEvent e, NotForegroundActiveLoadGearNode loadGear, [JoinAll] Optional<ForegroundActiveLoadGearNode> foregroundActiveGear)
        {
            loadGear.Entity.RemoveComponent<ActiveGearComponent>();
            if (foregroundActiveGear.IsPresent() && !foregroundActiveGear.Get().loadGear.gameObject.activeInHierarchy)
            {
                foregroundActiveGear.Get().loadGear.Animator.SetTrigger("show");
            }
        }

        [OnEventFire]
        public void ShowLoadGear(ShowLoadGearEvent e, SingleNode<LoadGearComponent> loadGear)
        {
            LoadGearComponent component = loadGear.component;
            component.GearProgressBar.gameObject.SetActive(e.ShowProgress);
            component.GearProgressBar.ProgressValue = 0f;
            component.gameObject.SetActive(true);
        }

        [OnEventFire]
        public void ShowLoadGear(ShowLoadGearEvent e, ForegroundLoadGearNode loadGear)
        {
            loadGear.Entity.AddComponent<ActiveGearComponent>();
            base.ScheduleEvent<ShowScreenForegroundEvent>(loadGear);
        }

        [OnEventComplete]
        public void ShowLoadGear(ShowLoadGearEvent e, NotForegroundLoadGearNode loadGear, [JoinAll] Optional<ForegroundActiveLoadGearNode> foregroundActiveGear)
        {
            loadGear.Entity.AddComponent<ActiveGearComponent>();
            if (foregroundActiveGear.IsPresent() && foregroundActiveGear.Get().loadGear.gameObject.activeInHierarchy)
            {
                foregroundActiveGear.Get().loadGear.Animator.SetTrigger("hide");
            }
        }

        [OnEventFire]
        public void UpdateLoadGearProgress(UpdateLoadGearProgressEvent e, ActiveLoadGearNode loadGear)
        {
            loadGear.loadGear.GearProgressBar.ProgressValue = e.Value;
        }

        public class ActiveLoadGearNode : Node
        {
            public LoadGearComponent loadGear;
            public ActiveGearComponent activeGear;
        }

        public class ForegroundActiveLoadGearNode : LoadGearSystem.ActiveLoadGearNode
        {
            public ScreenForegroundComponent screenForeground;
        }

        public class ForegroundLoadGearNode : LoadGearSystem.LoadGearNode
        {
            public ScreenForegroundComponent screenForeground;
        }

        [Not(typeof(ActiveGearComponent))]
        public class LoadGearNode : Node
        {
            public LoadGearComponent loadGear;
        }

        [Not(typeof(ScreenForegroundComponent))]
        public class NotForegroundActiveLoadGearNode : LoadGearSystem.ActiveLoadGearNode
        {
        }

        [Not(typeof(ScreenForegroundComponent))]
        public class NotForegroundLoadGearNode : LoadGearSystem.LoadGearNode
        {
        }
    }
}

