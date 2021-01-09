namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class MandatoryAssetsFirstLoadingComponent : Component
    {
        private MandatoryRequestsState currentRequestsState;
        private MandatoryRequestsState finishRequestsState = (MandatoryRequestsState.CONTAINER | MandatoryRequestsState.HULL_SKIN);
        private List<AssetRequestComponent> assetsRequests = new List<AssetRequestComponent>();

        public bool AllMandatoryAssetsAreRequested() => 
            this.currentRequestsState == this.finishRequestsState;

        public bool IsAssetRequestMandatory(AssetRequestComponent assetRequest) => 
            this.assetsRequests.Contains(assetRequest);

        public bool IsContainerRequested() => 
            (this.currentRequestsState & MandatoryRequestsState.CONTAINER) == MandatoryRequestsState.CONTAINER;

        public void MarkAsRequested(AssetRequestComponent assetRequest, MandatoryRequestsState mandatoryRequestsState)
        {
            this.currentRequestsState |= mandatoryRequestsState;
            this.assetsRequests.Add(assetRequest);
        }

        public bool LoadingScreenShown { get; set; }

        [Flags]
        public enum MandatoryRequestsState
        {
            HANGAR,
            WEAPON_SKIN,
            HULL_SKIN,
            TANK_COLORING,
            WEAPON_COLORING,
            CONTAINER
        }
    }
}

