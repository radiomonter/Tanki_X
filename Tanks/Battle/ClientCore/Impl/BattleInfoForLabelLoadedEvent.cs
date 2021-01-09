namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x8d323419135748eL)]
    public class BattleInfoForLabelLoadedEvent : Event
    {
        private Entity map;
        private long battleId;
        private string battleMode;

        public Entity Map
        {
            get => 
                this.map;
            set => 
                this.map = value;
        }

        public long BattleId
        {
            get => 
                this.battleId;
            set => 
                this.battleId = value;
        }

        public string BattleMode
        {
            get => 
                this.battleMode;
            set => 
                this.battleMode = value;
        }
    }
}

