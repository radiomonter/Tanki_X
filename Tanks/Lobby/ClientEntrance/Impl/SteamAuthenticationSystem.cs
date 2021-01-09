namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using Steamworks;
    using System;
    using UnityEngine;

    public class SteamAuthenticationSystem : ECSSystem
    {
        [OnEventFire]
        public void AuthenticateSteamUser(RequestSteamAuthenticationEvent e, SingleNode<ClientSessionComponent> clientSession, [JoinAll] SingleNode<SteamComponent> steam, [JoinAll] SingleNode<EntranceScreenComponent> entranceScreen)
        {
            if (!string.IsNullOrEmpty(SteamComponent.Ticket))
            {
                entranceScreen.component.LockScreen(true);
                PlayerPrefs.SetInt("SteamAuthentication", 0);
                AuthenticateSteamUserEvent event3 = new AuthenticateSteamUserEvent {
                    HardwareFingerpring = HardwareFingerprintUtils.HardwareFingerprint
                };
                string steamID = steam.component.SteamID;
                string text3 = steamID;
                if (steamID == null)
                {
                    string local1 = steamID;
                    text3 = string.Empty;
                }
                event3.SteamId = text3;
                event3.SteamNickname = SteamFriends.GetPersonaName();
                string ticket = SteamComponent.Ticket;
                string text4 = ticket;
                if (ticket == null)
                {
                    string local2 = ticket;
                    text4 = string.Empty;
                }
                event3.Ticket = text4;
                AuthenticateSteamUserEvent eventInstance = event3;
                base.ScheduleEvent(eventInstance, clientSession);
            }
        }

        [OnEventFire]
        public void DisableSteamLoginButton(NodeAddedEvent e, SingleNode<SteamLoginButtonComponent> button)
        {
            if (!SteamManager.Initialized || string.IsNullOrEmpty(SteamComponent.Ticket))
            {
                button.component.gameObject.SetActive(false);
            }
        }

        [OnEventFire]
        public void SteamAuthenticationButtonClick(ButtonClickEvent e, SingleNode<SteamLoginButtonComponent> button, [JoinAll] SingleNode<ClientSessionComponent> session, [JoinAll] SingleNode<EntranceScreenComponent> entranceScreen)
        {
            base.ScheduleEvent<RequestSteamAuthenticationEvent>(session);
            entranceScreen.component.LockScreen(true);
        }
    }
}

