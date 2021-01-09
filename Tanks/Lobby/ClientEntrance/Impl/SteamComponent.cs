namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using Steamworks;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class SteamComponent : ECSBehaviour, Component, AttachToEntityListener
    {
        private float MIN_RETRY_DELAY = 10f;
        private static byte[] ticket;
        private static uint ticketLength;
        private float lastRequestTicketTime = -10f;
        private bool goToPayment;

        public void AttachedToEntity(Platform.Kernel.ECS.ClientEntitySystem.API.Entity entity)
        {
            this.Entity = entity;
        }

        public void GetTicket(bool goToPayment = false)
        {
            if ((Time.unscaledTime - this.lastRequestTicketTime) > this.MIN_RETRY_DELAY)
            {
                this.lastRequestTicketTime = Time.unscaledTime;
                this.goToPayment = goToPayment;
                if (SteamManager.Initialized && SteamAPI.IsSteamRunning())
                {
                    ticket = new byte[0x400];
                    SteamUser.GetAuthSessionTicket(ticket, 0x400, out ticketLength);
                }
            }
        }

        public void OnGetAuthSessionTicketResponse(GetAuthSessionTicketResponse_t pCallback)
        {
            Debug.Log("OnGetAuthSessionTicketResponse ");
            string str = string.Empty;
            for (int i = 0; i < ticketLength; i++)
            {
                str = str + $"{ticket[i]:X2}";
            }
            Ticket = str;
            base.ScheduleEvent(new SteamAuthSessionRecievedEvent(), this.Entity);
        }

        public Platform.Kernel.ECS.ClientEntitySystem.API.Entity Entity { get; set; }

        public static string Ticket { get; private set; }

        public string SteamID =>
            SteamUser.GetSteamID().m_SteamID.ToString();
    }
}

