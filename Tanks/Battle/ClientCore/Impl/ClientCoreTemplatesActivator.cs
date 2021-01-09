namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ClientCoreTemplatesActivator : DefaultActivator<AutoCompleting>
    {
        protected override void Activate()
        {
            TemplateRegistry.Register(typeof(BattleTemplate));
            TemplateRegistry.Register(typeof(DMTemplate));
            TemplateRegistry.Register(typeof(TeamBattleTemplate));
            TemplateRegistry.Register(typeof(TDMTemplate));
            TemplateRegistry.Register(typeof(CTFTemplate));
            TemplateRegistry.Register(typeof(TeamTemplate));
            TemplateRegistry.Register(typeof(FlagTemplate));
            TemplateRegistry.Register(typeof(PedestalTemplate));
            TemplateRegistry.Register<ServerShutdownNotificationTemplate>();
            TemplateRegistry.Register<BattleShutdownNotificationTemplate>();
            TemplateRegistry.Register<ServerShutdownTemplate>();
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

