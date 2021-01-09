namespace Tanks.ClientLauncher
{
    using Lobby.ClientUserProfile.Impl;
    using Platform.Common.ClientECSCommon.src.main.csharp.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.Impl;
    using Platform.Library.ClientProtocol.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using Platform.Library.ClientUnityIntegration.Impl;
    using Platform.System.Data.Exchange.ClientNetwork.Impl;
    using Platform.System.Data.Statics.ClientConfigurator.Impl;
    using Platform.System.Data.Statics.ClientYaml.Impl;
    using Platform.Tool.ClientUnityLogger.Impl;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientNavigation.Impl;

    public class ActivatorBehaviour : ClientActivator
    {
        private readonly Type[] environmentActivators;
        [CompilerGenerated]
        private static Func<Type, Activator> <>f__am$cache0;

        public ActivatorBehaviour()
        {
            Type[] typeArray1 = new Type[11];
            typeArray1[0] = typeof(ClientLoggerActivator);
            typeArray1[1] = typeof(ClientUnityLoggerActivator);
            typeArray1[2] = typeof(CrashReporter);
            typeArray1[3] = typeof(ClientProtocolActivator);
            typeArray1[4] = typeof(YamlActivator);
            typeArray1[5] = typeof(ConfigurationServiceActivator);
            typeArray1[6] = typeof(EntitySystemActivator);
            typeArray1[7] = typeof(ClientECSCommonActivator);
            typeArray1[8] = typeof(ClientUserProfileActivator);
            typeArray1[9] = typeof(ClientCoreTemplatesActivator);
            typeArray1[10] = typeof(ClientUnityIntegrationActivator);
            this.environmentActivators = typeArray1;
        }

        public void Awake()
        {
            UnityProfiler.Listen();
            SceneSwitcher.Clean();
            base.ActivateClient(this.MakeCoreActivators(), this.MakeNonCoreActivators());
        }

        private List<Activator> MakeCoreActivators()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = t => (Activator) Activator.CreateInstance(t);
            }
            return this.environmentActivators.Select<Type, Activator>(<>f__am$cache0).ToList<Activator>();
        }

        private List<Activator> MakeNonCoreActivators()
        {
            Activator[] first = new Activator[] { new ClientNetworkActivator() };
            return first.Concat<Activator>(base.GetActivatorsAddedInUnityEditor()).ToList<Activator>();
        }

        private void OnApplicationQuit()
        {
            SceneSwitcher.DisposeUrlLoaders();
            WWWLoader.DisposeActiveLoaders();
            SceneSwitcher.Clean();
            Process.GetCurrentProcess().Kill();
        }
    }
}

