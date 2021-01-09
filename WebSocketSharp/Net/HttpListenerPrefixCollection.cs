﻿namespace WebSocketSharp.Net
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class HttpListenerPrefixCollection : ICollection<string>, IEnumerable<string>, IEnumerable
    {
        private HttpListener _listener;
        private List<string> _prefixes;

        internal HttpListenerPrefixCollection(HttpListener listener)
        {
            this._listener = listener;
            this._prefixes = new List<string>();
        }

        public void Add(string uriPrefix)
        {
            this._listener.CheckDisposed();
            HttpListenerPrefix.CheckPrefix(uriPrefix);
            if (!this._prefixes.Contains(uriPrefix))
            {
                this._prefixes.Add(uriPrefix);
                if (this._listener.IsListening)
                {
                    EndPointManager.AddPrefix(uriPrefix, this._listener);
                }
            }
        }

        public void Clear()
        {
            this._listener.CheckDisposed();
            this._prefixes.Clear();
            if (this._listener.IsListening)
            {
                EndPointManager.RemoveListener(this._listener);
            }
        }

        public bool Contains(string uriPrefix)
        {
            this._listener.CheckDisposed();
            if (uriPrefix == null)
            {
                throw new ArgumentNullException("uriPrefix");
            }
            return this._prefixes.Contains(uriPrefix);
        }

        public void CopyTo(Array array, int offset)
        {
            this._listener.CheckDisposed();
            this._prefixes.CopyTo(array, offset);
        }

        public void CopyTo(string[] array, int offset)
        {
            this._listener.CheckDisposed();
            this._prefixes.CopyTo(array, offset);
        }

        public IEnumerator<string> GetEnumerator() => 
            this._prefixes.GetEnumerator();

        public bool Remove(string uriPrefix)
        {
            this._listener.CheckDisposed();
            if (uriPrefix == null)
            {
                throw new ArgumentNullException("uriPrefix");
            }
            bool flag = this._prefixes.Remove(uriPrefix);
            if (flag && this._listener.IsListening)
            {
                EndPointManager.RemovePrefix(uriPrefix, this._listener);
            }
            return flag;
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            this._prefixes.GetEnumerator();

        public int Count =>
            this._prefixes.Count;

        public bool IsReadOnly =>
            false;

        public bool IsSynchronized =>
            false;
    }
}

