namespace Platform.System.Data.Statics.ClientConfigurator.Impl
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;

    public class ConfigurationProfileImpl : ConfigurationProfile
    {
        private readonly string[] profileElements;
        private readonly string PROFILE_PATTERN = "_([A-Za-z0-9]+)";
        private readonly string PUBLIC_CONFIG_PATTERN = @"public(_[A-Za-z0-9]+)*\.yml";
        private Regex regexProfile;
        private Regex regexConfig;

        public ConfigurationProfileImpl(string[] profileElements = null)
        {
            this.profileElements = profileElements;
            this.regexProfile = new Regex(this.PROFILE_PATTERN);
            this.regexConfig = new Regex(this.PUBLIC_CONFIG_PATTERN);
        }

        public bool Match(string configName)
        {
            bool flag;
            if (this.profileElements == null)
            {
                return configName.Equals("public.yml");
            }
            if (!this.regexConfig.IsMatch(configName))
            {
                return false;
            }
            IEnumerator enumerator = this.regexProfile.Matches(configName).GetEnumerator();
            try
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        System.Text.RegularExpressions.Match current = (System.Text.RegularExpressions.Match) enumerator.Current;
                        if (this.profileElements.Contains<string>(current.Groups[1].Value))
                        {
                            continue;
                        }
                        flag = false;
                    }
                    else
                    {
                        return true;
                    }
                    break;
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            return flag;
        }
    }
}

