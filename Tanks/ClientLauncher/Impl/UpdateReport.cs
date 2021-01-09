namespace Tanks.ClientLauncher.Impl
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [Serializable]
    public class UpdateReport
    {
        public UpdateReport()
        {
            this.IsSuccess = true;
            this.Error = string.Empty;
            this.StackTrace = string.Empty;
            this.UpdateVersion = string.Empty;
        }

        public void Read(Stream stream)
        {
            UpdateReport report = (UpdateReport) new XmlSerializer(typeof(UpdateReport)).Deserialize(stream);
            this.UpdateVersion = report.UpdateVersion;
            this.IsSuccess = report.IsSuccess;
            this.Error = report.Error;
            this.StackTrace = report.StackTrace;
        }

        public void Write(Stream stream)
        {
            new XmlSerializer(typeof(UpdateReport)).Serialize(stream, this);
        }

        public bool IsSuccess { get; set; }

        public string UpdateVersion { get; set; }

        public string Error { get; set; }

        public string StackTrace { get; set; }
    }
}

