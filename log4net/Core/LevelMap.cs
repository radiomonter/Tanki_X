namespace log4net.Core
{
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Reflection;

    public sealed class LevelMap
    {
        private Hashtable m_mapName2Level = SystemInfo.CreateCaseInsensitiveHashtable();

        public void Add(Level level)
        {
            if (level == null)
            {
                throw new ArgumentNullException("level");
            }
            lock (this)
            {
                this.m_mapName2Level[level.Name] = level;
            }
        }

        public void Add(string name, int value)
        {
            this.Add(name, value, null);
        }

        public void Add(string name, int value, string displayName)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length == 0)
            {
                throw SystemInfo.CreateArgumentOutOfRangeException("name", name, "Parameter: name, Value: [" + name + "] out of range. Level name must not be empty");
            }
            if ((displayName == null) || (displayName.Length == 0))
            {
                displayName = name;
            }
            this.Add(new Level(value, name, displayName));
        }

        public void Clear()
        {
            this.m_mapName2Level.Clear();
        }

        public Level LookupWithDefault(Level defaultLevel)
        {
            Level level2;
            if (defaultLevel == null)
            {
                throw new ArgumentNullException("defaultLevel");
            }
            lock (this)
            {
                Level level = (Level) this.m_mapName2Level[defaultLevel.Name];
                if (level != null)
                {
                    level2 = level;
                }
                else
                {
                    this.m_mapName2Level[defaultLevel.Name] = defaultLevel;
                    level2 = defaultLevel;
                }
            }
            return level2;
        }

        public Level this[string name]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }
                lock (this)
                {
                    return (Level) this.m_mapName2Level[name];
                }
            }
        }

        public LevelCollection AllLevels
        {
            get
            {
                lock (this)
                {
                    return new LevelCollection(this.m_mapName2Level.Values);
                }
            }
        }
    }
}

