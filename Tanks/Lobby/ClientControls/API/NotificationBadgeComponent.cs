namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;

    public class NotificationBadgeComponent : BehaviourComponent
    {
        public bool BadgeActivity
        {
            set => 
                base.gameObject.SetActive(value);
        }
    }
}

