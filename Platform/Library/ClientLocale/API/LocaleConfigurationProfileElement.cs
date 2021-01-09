namespace Platform.Library.ClientLocale.API
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLocale.Impl;
    using Platform.Library.ClientLogger.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class LocaleConfigurationProfileElement : MonoBehaviour, ConfigurationProfileElement
    {
        public string language;

        [Inject]
        public static ConfigurationService configurationService { get; set; }

        public string ProfileElement
        {
            get
            {
                this.language = LocaleUtils.GetSavedLocaleCode();
                if (string.IsNullOrEmpty(this.language))
                {
                    DefaultLocaleConfiguration configuration = null;
                    try
                    {
                        configuration = configurationService.GetConfig(ConfigPath.DEFAULT_LOCALE).ConvertTo<DefaultLocaleConfiguration>();
                    }
                    catch (Exception exception)
                    {
                        LoggerProvider.GetLogger(this).WarnFormat("Unable to read default lcoale  {0}. {1}", exception.Message, exception);
                    }
                    if ((configuration != null) && !string.IsNullOrEmpty(configuration.DefaultLocale))
                    {
                        this.language = configuration.DefaultLocale;
                    }
                    else
                    {
                        SystemLanguage systemLanguage = Application.systemLanguage;
                        this.language = (systemLanguage == SystemLanguage.English) ? "en" : ((systemLanguage == SystemLanguage.Russian) ? "ru" : ((systemLanguage == SystemLanguage.Turkish) ? "tr" : "en"));
                    }
                }
                LocaleUtils.SaveLocaleCode(this.language);
                return this.language;
            }
        }
    }
}

