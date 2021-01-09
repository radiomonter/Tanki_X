namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class SpectatorBattleScreenComponent : BehaviourComponent, AttachToEntityListener, DetachFromEntityListener
    {
        public GameObject scoreTable;
        public GameObject scoreTableShadow;
        public GameObject spectatorChat;
        private Entity entity;

        public void AttachedToEntity(Entity entity)
        {
            this.entity = entity;
        }

        public void DetachedFromEntity(Entity entity)
        {
            this.entity = null;
        }

        private void OnDisable()
        {
            this.spectatorChat.SetActive(false);
        }

        private void OnGUI()
        {
            if ((this.entity != null) && ((Event.current.type == EventType.KeyDown) && InputManager.GetActionKeyDown(SpectatorCameraActions.GoBack)))
            {
                ECSBehaviour.EngineService.Engine.ScheduleEvent<SpectatorGoBackRequestEvent>(this.entity);
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }
    }
}

