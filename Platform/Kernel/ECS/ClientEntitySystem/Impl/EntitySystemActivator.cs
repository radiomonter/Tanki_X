namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Runtime.CompilerServices;

    public class EntitySystemActivator : DefaultActivator<AutoCompleting>
    {
        protected override void Activate()
        {
            TemplateRegistryImpl service = new TemplateRegistryImpl();
            ServiceRegistry.Current.RegisterService<TemplateRegistry>(service);
            ServiceRegistry.Current.RegisterService<ConfigEntityLoader>(new ConfigEntityLoaderImpl());
            ComponentBitIdRegistryImpl impl2 = new ComponentBitIdRegistryImpl();
            ServiceRegistry.Current.RegisterService<ComponentBitIdRegistry>(impl2);
            HandlerCollector handlerCollector = new HandlerCollector();
            ServiceRegistry.Current.RegisterService<NodeDescriptionRegistry>(new NodeDescriptionRegistryImpl());
            EngineServiceImpl impl3 = new EngineServiceImpl(service, handlerCollector, new EventMaker(handlerCollector), impl2);
            ServiceRegistry.Current.RegisterService<EngineService>(impl3);
            ServiceRegistry.Current.RegisterService<EngineServiceInternal>(impl3);
            ServiceRegistry.Current.RegisterService<TemplateRegistry>(service);
            ServiceRegistry.Current.RegisterService<GroupRegistry>(new GroupRegistryImpl());
            impl3.HandlerCollector.AddHandlerListener(impl2);
            YamlService.RegisterConverter(new EntityYamlConverter(impl3));
            YamlService.RegisterConverter(new TemplateDescriptionYamlConverter(service));
            ServiceRegistry.Current.RegisterService<FlowInstancesCache>(new FlowInstancesCache());
        }

        [Inject]
        public static Platform.System.Data.Statics.ClientYaml.API.YamlService YamlService { get; set; }
    }
}

