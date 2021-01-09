namespace Platform.System.Data.Statics.ClientYaml.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;

    public class YamlActivator : DefaultActivator<AutoCompleting>
    {
        protected override void Activate()
        {
            ServiceRegistry.Current.RegisterService<YamlService>(new YamlServiceImpl());
        }
    }
}

