namespace WebSocketSharp
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;

    public class LogData
    {
        private StackFrame _caller;
        private DateTime _date;
        private LogLevel _level;
        private string _message;

        internal LogData(LogLevel level, StackFrame caller, string message)
        {
            this._level = level;
            this._caller = caller;
            string text1 = message;
            if (message == null)
            {
                string local1 = message;
                text1 = string.Empty;
            }
            this._message = text1;
            this._date = DateTime.Now;
        }

        public override string ToString()
        {
            string str = string.Format("{0}|{1,-5}|", this._date, this._level);
            MethodBase method = this._caller.GetMethod();
            string str2 = $"{str}{method.DeclaringType.Name}.{method.Name}|";
            char[] trimChars = new char[] { '\n' };
            char[] separator = new char[] { '\n' };
            string[] strArray = this._message.Replace("\r\n", "\n").TrimEnd(trimChars).Split(separator);
            if (strArray.Length <= 1)
            {
                return $"{str2}{this._message}";
            }
            StringBuilder builder = new StringBuilder($"{str2}{strArray[0]}
", 0x40);
            string format = $"{{0,{str.Length}}}{{1}}
";
            for (int i = 1; i < strArray.Length; i++)
            {
                builder.AppendFormat(format, string.Empty, strArray[i]);
            }
            builder.Length--;
            return builder.ToString();
        }

        public StackFrame Caller =>
            this._caller;

        public DateTime Date =>
            this._date;

        public LogLevel Level =>
            this._level;

        public string Message =>
            this._message;
    }
}

