namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class SoundListenerSystem : ECSSystem
    {
        private void ApplyListenerTransformToCamera(SoundListenerComponent soundListener, Transform cameraTransform)
        {
            Transform transform = soundListener.transform;
            transform.position = cameraTransform.position;
            transform.rotation = cameraTransform.rotation;
        }

        [OnEventFire]
        public void InitSoundListenerInBattle(NodeAddedEvent evt, SingleNode<SoundListenerComponent> soundListener, [Context, JoinAll] BattleCameraNode node)
        {
            this.ApplyListenerTransformToCamera(soundListener.component, node.cameraRootTransform.Root);
        }

        [OnEventFire]
        public void UpdateSoundListenerInBattle(UpdateEvent evt, SingleNode<SoundListenerComponent> soundListener, [JoinAll] BattleCameraNode node, [JoinAll] SingleNode<MapInstanceComponent> map)
        {
            this.ApplyListenerTransformToCamera(soundListener.component, node.cameraRootTransform.Root);
        }

        [OnEventFire]
        public void UpdateSoundListenerInHangar(UpdateEvent evt, SoundListenerInLobbyNode soundListener, [JoinAll] HangarCameraNode node)
        {
            this.ApplyListenerTransformToCamera(soundListener.soundListener, node.cameraRootTransform.Root);
        }

        public class BattleCameraNode : SoundListenerSystem.CameraNode
        {
            public BattleCameraComponent battleCamera;
        }

        public class CameraNode : Node
        {
            public CameraComponent camera;
            public CameraRootTransformComponent cameraRootTransform;
        }

        public class HangarCameraNode : SoundListenerSystem.CameraNode
        {
            public HangarCameraComponent hangarCamera;
        }

        public class SoundListenerInLobbyNode : Node
        {
            public SoundListenerComponent soundListener;
            public SoundListenerLobbyStateComponent soundListenerLobbyState;
        }
    }
}

