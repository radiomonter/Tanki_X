namespace Tanks.ClientLauncher.API
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public class VersionInfo
    {
        public void Read(Stream stream)
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                reader.ReadToFollowing("version");
                this.Version = reader.ReadElementContentAsString();
                reader.ReadToFollowing("executable");
                this.Executable = reader.ReadElementContentAsString();
                reader.ReadToFollowing("distributeUrl");
                this.DistributionURL = reader.ReadElementContentAsString();
            }
        }

        public void Write(Stream stream)
        {
            XmlWriterSettings settings = new XmlWriterSettings {
                Indent = true
            };
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                writer.WriteStartElement("distribution");
                writer.WriteStartElement("version");
                writer.WriteString(this.Version);
                writer.WriteEndElement();
                writer.WriteStartElement("executable");
                writer.WriteString(this.Executable);
                writer.WriteEndElement();
                writer.WriteStartElement("distributeUrl");
                writer.WriteString(this.DistributionURL);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }

        public string Version { get; set; }

        public string Executable { get; set; }

        public string DistributionURL { get; set; }
    }
}

