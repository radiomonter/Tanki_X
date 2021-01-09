namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientGraphics.Impl;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class HangarCameraSwitchSystem : ECSSystem
    {
        [OnEventComplete]
        public void DeleteHangarCamera(NodeRemoveEvent e, SingleNode<HangarComponent> h, [JoinSelf] HangarCameraNode hangar)
        {
            hangar.Entity.RemoveComponent<HangarCameraViewStateComponent>();
            hangar.Entity.RemoveComponent<HangarCameraStateComponent>();
            hangar.Entity.RemoveComponent<HangarCameraRotationStateComponent>();
            hangar.Entity.RemoveComponent<HangarCameraComponent>();
            hangar.Entity.RemoveComponent<CameraComponent>();
            hangar.Entity.RemoveComponent<CameraRootTransformComponent>();
        }

        [OnEventFire]
        public void DisableHangarCamera(NodeRemoveEvent e, HangarCameraEnabledNode hangar)
        {
            if (hangar.camera.Enabled)
            {
                hangar.camera.Enabled = false;
            }
        }

        [OnEventFire]
        public void DisableHangarCamera(NodeAddedEvent e, ScreenNode screen, HangarCameraEnabledNode hangar)
        {
            if (!screen.screen.ShowHangar)
            {
                hangar.hangarCameraState.Esm.ChangeState<HangarCameraState.Disabled>();
            }
        }

        [OnEventFire]
        public void DisableHangarCameraRotation(NodeAddedEvent e, ScreenNode screen, HangarCameraRotationEnabledNode hangar)
        {
            if (!screen.screen.RotateHangarCamera)
            {
                hangar.Entity.RemoveComponent<HangarCameraRotationEnabledComponent>();
                hangar.Entity.AddComponent<HangarCameraRotationDisabledComponent>();
            }
        }

        [OnEventFire]
        public void EnableHangarCamera(NodeAddedEvent e, HangarCameraEnabledNode hangar)
        {
            if (hangar.camera.Enabled)
            {
                hangar.camera.Enabled = true;
            }
        }

        [OnEventFire]
        public void EnableHangarCamera(NodeAddedEvent e, ScreenNode screen, HangarCameraDisabledNode hangar)
        {
            if (screen.screen.ShowHangar)
            {
                hangar.hangarCameraState.Esm.ChangeState<HangarCameraState.Enabled>();
            }
        }

        [OnEventFire]
        public void EnableHangarCameraRotation(NodeAddedEvent e, ScreenNode screen, HangarCameraRotationDisabledNode hangar)
        {
            if (screen.screen.RotateHangarCamera)
            {
                hangar.Entity.RemoveComponent<HangarCameraRotationDisabledComponent>();
                hangar.Entity.AddComponent<HangarCameraRotationEnabledComponent>();
            }
        }

        [OnEventFire]
        public void InitHangarCamera(NodeAddedEvent e, HangarCameraInitNode hangar)
        {
            if (hangar.hangar)
            {
                Camera componentInChildren = hangar.hangar.GetComponentInChildren<Camera>();
                componentInChildren.transform.parent.position = hangar.hangarCameraStartPosition.transform.position;
                componentInChildren.transform.parent.LookAt(hangar.hangarTankPosition.transform.position);
                hangar.Entity.AddComponent(new CameraRootTransformComponent(componentInChildren.transform.parent));
                hangar.Entity.AddComponent<HangarCameraComponent>();
                hangar.Entity.AddComponent(new CameraComponent(componentInChildren));
                this.SetupCameraESM(hangar.Entity);
                this.SetupCameraViewESM(hangar.Entity);
                this.SetupCameraRotationESM(hangar.Entity);
            }
        }

        [OnEventFire]
        public void OnMain(NodeAddedEvent e, SingleNode<MainScreenComponent> screen, HangarCameraNode camera)
        {
            camera.hangar.GetComponentInChildren<Camera>().GetComponent<CameraOffsetBehaviour>().SetOffset(0f);
        }

        private void SetupCameraESM(Entity camera)
        {
            HangarCameraStateComponent component = new HangarCameraStateComponent();
            camera.AddComponent(component);
            EntityStateMachine esm = component.Esm;
            esm.AddState<HangarCameraState.Enabled>();
            esm.AddState<HangarCameraState.Disabled>();
            esm.ChangeState<HangarCameraState.Disabled>();
        }

        private void SetupCameraRotationESM(Entity camera)
        {
            HangarCameraRotationStateComponent component = new HangarCameraRotationStateComponent();
            camera.AddComponent(component);
            EntityStateMachine esm = component.Esm;
            esm.AddState<HangarCameraRotationState.Enabled>();
            esm.AddState<HangarCameraRotationState.Disabled>();
            esm.ChangeState<HangarCameraRotationState.Disabled>();
        }

        private void SetupCameraViewESM(Entity camera)
        {
            HangarCameraViewStateComponent component = new HangarCameraViewStateComponent();
            camera.AddComponent(component);
            EntityStateMachine esm = component.Esm;
            esm.AddState<HangarCameraViewState.TankViewState>();
            esm.AddState<HangarCameraViewState.FlightToLocationState>();
            esm.AddState<HangarCameraViewState.LocationViewState>();
            esm.AddState<HangarCameraViewState.FlightToTankState>();
            esm.ChangeState<HangarCameraViewState.TankViewState>();
        }

        public class HangarCameraDisabledNode : HangarCameraSwitchSystem.HangarCameraNode
        {
            public HangarCameraDisabledComponent hangarCameraDisabled;
        }

        public class HangarCameraEnabledNode : HangarCameraSwitchSystem.HangarCameraNode
        {
            public HangarCameraEnabledComponent hangarCameraEnabled;
        }

        public class HangarCameraInitNode : Node
        {
            public HangarComponent hangar;
            public HangarTankPositionComponent hangarTankPosition;
            public HangarCameraStartPositionComponent hangarCameraStartPosition;
        }

        public class HangarCameraNode : Node
        {
            public HangarComponent hangar;
            public HangarCameraComponent hangarCamera;
            public CameraComponent camera;
            public HangarCameraViewStateComponent hangarCameraViewState;
            public HangarCameraStateComponent hangarCameraState;
            public HangarCameraRotationStateComponent hangarCameraRotationState;
        }

        public class HangarCameraRotationDisabledNode : HangarCameraSwitchSystem.HangarCameraNode
        {
            public HangarCameraRotationDisabledComponent hangarCameraRotationDisabled;
        }

        public class HangarCameraRotationEnabledNode : HangarCameraSwitchSystem.HangarCameraNode
        {
            public HangarCameraRotationEnabledComponent hangarCameraRotationEnabled;
        }

        public class ScreenNode : Node
        {
            public ScreenComponent screen;
            public ActiveScreenComponent activeScreen;
        }
    }
}

