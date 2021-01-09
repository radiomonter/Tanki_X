namespace log4net.Util
{
    using System;

    public class FormattingInfo
    {
        private int m_min;
        private int m_max;
        private bool m_leftAlign;

        public FormattingInfo()
        {
            this.m_min = -1;
            this.m_max = 0x7fffffff;
        }

        public FormattingInfo(int min, int max, bool leftAlign)
        {
            this.m_min = -1;
            this.m_max = 0x7fffffff;
            this.m_min = min;
            this.m_max = max;
            this.m_leftAlign = leftAlign;
        }

        public int Min
        {
            get => 
                this.m_min;
            set => 
                this.m_min = value;
        }

        public int Max
        {
            get => 
                this.m_max;
            set => 
                this.m_max = value;
        }

        public bool LeftAlign
        {
            get => 
                this.m_leftAlign;
            set => 
                this.m_leftAlign = value;
        }
    }
}

