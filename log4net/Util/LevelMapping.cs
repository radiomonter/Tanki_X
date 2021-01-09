namespace log4net.Util
{
    using log4net.Core;
    using System;
    using System.Collections;

    public sealed class LevelMapping : IOptionHandler
    {
        private Hashtable m_entriesMap = new Hashtable();
        private LevelMappingEntry[] m_entries;

        public void ActivateOptions()
        {
            Level[] array = new Level[this.m_entriesMap.Count];
            LevelMappingEntry[] entryArray = new LevelMappingEntry[this.m_entriesMap.Count];
            this.m_entriesMap.Keys.CopyTo(array, 0);
            this.m_entriesMap.Values.CopyTo(entryArray, 0);
            Array.Sort<Level, LevelMappingEntry>(array, entryArray, 0, array.Length, null);
            Array.Reverse(entryArray, 0, entryArray.Length);
            foreach (LevelMappingEntry entry in entryArray)
            {
                entry.ActivateOptions();
            }
            this.m_entries = entryArray;
        }

        public void Add(LevelMappingEntry entry)
        {
            if (this.m_entriesMap.ContainsKey(entry.Level))
            {
                this.m_entriesMap.Remove(entry.Level);
            }
            this.m_entriesMap.Add(entry.Level, entry);
        }

        public LevelMappingEntry Lookup(Level level)
        {
            if (this.m_entries != null)
            {
                foreach (LevelMappingEntry entry in this.m_entries)
                {
                    if (level >= entry.Level)
                    {
                        return entry;
                    }
                }
            }
            return null;
        }
    }
}

