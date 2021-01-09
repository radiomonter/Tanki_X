namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class BackgroundComponent : BackgroundDimensionsChangeComponent, Component, NoScaleScreen
    {
        private const string VISIBLE_ANIMATION_PARAM = "Visible";

        public virtual void Hide()
        {
            base.GetComponent<Animator>().SetBool("Visible", false);
        }

        public virtual void Show()
        {
            base.GetComponent<Animator>().SetBool("Visible", true);
        }
    }
}

