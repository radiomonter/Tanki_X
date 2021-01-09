namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Steamworks;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.Impl;
    using UnityEngine;

    public class SteamConnector : MonoBehaviour
    {
        private static bool initialized;
        private static SteamConnector instance;
        [SerializeField]
        private SteamComponent steamEntityBehaviourPrefab;
        private static SteamComponent steamComponent;
        protected static Callback<GetAuthSessionTicketResponse_t> GetAuthSessionTicketResponse;
        protected static Callback<MicroTxnAuthorizationResponse_t> MicroTxnAuthorizationResponse;
        protected static Callback<DlcInstalled_t> DlcInstalled;
        [CompilerGenerated]
        private static Callback<GetAuthSessionTicketResponse_t>.DispatchDelegate <>f__mg$cache0;
        [CompilerGenerated]
        private static Callback<MicroTxnAuthorizationResponse_t>.DispatchDelegate <>f__mg$cache1;
        [CompilerGenerated]
        private static Callback<DlcInstalled_t>.DispatchDelegate <>f__mg$cache2;

        private void Initialize()
        {
            if (!SteamManager.Initialized)
            {
                Destroy(this);
            }
            else
            {
                SteamManager manager = FindObjectOfType<SteamManager>();
                if ((manager != null) && (manager.GetComponent<SkipRemoveOnSceneSwitch>() == null))
                {
                    manager.gameObject.AddComponent<SkipRemoveOnSceneSwitch>();
                }
                if (!initialized)
                {
                    initialized = true;
                    <>f__mg$cache0 ??= new Callback<GetAuthSessionTicketResponse_t>.DispatchDelegate(SteamConnector.OnGetAuthSessionTicketResponse);
                    GetAuthSessionTicketResponse = Callback<GetAuthSessionTicketResponse_t>.Create(<>f__mg$cache0);
                    <>f__mg$cache1 ??= new Callback<MicroTxnAuthorizationResponse_t>.DispatchDelegate(SteamConnector.OnMicroTxnAuthorizationResponse);
                    MicroTxnAuthorizationResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(<>f__mg$cache1);
                    <>f__mg$cache2 ??= new Callback<DlcInstalled_t>.DispatchDelegate(SteamConnector.OnDlcInstalled);
                    DlcInstalled = Callback<DlcInstalled_t>.Create(<>f__mg$cache2);
                }
                if (steamComponent == null)
                {
                    steamComponent = Instantiate<SteamComponent>(this.steamEntityBehaviourPrefab);
                    steamComponent.transform.SetParent(base.transform);
                    steamComponent.GetTicket(false);
                }
            }
        }

        private static void OnDlcInstalled(DlcInstalled_t pCallback)
        {
            if ((steamComponent != null) && !string.IsNullOrEmpty(SteamComponent.Ticket))
            {
                RequestCheckSteamDlcInstalledEvent eventInstance = new RequestCheckSteamDlcInstalledEvent();
                EngineService.Engine.ScheduleEvent(eventInstance, steamComponent.Entity);
            }
        }

        private static void OnGetAuthSessionTicketResponse(GetAuthSessionTicketResponse_t pCallback)
        {
            if (steamComponent != null)
            {
                steamComponent.OnGetAuthSessionTicketResponse(pCallback);
            }
        }

        private static void OnMicroTxnAuthorizationResponse(MicroTxnAuthorizationResponse_t pCallback)
        {
            if (steamComponent != null)
            {
                MicroTxnAuthorizationResponseEvent eventInstance = new MicroTxnAuthorizationResponseEvent(pCallback);
                EngineService.Engine.ScheduleEvent(eventInstance, steamComponent.Entity);
            }
        }

        public void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(base.gameObject);
            }
            instance.Initialize();
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

