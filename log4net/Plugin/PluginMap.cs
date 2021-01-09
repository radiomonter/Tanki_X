namespace log4net.Plugin
{
    using log4net.Repository;
    using System;
    using System.Collections;
    using System.Reflection;

    public sealed class PluginMap
    {
        private readonly Hashtable m_mapName2Plugin = new Hashtable();
        private readonly ILoggerRepository m_repository;

        public PluginMap(ILoggerRepository repository)
        {
            this.m_repository = repository;
        }

        public void Add(IPlugin plugin)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException("plugin");
            }
            IPlugin plugin2 = null;
            lock (this)
            {
                plugin2 = this.m_mapName2Plugin[plugin.Name] as IPlugin;
                this.m_mapName2Plugin[plugin.Name] = plugin;
            }
            if (plugin2 != null)
            {
                plugin2.Shutdown();
            }
            plugin.Attach(this.m_repository);
        }

        public void Remove(IPlugin plugin)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException("plugin");
            }
            lock (this)
            {
                this.m_mapName2Plugin.Remove(plugin.Name);
            }
        }

        public IPlugin this[string name]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }
                lock (this)
                {
                    return (IPlugin) this.m_mapName2Plugin[name];
                }
            }
        }

        public PluginCollection AllPlugins
        {
            get
            {
                lock (this)
                {
                    return new PluginCollection(this.m_mapName2Plugin.Values);
                }
            }
        }
    }
}

