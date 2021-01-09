namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class MVPModuleContainerParallaxEnabler : HoverHandler
    {
        [SerializeField]
        private ParallaxContainer parallaxContainer;

        protected override bool pointerIn
        {
            set
            {
                base.pointerIn = value;
                this.parallaxContainer.IsActive = value;
            }
        }
    }
}

