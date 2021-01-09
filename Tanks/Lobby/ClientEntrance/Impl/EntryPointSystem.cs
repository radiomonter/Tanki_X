namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using System.IO;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class EntryPointSystem : ECSSystem
    {
        public const string AUTO_AUTHENTICATION_TOKEN = "TOToken";
        public const string AUTO_AUTHENTICATION_LOGIN = "TOLogin";
        public const string STEAM_AUTHENTICATION_KEY = "SteamAuthentication";

        [OnEventFire]
        public void CheckAutoLogin(NodeAddedEvent e, SecuredClientSessionNode clientSession, SingleNode<ScreensRegistryComponent> screenRegistry, SingleNode<EntranceValidationRulesComponent> validationRules)
        {
            string str = PlayerPrefs.GetString("TOLogin");
            string str2 = PlayerPrefs.GetString("TOToken");
            if (!clientSession.Entity.HasComponent<InviteComponent>() && (!string.IsNullOrEmpty(str2) && !string.IsNullOrEmpty(str)))
            {
                AutoLoginUserEvent eventInstance = new AutoLoginUserEvent {
                    Uid = str,
                    EncryptedToken = PasswordSecurityUtils.RSAEncrypt(clientSession.sessionSecurityPublic.PublicKey, Convert.FromBase64String(str2)),
                    HardwareFingerprint = HardwareFingerprintUtils.HardwareFingerprint
                };
                base.ScheduleEvent(eventInstance, clientSession);
            }
            else if (this.IsSteamUserLogin())
            {
                base.ScheduleEvent<ShowFirstScreenEvent<EntranceScreenComponent>>(screenRegistry);
                base.ScheduleEvent<RequestSteamAuthenticationEvent>(clientSession);
            }
            else if (string.IsNullOrEmpty(SaveLoginSystem.GetSavedLogin()) && string.IsNullOrEmpty(clientSession.webId.WebIdUid))
            {
                base.ScheduleEvent<ShowFirstScreenEvent<RegistrationScreenComponent>>(screenRegistry);
            }
            else
            {
                base.ScheduleEvent<ShowFirstScreenEvent<EntranceScreenComponent>>(screenRegistry);
            }
        }

        private void ClearAutoLoginToken()
        {
            PlayerPrefs.DeleteKey("TOToken");
            PlayerPrefs.DeleteKey("TOLogin");
        }

        [OnEventFire]
        public void ContinueWithLogin(AutoLoginFailedEvent e, Node any, [JoinAll] SingleNode<TopPanelComponent> topPanel)
        {
            this.ClearAutoLoginToken();
            base.ScheduleEvent<ShowFirstScreenEvent<EntranceScreenComponent>>(topPanel);
        }

        private string DecryptToken(byte[] passwordDigest, byte[] encryptedToken)
        {
            byte[] inArray = new byte[encryptedToken.Length];
            for (int i = 0; i < encryptedToken.Length; i++)
            {
                inArray[i] = (byte) (encryptedToken[i] ^ passwordDigest[i % passwordDigest.Length]);
            }
            return Convert.ToBase64String(inArray);
        }

        private bool IsSteamUserLogin()
        {
            bool flag = PlayerPrefs.GetInt("SteamAuthentication", 0) == 1;
            return (SteamManager.Initialized && !flag);
        }

        [Mandatory, OnEventFire]
        public void SaveToken(SaveAutoLoginTokenEvent e, Node user, SessionAwaitingTokenNode clientSession)
        {
            string str = this.DecryptToken(clientSession.autoLoginTokenAwaiting.PasswordDigest, e.Token);
            PlayerPrefs.SetString("TOLogin", e.Uid);
            PlayerPrefs.SetString("TOToken", str);
            clientSession.Entity.RemoveComponent<AutoLoginTokenAwaitingComponent>();
        }

        [OnEventFire]
        public void SendWebId(NodeAddedEvent e, SingleNode<ClientSessionComponent> node)
        {
            string str;
            try
            {
                str = File.ReadAllText(Application.dataPath + "/USER_ID");
                long num = Convert.ToInt64(str);
            }
            catch (Exception)
            {
                str = string.Empty;
            }
            base.ScheduleEvent(new ClientLaunchEvent(str), node);
        }

        public class SecuredClientSessionNode : Node
        {
            public ClientSessionComponent clientSession;
            public SessionSecurityPublicComponent sessionSecurityPublic;
            public WebIdComponent webId;
        }

        public class SessionAwaitingTokenNode : Node
        {
            public ClientSessionComponent clientSession;
            public AutoLoginTokenAwaitingComponent autoLoginTokenAwaiting;
        }
    }
}

