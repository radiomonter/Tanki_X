namespace log4net.Config
{
    using System;

    [Serializable, AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true)]
    public class AliasRepositoryAttribute : Attribute
    {
        private string m_name;

        public AliasRepositoryAttribute(string name)
        {
            this.Name = name;
        }

        public string Name
        {
            get => 
                this.m_name;
            set => 
                this.m_name = value;
        }
    }
}

