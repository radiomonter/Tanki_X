namespace Platform.System.Data.Statics.ClientConfigurator.API
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.System.Data.Statics.ClientConfigurator.Impl;
    using Platform.System.Data.Statics.ClientYaml.API;
    using Platform.System.Data.Statics.ClientYaml.Impl;
    using SharpCompress.Common;
    using SharpCompress.Reader;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class TarImporter
    {
        private string GetDirectoryName(string filePath)
        {
            char[] trimChars = new char[] { Path.DirectorySeparatorChar };
            return Path.GetDirectoryName(filePath).Trim(trimChars).Replace(Path.DirectorySeparatorChar, '/');
        }

        public T Import<T>(Stream inputStream, ConfigurationProfile configurationProfile) where T: ConfigTreeNode, new()
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream");
            }
            if (configurationProfile == null)
            {
                throw new ArgumentNullException("configurationProfile");
            }
            using (MemoryStream stream = new MemoryStream())
            {
                this.TransferTo(inputStream, stream);
                stream.Seek(0L, SeekOrigin.Begin);
                return this.ReadToConfigTree<T>(stream, configurationProfile);
            }
        }

        public T ImportAll<T>(Stream inputStream) where T: ConfigTreeNode, new() => 
            this.Import<T>(inputStream, AnyProfile.Instance);

        private YamlNodeImpl LoadYaml(IReader reader)
        {
            YamlNodeImpl impl;
            using (EntryStream stream = reader.OpenEntryStream())
            {
                using (MemoryStream stream2 = new MemoryStream())
                {
                    this.TransferTo(stream, stream2);
                    stream2.Seek(0L, SeekOrigin.Begin);
                    using (StreamReader reader2 = new StreamReader(stream2))
                    {
                        impl = YamlService.Load(reader2);
                    }
                }
            }
            return impl;
        }

        private static string NormalizePath(string path) => 
            !path.StartsWith("./") ? path : path.Substring(2);

        private T ReadToConfigTree<T>(Stream inputStream, ConfigurationProfile configurationProfile) where T: ConfigTreeNode, new()
        {
            T local = Activator.CreateInstance<T>();
            ConfigsMerger merger = new ConfigsMerger();
            string str = null;
            try
            {
                IReader reader = ReaderFactory.Open(inputStream, Options.KeepStreamsOpen);
                while (reader.MoveToNextEntry())
                {
                    str = NormalizePath(reader.Entry.FilePath);
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (reader.Entry.IsDirectory)
                        {
                            local.FindOrCreateNode(str);
                            continue;
                        }
                        if (configurationProfile.Match(Path.GetFileName(str)))
                        {
                            merger.Put(local.FindOrCreateNode(this.GetDirectoryName(str)), Path.GetFileNameWithoutExtension(str), this.LoadYaml(reader));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LoggerProvider.GetLogger(this).Fatal("Error read configs " + str, exception);
                throw;
            }
            merger.Merge();
            return local;
        }

        private void TransferTo(Stream source, Stream destination)
        {
            int num;
            byte[] buffer = new byte[0x1000];
            while ((num = source.Read(buffer, 0, buffer.Length)) != 0)
            {
                destination.Write(buffer, 0, num);
            }
        }

        [Inject]
        public static Platform.System.Data.Statics.ClientYaml.API.YamlService YamlService { get; set; }

        private class AnyProfile : ConfigurationProfile
        {
            public static readonly ConfigurationProfile Instance = new TarImporter.AnyProfile();

            public bool Match(string configName) => 
                true;
        }
    }
}

