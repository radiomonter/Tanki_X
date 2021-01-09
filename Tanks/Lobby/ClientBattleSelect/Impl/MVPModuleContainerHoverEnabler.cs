namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class MVPModuleContainerHoverEnabler : HoverHandler
    {
        [SerializeField]
        private Animator animator;

        protected override bool pointerIn
        {
            set
            {
                base.pointerIn = value;
                this.animator.SetBool("pointerIn", value);
            }
        }
    }
}

