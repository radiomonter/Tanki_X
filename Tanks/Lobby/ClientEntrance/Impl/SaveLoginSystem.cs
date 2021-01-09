namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class SaveLoginSystem : ECSSystem
    {
        public const string LOGIN_PLAYERPREFS_KEY = "PlayerLogin";
        public const string REMEMBERME_PLAYERPREFS_KEY = "RemeberMeFlag";

        [OnEventFire]
        public void FillLoginFromWebId(NodeAddedEvent e, LoginInputFieldNode loginInput, [Context, JoinByScreen] PasswordInputFieldNode passwordInput, [JoinAll] SingleNode<ScreensRegistryComponent> screenRegistry, [JoinAll] SingleNode<WebIdComponent> clientSession)
        {
            if (string.IsNullOrEmpty(GetSavedLogin()))
            {
                string webIdUid = clientSession.component.WebIdUid;
                if (!string.IsNullOrEmpty(webIdUid))
                {
                    loginInput.inputField.Input = webIdUid;
                    this.SelectPasswordField(passwordInput);
                }
            }
        }

        private static string GetCommandlineParam(string paramName, string defaultValue)
        {
            <GetCommandlineParam>c__AnonStorey0 storey = new <GetCommandlineParam>c__AnonStorey0();
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            string str = defaultValue;
            storey.paramWithSeparator = paramName + "=";
            string str2 = commandLineArgs.FirstOrDefault<string>(new Func<string, bool>(storey.<>m__0));
            if (!string.IsNullOrEmpty(str2))
            {
                str = str2.Substring(storey.paramWithSeparator.Length);
            }
            return str;
        }

        public static string GetSavedLogin() => 
            GetCommandlineParam("login", PlayerPrefs.GetString("PlayerLogin"));

        [OnEventComplete]
        public void RetrieveLogin(NodeAddedEvent e, LoginInputFieldNode loginInput, [Context, JoinByScreen] PasswordInputFieldNode passwordInput, [JoinAll] SingleNode<ClientSessionComponent> clientSessionNode)
        {
            string savedLogin = GetSavedLogin();
            if (!string.IsNullOrEmpty(savedLogin))
            {
                loginInput.inputField.Input = savedLogin;
                this.SelectPasswordField(passwordInput);
            }
        }

        [OnEventComplete]
        public void RetrievePassword(NodeAddedEvent e, PasswordInputFieldNode passwordInput, [Context, JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreen)
        {
            string commandlineParam = GetCommandlineParam("password", string.Empty);
            if (!string.IsNullOrEmpty(commandlineParam))
            {
                passwordInput.inputField.Input = commandlineParam;
                entranceScreen.component.RememberMe = false;
            }
        }

        [OnEventFire]
        public void SaveLogin(NodeAddedEvent e, SelfUserNode node)
        {
            PlayerPrefs.SetString("PlayerLogin", node.userUid.Uid);
        }

        [OnEventFire]
        public void SaveLogin(UIDChangedEvent e, SelfUserNode node)
        {
            PlayerPrefs.SetString("PlayerLogin", node.userUid.Uid);
        }

        private void SelectPasswordField(PasswordInputFieldNode passwordInput)
        {
            InputFieldComponent inputField = passwordInput.inputField;
            if (inputField.InputField != null)
            {
                inputField.InputField.Select();
            }
            else
            {
                inputField.TMPInputField.Select();
            }
        }

        [OnEventFire]
        public void SetRemeberMeOptionOnLoad(NodeAddedEvent e, SingleNode<EntranceScreenComponent> entranceScreen)
        {
            entranceScreen.component.RememberMe = !PlayerPrefs.HasKey("RemeberMeFlag") || (PlayerPrefs.GetInt("RemeberMeFlag") != 0);
        }

        [OnEventFire]
        public void StoreRemeberMeOption(ButtonClickEvent e, SingleNode<LoginButtonComponent> loginButton, [JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreen)
        {
            PlayerPrefs.SetInt("RemeberMeFlag", !entranceScreen.component.RememberMe ? 0 : 1);
        }

        [CompilerGenerated]
        private sealed class <GetCommandlineParam>c__AnonStorey0
        {
            internal string paramWithSeparator;

            internal bool <>m__0(string arg) => 
                arg.StartsWith(this.paramWithSeparator);
        }

        public class LoginInputFieldNode : Node
        {
            public LoginInputFieldComponent loginInputField;
            public InputFieldComponent inputField;
            public ESMComponent esm;
            public InteractivityPrerequisiteStateComponent interactivityPrerequisiteState;
        }

        public class PasswordInputFieldNode : Node
        {
            public PasswordInputFieldComponent passwordInputField;
            public InputFieldComponent inputField;
            public ESMComponent esm;
            public InteractivityPrerequisiteStateComponent interactivityPrerequisiteState;
        }

        public class SelfUserNode : Node
        {
            public UserComponent user;
            public UserUidComponent userUid;
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
        }
    }
}

