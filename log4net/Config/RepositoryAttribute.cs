namespace log4net.Config
{
    using System;

    [Serializable, AttributeUsage(AttributeTargets.Assembly)]
    public class RepositoryAttribute : Attribute
    {
        private string m_name;
        private Type m_repositoryType;

        public RepositoryAttribute()
        {
        }

        public RepositoryAttribute(string name)
        {
            this.m_name = name;
        }

        public string Name
        {
            get => 
                this.m_name;
            set => 
                this.m_name = value;
        }

        public Type RepositoryType
        {
            get => 
                this.m_repositoryType;
            set => 
                this.m_repositoryType = value;
        }
    }
}

