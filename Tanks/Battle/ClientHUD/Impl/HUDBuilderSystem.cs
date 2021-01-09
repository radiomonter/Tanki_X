namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class HUDBuilderSystem : ECSSystem
    {
        [OnEventFire]
        public void AddESMComponent(NodeAddedEvent e, HUDScreenNode hud)
        {
            BattleHUDESMComponent component = new BattleHUDESMComponent();
            EntityStateMachine esm = component.Esm;
            esm.AddState<BattleHUDStates.ActionsState>();
            esm.AddState<BattleHUDStates.ChatState>();
            esm.AddState<BattleHUDStates.ShaftAimingState>();
            hud.Entity.AddComponent(component);
        }

        private static void CreateWorldSpaceCanvas(HUDScreenNode hud)
        {
            Object.Instantiate<GameObject>(hud.hudWorldSpaceCanvasPrefab.hudWorldSpaceCanvasPrefab).GetComponent<Canvas>().worldCamera = Camera.main;
        }

        [OnEventFire]
        public void HUDActivation(NodeAddedEvent e, HUDScreenNode hud, CameraNode camera)
        {
            CreateWorldSpaceCanvas(hud);
        }

        [OnEventFire]
        public void RemoveWorldSpaceCanvas(NodeRemoveEvent e, HUDScreenNode hud, [JoinAll] SingleNode<HUDWorldSpaceCanvas> worldSpaceHUD)
        {
            Object.Destroy(worldSpaceHUD.component.gameObject);
        }

        public class CameraNode : Node
        {
            public BattleCameraComponent battleCamera;
            public CameraComponent camera;
        }

        public class HUDScreenNode : Node
        {
            public BattleScreenComponent battleScreen;
            public HUDWorldSpaceCanvasPrefabComponent hudWorldSpaceCanvasPrefab;
        }
    }
}

