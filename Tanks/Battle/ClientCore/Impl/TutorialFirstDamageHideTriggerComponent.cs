namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class TutorialFirstDamageHideTriggerComponent : TutorialHideTriggerComponent
    {
        [SerializeField]
        private float showTime = 30f;
        private float timer;

        private void OnEnable()
        {
            this.timer = 0f;
        }

        protected void Update()
        {
            if (!base.triggered)
            {
                this.timer += Time.deltaTime;
                if (InputManager.GetActionKeyDown(InventoryAction.INVENTORY_SLOT2) || (this.timer >= this.showTime))
                {
                    this.Triggered();
                }
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }
    }
}

