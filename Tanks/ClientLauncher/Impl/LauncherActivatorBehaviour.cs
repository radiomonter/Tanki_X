namespace Tanks.ClientLauncher.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.ClientLauncher;
    using Tanks.ClientLauncher.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientNavigation.Impl;
    using UnityEngine;

    public class LauncherActivatorBehaviour : MonoBehaviour
    {
        private readonly Type[] environmentActivators = new Type[] { typeof(ClientLoggerActivator), typeof(ClientUnityLoggerActivator), typeof(ClientProtocolActivator), typeof(YamlActivator), typeof(ConfigurationServiceActivator), typeof(EntitySystemActivator), typeof(ClientECSCommonActivator) };
        [CompilerGenerated]
        private static Func<Type, Activator> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<Activator, bool> <>f__am$cache1;

        public void Awake()
        {
            this.ProcessAdditionalClientCommands();
            if (ClientUpdater.IsUpdaterRunning())
            {
                Application.Quit();
            }
            else
            {
                SceneSwitcher.Clean();
                if (!this.TryUpdateVersion())
                {
                    this.LaunchActivators();
                }
            }
        }

        private IEnumerable<Activator> GetActivatorsAddedInUnityEditor()
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = a => ((MonoBehaviour) a).enabled;
            }
            return base.gameObject.GetComponentsInChildren<Activator>().Where<Activator>(<>f__am$cache1);
        }

        private void LaunchActivators()
        {
            try
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = t => (Activator) Activator.CreateInstance(t);
                }
                new ActivatorsLauncher(this.environmentActivators.Select<Type, Activator>(<>f__am$cache0)).LaunchAll(() => new ActivatorsLauncher(this.GetActivatorsAddedInUnityEditor()).LaunchAll(new Action(this.OnAllActivatorsLaunched)));
            }
            catch (Exception exception)
            {
                LoggerProvider.GetLogger<LauncherActivatorBehaviour>().Error(exception.Message, exception);
                FatalErrorHandler.ShowFatalErrorScreen("clientlocal/ui/screen/error/unexpected");
            }
        }

        private void OnAllActivatorsLaunched()
        {
            base.gameObject.AddComponent<PreciseTimeBehaviour>();
            base.gameObject.AddComponent<EngineBehaviour>();
            base.GetComponent<LauncherBehaviour>().Launch();
        }

        private void ProcessAdditionalClientCommands()
        {
            string str;
            if (new CommandLineParser(Environment.GetCommandLineArgs()).TryGetValue(LauncherConstants.CLEAN_PREFS, out str))
            {
                PlayerPrefs.DeleteAll();
            }
        }

        private bool TryUpdateVersion()
        {
            if (!ClientUpdater.IsApplicationRunInUpdateMode())
            {
                return false;
            }
            ClientUpdater.Update();
            return true;
        }
    }
}

