namespace Platform.Library.ClientResources.API
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class CommandLineParser
    {
        private string[] args;

        public CommandLineParser(string[] args)
        {
            this.args = args;
        }

        public string GetSubLine(string[] paramsName)
        {
            string str = string.Empty;
            foreach (string str2 in paramsName)
            {
                if (this.IsExist(str2))
                {
                    string valueOrDefault = this.GetValueOrDefault(str2, string.Empty);
                    string str4 = (valueOrDefault != string.Empty) ? (str2 + "=" + valueOrDefault) : str2;
                    str = str + str4 + " ";
                }
            }
            return str.Trim();
        }

        public string GetValue(string paramName)
        {
            string str;
            if (!this.TryGetValue(paramName, out str))
            {
                throw new ParameterNotFoundException(paramName);
            }
            return str;
        }

        public string GetValueOrDefault(string paramName, string defaultValue)
        {
            string str;
            return (!this.TryGetValue(paramName, out str) ? defaultValue : str);
        }

        public string[] GetValues(string paramName)
        {
            char[] separator = new char[] { ',' };
            return this.GetValue(paramName).Split(separator);
        }

        public bool IsExist(string paramName)
        {
            <IsExist>c__AnonStorey0 storey = new <IsExist>c__AnonStorey0 {
                paramName = paramName
            };
            return this.args.Any<string>(new Func<string, bool>(storey.<>m__0));
        }

        public bool TryGetValue(string paramName, out string paramValue)
        {
            foreach (string str in this.args)
            {
                if (str.StartsWith(paramName, StringComparison.Ordinal))
                {
                    paramValue = ((paramName.Length + 1) >= str.Length) ? string.Empty : str.Substring(paramName.Length + 1);
                    return true;
                }
            }
            paramValue = string.Empty;
            return false;
        }

        [CompilerGenerated]
        private sealed class <IsExist>c__AnonStorey0
        {
            internal string paramName;

            internal bool <>m__0(string arg) => 
                arg.StartsWith(this.paramName, StringComparison.Ordinal);
        }
    }
}

