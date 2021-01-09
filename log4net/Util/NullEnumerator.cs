namespace log4net.Util
{
    using System;
    using System.Collections;

    public sealed class NullEnumerator : IEnumerator
    {
        private static readonly NullEnumerator s_instance = new NullEnumerator();

        private NullEnumerator()
        {
        }

        public bool MoveNext() => 
            false;

        public void Reset()
        {
        }

        public static NullEnumerator Instance =>
            s_instance;

        public object Current
        {
            get
            {
                throw new InvalidOperationException();
            }
        }
    }
}

