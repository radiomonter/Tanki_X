namespace log4net.Util
{
    using System;
    using System.Collections;

    public sealed class NullDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
    {
        private static readonly NullDictionaryEnumerator s_instance = new NullDictionaryEnumerator();

        private NullDictionaryEnumerator()
        {
        }

        public bool MoveNext() => 
            false;

        public void Reset()
        {
        }

        public static NullDictionaryEnumerator Instance =>
            s_instance;

        public object Current
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public object Key
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public object Value
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public DictionaryEntry Entry
        {
            get
            {
                throw new InvalidOperationException();
            }
        }
    }
}

