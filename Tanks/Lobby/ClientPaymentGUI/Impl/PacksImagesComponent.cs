namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public class PacksImagesComponent : Component
    {
        private Dictionary<long, List<string>> amountToImages;

        public Dictionary<long, List<string>> AmountToImages
        {
            get => 
                this.amountToImages;
            set => 
                this.amountToImages = value;
        }
    }
}

