namespace Platform.Library.ClientLocale.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientLocale.Impl;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d286f22b624f5dL)]
    public interface LocaleListTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        LocaleListComponent localeList();
    }
}

