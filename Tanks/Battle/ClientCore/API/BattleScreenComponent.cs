namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    [SerialVersionUID(0x8d2e6e107618e2aL)]
    public class BattleScreenComponent : ECSBehaviour, Component, AttachToEntityListener
    {
        private Entity entity;
        public GameObject hud;
        public GameObject topPanel;
        public GameObject tankInfo;
        public GameObject battleChat;
        public GameObject combatEventLog;
        public bool showTankInfo;
        public bool showBattleChat;
        public bool showCombatEventLog;

        public void AttachedToEntity(Entity entity)
        {
            this.entity = entity;
        }

        private void OnDisable()
        {
            if (this.hud != null)
            {
                this.hud.SetActive(false);
            }
            if (this.tankInfo != null)
            {
                this.tankInfo.SetActive(false);
            }
            if (this.battleChat != null)
            {
                this.battleChat.SetActive(false);
            }
            if (this.combatEventLog != null)
            {
                this.combatEventLog.SetActive(false);
            }
        }

        private void OnEnable()
        {
            this.hud.SetActive(true);
            this.topPanel.GetComponent<CanvasGroup>().alpha = 1f;
            this.tankInfo.SetActive(this.showTankInfo);
            this.battleChat.SetActive(this.showBattleChat);
        }

        private void OnGUI()
        {
            if ((this.entity != null) && ((Event.current.type == EventType.KeyDown) && InputManager.GetActionKeyDown(BattleActions.EXIT_BATTLE)))
            {
                base.ScheduleEvent<RequestGoBackFromBattleEvent>(this.entity);
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }
    }
}

