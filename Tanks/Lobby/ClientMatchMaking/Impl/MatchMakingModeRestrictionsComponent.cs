namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class MatchMakingModeRestrictionsComponent : Component
    {
        private int _minimalRank;
        private int _maximalRank = 0x7fffffff;
        private int _minimalShowRank;
        private int _maximalShowRank = 0x7fffffff;

        public int MinimalRank
        {
            get => 
                this._minimalRank;
            set => 
                this._minimalRank = value;
        }

        public int MaximalRank
        {
            get => 
                this._maximalRank;
            set => 
                this._maximalRank = value;
        }

        public int MinimalShowRank
        {
            get => 
                this._minimalShowRank;
            set => 
                this._minimalShowRank = value;
        }

        public int MaximalShowRank
        {
            get => 
                this._maximalShowRank;
            set => 
                this._maximalShowRank = value;
        }
    }
}

