namespace log4net.Layout
{
    using log4net.Util;
    using System;

    public class DynamicPatternLayout : PatternLayout
    {
        private PatternString m_headerPatternString;
        private PatternString m_footerPatternString;

        public DynamicPatternLayout()
        {
            this.m_headerPatternString = new PatternString(string.Empty);
            this.m_footerPatternString = new PatternString(string.Empty);
        }

        public DynamicPatternLayout(string pattern) : base(pattern)
        {
            this.m_headerPatternString = new PatternString(string.Empty);
            this.m_footerPatternString = new PatternString(string.Empty);
        }

        public override string Header
        {
            get => 
                this.m_headerPatternString.Format();
            set
            {
                base.Header = value;
                this.m_headerPatternString = new PatternString(value);
            }
        }

        public override string Footer
        {
            get => 
                this.m_footerPatternString.Format();
            set
            {
                base.Footer = value;
                this.m_footerPatternString = new PatternString(value);
            }
        }
    }
}

