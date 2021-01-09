namespace Platform.Library.ClientLocale.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d286f2189cd63bL)]
    public interface LocaleTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        LocaleComponent locale();
    }
}

