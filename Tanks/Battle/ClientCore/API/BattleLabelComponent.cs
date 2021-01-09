namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    [SerialVersionUID(0x8d323372bc5c4c8L)]
    public class BattleLabelComponent : EntityBehaviour, Component
    {
        [SerializeField]
        private long battleId;
        [SerializeField]
        private GameObject map;
        [SerializeField]
        private GameObject mode;
        [SerializeField]
        private GameObject battleIcon;

        public long BattleId
        {
            get => 
                this.battleId;
            set
            {
                this.battleId = value;
                base.gameObject.AddComponent<BattleLabelReadyComponent>();
            }
        }

        public string Map
        {
            get => 
                this.map.GetComponent<Text>().text;
            set
            {
                this.map.GetComponent<Text>().text = value;
                this.map.SetActive(true);
            }
        }

        public string Mode
        {
            get => 
                this.mode.GetComponent<Text>().text;
            set
            {
                this.mode.GetComponent<Text>().text = value;
                this.mode.SetActive(true);
            }
        }

        public bool BattleIconActivity
        {
            get => 
                this.battleIcon.activeSelf;
            set => 
                this.battleIcon.SetActive(value);
        }
    }
}

