namespace Platform.Tool.ClientUnityLogger.API
{
    using log4net.Layout;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GelfLayout : PatternLayout
    {
        private const string GELF_VERSION = "1.1";
        private Dictionary<string, string> gelfLayoutPrototype;

        public GelfLayout()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string> {
                { 
                    "version",
                    "1.1"
                },
                { 
                    "host",
                    Environment.MachineName
                },
                { 
                    "short_message",
                    "%escapedMessage"
                },
                { 
                    "level",
                    "%syslogLevel"
                },
                { 
                    "_exception",
                    "%escapedException"
                },
                { 
                    "_device_id",
                    "%deviceId"
                },
                { 
                    "_ecs_session_id",
                    "%ECSSessionId"
                },
                { 
                    "_user",
                    "%UserUID"
                },
                { 
                    "_client_version",
                    "%ClientVersion"
                },
                { 
                    "_server_url",
                    "%InitUrl"
                }
            };
            this.gelfLayoutPrototype = dictionary;
            base.ConversionPattern = this.GetLayoutPattern();
            base.AddConverter("escapedMessage", typeof(MessageEscapeConvertor));
            base.AddConverter("syslogLevel", typeof(SyslogLevelConverter));
            base.AddConverter("escapedException", typeof(ExceptionEscapeConverter));
            base.AddConverter("deviceId", typeof(DeviceIdConverter));
            base.AddConverter("ECSSessionId", typeof(ECSSessionIdConverter));
            base.AddConverter("UserUID", typeof(UserUIDConverter));
            base.AddConverter("ClientVersion", typeof(ClientVersionConverter));
            base.AddConverter("InitUrl", typeof(ServerUrlConverter));
            base.AddConverter("deviceModel", typeof(DeviceModelConverter));
            base.AddConverter("deviceName", typeof(DeviceNameConverter));
            base.AddConverter("buildGUID", typeof(BuildGuidConverter));
            base.AddConverter("operatingSystem", typeof(OperatingSystemConverter));
            this.ActivateOptions();
        }

        private string GetLayoutPattern()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{").AppendLine();
            foreach (KeyValuePair<string, string> pair in this.gelfLayoutPrototype)
            {
                builder.AppendFormat("\"{0}\": \"{1}\",\n", pair.Key, pair.Value);
            }
            builder.Remove(builder.Length - 2, 1);
            builder.Append("}");
            return builder.ToString();
        }
    }
}

