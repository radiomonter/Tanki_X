namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientLocale.API;
    using System;

    public class ClientLocaleSystem : ECSSystem
    {
        [OnEventFire]
        public void SetLocale(NodeAddedEvent e, SingleNode<ClientSessionComponent> session)
        {
            string savedLocaleCode = LocaleUtils.GetSavedLocaleCode();
            session.Entity.AddComponent(new ClientLocaleComponent(savedLocaleCode));
        }
    }
}

