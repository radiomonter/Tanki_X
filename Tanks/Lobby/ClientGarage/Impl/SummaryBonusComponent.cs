namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class SummaryBonusComponent : LocalizedControl, Component
    {
        private Dictionary<MapKey, StaticBonusUI> effectToInstance = new Dictionary<MapKey, StaticBonusUI>();
        [SerializeField]
        private List<StaticBonusUI> bonuses = new List<StaticBonusUI>();
        private int usedBonuses;
        [SerializeField]
        private Text totalBonusText;

        protected override void Awake()
        {
            base.Awake();
            if (this.usedBonuses == 0)
            {
                base.gameObject.SetActive(false);
            }
        }

        private static string GetItemIcon(long marketItem) => 
            Flow.Current.EntityRegistry.GetEntity(marketItem).GetComponent<ItemIconComponent>().SpriteUid;

        private void OnDisable()
        {
            foreach (StaticBonusUI sui in this.bonuses)
            {
                sui.gameObject.SetActive(false);
            }
            this.effectToInstance.Clear();
            this.usedBonuses = 0;
            base.gameObject.SetActive(false);
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public string TotalBonusText
        {
            set => 
                this.totalBonusText.text = value;
        }

        [StructLayout(LayoutKind.Sequential, Size=1)]
        private struct MapKey
        {
            public long MarketItem { get; set; }
        }
    }
}

