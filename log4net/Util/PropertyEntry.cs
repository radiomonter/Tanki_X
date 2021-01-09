namespace log4net.Util
{
    using System;

    public class PropertyEntry
    {
        private string m_key;
        private object m_value;

        public override string ToString()
        {
            object[] objArray1 = new object[] { "PropertyEntry(Key=", this.m_key, ", Value=", this.m_value, ")" };
            return string.Concat(objArray1);
        }

        public string Key
        {
            get => 
                this.m_key;
            set => 
                this.m_key = value;
        }

        public object Value
        {
            get => 
                this.m_value;
            set => 
                this.m_value = value;
        }
    }
}

