namespace log4net.Repository.Hierarchy
{
    using System;

    internal sealed class LoggerKey
    {
        private readonly string m_name;
        private readonly int m_hashCache;

        internal LoggerKey(string name)
        {
            this.m_name = string.Intern(name);
            this.m_hashCache = name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            LoggerKey key = obj as LoggerKey;
            return ((key != null) && ReferenceEquals(this.m_name, key.m_name));
        }

        public override int GetHashCode() => 
            this.m_hashCache;
    }
}

