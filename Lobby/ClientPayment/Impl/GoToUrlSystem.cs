namespace Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class GoToUrlSystem : ECSSystem
    {
        [OnEventFire]
        public void OpenUrl(GoToUrlToPayEvent e, Node node)
        {
            Application.OpenURL(e.Url);
        }
    }
}

