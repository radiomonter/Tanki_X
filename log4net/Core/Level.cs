namespace log4net.Core
{
    using System;

    [Serializable]
    public sealed class Level : IComparable
    {
        public static readonly Level Off = new Level(0x7fffffff, "OFF");
        public static readonly Level Log4Net_Debug = new Level(0x1d4c0, "log4net:DEBUG");
        public static readonly Level Emergency = new Level(0x1d4c0, "EMERGENCY");
        public static readonly Level Fatal = new Level(0x1adb0, "FATAL");
        public static readonly Level Alert = new Level(0x186a0, "ALERT");
        public static readonly Level Critical = new Level(0x15f90, "CRITICAL");
        public static readonly Level Severe = new Level(0x13880, "SEVERE");
        public static readonly Level Error = new Level(0x11170, "ERROR");
        public static readonly Level Warn = new Level(0xea60, "WARN");
        public static readonly Level Notice = new Level(0xc350, "NOTICE");
        public static readonly Level Info = new Level(0x9c40, "INFO");
        public static readonly Level Debug = new Level(0x7530, "DEBUG");
        public static readonly Level Fine = new Level(0x7530, "FINE");
        public static readonly Level Trace = new Level(0x4e20, "TRACE");
        public static readonly Level Finer = new Level(0x4e20, "FINER");
        public static readonly Level Verbose = new Level(0x2710, "VERBOSE");
        public static readonly Level Finest = new Level(0x2710, "FINEST");
        public static readonly Level All = new Level(-2147483648, "ALL");
        private readonly int m_levelValue;
        private readonly string m_levelName;
        private readonly string m_levelDisplayName;

        public Level(int level, string levelName) : this(level, levelName, levelName)
        {
        }

        public Level(int level, string levelName, string displayName)
        {
            if (levelName == null)
            {
                throw new ArgumentNullException("levelName");
            }
            if (displayName == null)
            {
                throw new ArgumentNullException("displayName");
            }
            this.m_levelValue = level;
            this.m_levelName = string.Intern(levelName);
            this.m_levelDisplayName = displayName;
        }

        public static int Compare(Level l, Level r) => 
            !ReferenceEquals(l, r) ? (((l != null) || (r != null)) ? ((l != null) ? ((r != null) ? l.m_levelValue.CompareTo(r.m_levelValue) : 1) : -1) : 0) : 0;

        public int CompareTo(object r)
        {
            Level level = r as Level;
            if (level == null)
            {
                throw new ArgumentException("Parameter: r, Value: [" + r + "] is not an instance of Level");
            }
            return Compare(this, level);
        }

        public override bool Equals(object o)
        {
            Level level = o as Level;
            return ((level == null) ? base.Equals(o) : (this.m_levelValue == level.m_levelValue));
        }

        public override int GetHashCode() => 
            this.m_levelValue;

        public static bool operator ==(Level l, Level r) => 
            ((l == null) || (r == null)) ? ReferenceEquals(l, r) : (l.m_levelValue == r.m_levelValue);

        public static bool operator >(Level l, Level r) => 
            l.m_levelValue > r.m_levelValue;

        public static bool operator >=(Level l, Level r) => 
            l.m_levelValue >= r.m_levelValue;

        public static bool operator !=(Level l, Level r) => 
            !(l == r);

        public static bool operator <(Level l, Level r) => 
            l.m_levelValue < r.m_levelValue;

        public static bool operator <=(Level l, Level r) => 
            l.m_levelValue <= r.m_levelValue;

        public override string ToString() => 
            this.m_levelName;

        public string Name =>
            this.m_levelName;

        public int Value =>
            this.m_levelValue;

        public string DisplayName =>
            this.m_levelDisplayName;
    }
}

