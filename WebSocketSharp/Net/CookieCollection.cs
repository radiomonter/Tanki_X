namespace WebSocketSharp.Net
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using WebSocketSharp;

    [Serializable]
    public class CookieCollection : ICollection, IEnumerable
    {
        private List<Cookie> _list = new List<Cookie>();
        private object _sync;
        [CompilerGenerated]
        private static Comparison<Cookie> <>f__mg$cache0;
        [CompilerGenerated]
        private static Comparison<Cookie> <>f__mg$cache1;

        public void Add(Cookie cookie)
        {
            if (cookie == null)
            {
                throw new ArgumentNullException("cookie");
            }
            int num = this.searchCookie(cookie);
            if (num == -1)
            {
                this._list.Add(cookie);
            }
            else
            {
                this._list[num] = cookie;
            }
        }

        public void Add(CookieCollection cookies)
        {
            if (cookies == null)
            {
                throw new ArgumentNullException("cookies");
            }
            IEnumerator enumerator = cookies.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Cookie current = (Cookie) enumerator.Current;
                    this.Add(current);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        private static int compareCookieWithinSort(Cookie x, Cookie y) => 
            (x.Name.Length + x.Value.Length) - (y.Name.Length + y.Value.Length);

        private static int compareCookieWithinSorted(Cookie x, Cookie y)
        {
            int num = 0;
            return (((num = x.Version - y.Version) == 0) ? (((num = x.Name.CompareTo(y.Name)) == 0) ? (y.Path.Length - x.Path.Length) : num) : num);
        }

        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", "Less than zero.");
            }
            if (array.Rank > 1)
            {
                throw new ArgumentException("Multidimensional.", "array");
            }
            if ((array.Length - index) < this._list.Count)
            {
                throw new ArgumentException("The number of elements in this collection is greater than the available space of the destination array.");
            }
            if (!array.GetType().GetElementType().IsAssignableFrom(typeof(Cookie)))
            {
                throw new InvalidCastException("The elements in this collection cannot be cast automatically to the type of the destination array.");
            }
            this._list.CopyTo(array, index);
        }

        public void CopyTo(Cookie[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", "Less than zero.");
            }
            if ((array.Length - index) < this._list.Count)
            {
                throw new ArgumentException("The number of elements in this collection is greater than the available space of the destination array.");
            }
            this._list.CopyTo(array, index);
        }

        public IEnumerator GetEnumerator() => 
            this._list.GetEnumerator();

        internal static CookieCollection Parse(string value, bool response) => 
            !response ? parseRequest(value) : parseResponse(value);

        private static CookieCollection parseRequest(string value)
        {
            CookieCollection cookies = new CookieCollection();
            Cookie cookie = null;
            int num = 0;
            string[] strArray = splitCookieHeaderValue(value);
            for (int i = 0; i < strArray.Length; i++)
            {
                string nameAndValue = strArray[i].Trim();
                if (nameAndValue.Length != 0)
                {
                    if (nameAndValue.StartsWith("$version", StringComparison.InvariantCultureIgnoreCase))
                    {
                        num = int.Parse(nameAndValue.GetValue('=', true));
                    }
                    else if (nameAndValue.StartsWith("$path", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (cookie != null)
                        {
                            cookie.Path = nameAndValue.GetValue('=');
                        }
                    }
                    else if (nameAndValue.StartsWith("$domain", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (cookie != null)
                        {
                            cookie.Domain = nameAndValue.GetValue('=');
                        }
                    }
                    else if (nameAndValue.StartsWith("$port", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string str2 = !nameAndValue.Equals("$port", StringComparison.InvariantCultureIgnoreCase) ? nameAndValue.GetValue('=') : "\"\"";
                        if (cookie != null)
                        {
                            cookie.Port = str2;
                        }
                    }
                    else
                    {
                        string str3;
                        if (cookie != null)
                        {
                            cookies.Add(cookie);
                        }
                        string str4 = string.Empty;
                        int index = nameAndValue.IndexOf('=');
                        if (index == -1)
                        {
                            str3 = nameAndValue;
                        }
                        else if (index == (nameAndValue.Length - 1))
                        {
                            char[] trimChars = new char[] { ' ' };
                            str3 = nameAndValue.Substring(0, index).TrimEnd(trimChars);
                        }
                        else
                        {
                            char[] trimChars = new char[] { ' ' };
                            str3 = nameAndValue.Substring(0, index).TrimEnd(trimChars);
                            char[] chArray3 = new char[] { ' ' };
                            str4 = nameAndValue.Substring(index + 1).TrimStart(chArray3);
                        }
                        cookie = new Cookie(str3, str4);
                        if (num != 0)
                        {
                            cookie.Version = num;
                        }
                    }
                }
            }
            if (cookie != null)
            {
                cookies.Add(cookie);
            }
            return cookies;
        }

        private static CookieCollection parseResponse(string value)
        {
            CookieCollection cookies = new CookieCollection();
            Cookie cookie = null;
            string[] strArray = splitCookieHeaderValue(value);
            for (int i = 0; i < strArray.Length; i++)
            {
                string nameAndValue = strArray[i].Trim();
                if (nameAndValue.Length != 0)
                {
                    if (nameAndValue.StartsWith("version", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (cookie != null)
                        {
                            cookie.Version = int.Parse(nameAndValue.GetValue('=', true));
                        }
                    }
                    else if (nameAndValue.StartsWith("expires", StringComparison.InvariantCultureIgnoreCase))
                    {
                        DateTime now;
                        StringBuilder builder = new StringBuilder(nameAndValue.GetValue('='), 0x20);
                        if (i < (strArray.Length - 1))
                        {
                            builder.AppendFormat(", {0}", strArray[++i].Trim());
                        }
                        string[] formats = new string[] { "ddd, dd'-'MMM'-'yyyy HH':'mm':'ss 'GMT'", "r" };
                        if (!DateTime.TryParseExact(builder.ToString(), formats, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out now))
                        {
                            now = DateTime.Now;
                        }
                        if ((cookie != null) && (cookie.Expires == DateTime.MinValue))
                        {
                            cookie.Expires = now.ToLocalTime();
                        }
                    }
                    else if (nameAndValue.StartsWith("max-age", StringComparison.InvariantCultureIgnoreCase))
                    {
                        DateTime time2 = DateTime.Now.AddSeconds((double) int.Parse(nameAndValue.GetValue('=', true)));
                        if (cookie != null)
                        {
                            cookie.Expires = time2;
                        }
                    }
                    else if (nameAndValue.StartsWith("path", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (cookie != null)
                        {
                            cookie.Path = nameAndValue.GetValue('=');
                        }
                    }
                    else if (nameAndValue.StartsWith("domain", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (cookie != null)
                        {
                            cookie.Domain = nameAndValue.GetValue('=');
                        }
                    }
                    else if (nameAndValue.StartsWith("port", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string str2 = !nameAndValue.Equals("port", StringComparison.InvariantCultureIgnoreCase) ? nameAndValue.GetValue('=') : "\"\"";
                        if (cookie != null)
                        {
                            cookie.Port = str2;
                        }
                    }
                    else if (nameAndValue.StartsWith("comment", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (cookie != null)
                        {
                            cookie.Comment = nameAndValue.GetValue('=').UrlDecode();
                        }
                    }
                    else if (nameAndValue.StartsWith("commenturl", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (cookie != null)
                        {
                            cookie.CommentUri = nameAndValue.GetValue('=', true).ToUri();
                        }
                    }
                    else if (nameAndValue.StartsWith("discard", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (cookie != null)
                        {
                            cookie.Discard = true;
                        }
                    }
                    else if (nameAndValue.StartsWith("secure", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (cookie != null)
                        {
                            cookie.Secure = true;
                        }
                    }
                    else if (nameAndValue.StartsWith("httponly", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (cookie != null)
                        {
                            cookie.HttpOnly = true;
                        }
                    }
                    else
                    {
                        string str3;
                        if (cookie != null)
                        {
                            cookies.Add(cookie);
                        }
                        string str4 = string.Empty;
                        int index = nameAndValue.IndexOf('=');
                        if (index == -1)
                        {
                            str3 = nameAndValue;
                        }
                        else if (index == (nameAndValue.Length - 1))
                        {
                            char[] trimChars = new char[] { ' ' };
                            str3 = nameAndValue.Substring(0, index).TrimEnd(trimChars);
                        }
                        else
                        {
                            char[] trimChars = new char[] { ' ' };
                            str3 = nameAndValue.Substring(0, index).TrimEnd(trimChars);
                            char[] chArray3 = new char[] { ' ' };
                            str4 = nameAndValue.Substring(index + 1).TrimStart(chArray3);
                        }
                        cookie = new Cookie(str3, str4);
                    }
                }
            }
            if (cookie != null)
            {
                cookies.Add(cookie);
            }
            return cookies;
        }

        private int searchCookie(Cookie cookie)
        {
            string name = cookie.Name;
            string path = cookie.Path;
            string domain = cookie.Domain;
            int version = cookie.Version;
            for (int i = this._list.Count - 1; i >= 0; i--)
            {
                Cookie cookie2 = this._list[i];
                if (cookie2.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && (cookie2.Path.Equals(path, StringComparison.InvariantCulture) && (cookie2.Domain.Equals(domain, StringComparison.InvariantCultureIgnoreCase) && (cookie2.Version == version))))
                {
                    return i;
                }
            }
            return -1;
        }

        internal void SetOrRemove(Cookie cookie)
        {
            int index = this.searchCookie(cookie);
            if (index == -1)
            {
                if (!cookie.Expired)
                {
                    this._list.Add(cookie);
                }
            }
            else if (!cookie.Expired)
            {
                this._list[index] = cookie;
            }
            else
            {
                this._list.RemoveAt(index);
            }
        }

        internal void SetOrRemove(CookieCollection cookies)
        {
            IEnumerator enumerator = cookies.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Cookie current = (Cookie) enumerator.Current;
                    this.SetOrRemove(current);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        internal void Sort()
        {
            if (this._list.Count > 1)
            {
                if (<>f__mg$cache1 == null)
                {
                    <>f__mg$cache1 = new Comparison<Cookie>(CookieCollection.compareCookieWithinSort);
                }
                this._list.Sort(<>f__mg$cache1);
            }
        }

        private static string[] splitCookieHeaderValue(string value)
        {
            char[] separators = new char[] { ',', ';' };
            return new List<string>(value.SplitHeaderValue(separators)).ToArray();
        }

        internal IList<Cookie> List =>
            this._list;

        internal IEnumerable<Cookie> Sorted
        {
            get
            {
                List<Cookie> list = new List<Cookie>(this._list);
                if (list.Count > 1)
                {
                    if (<>f__mg$cache0 == null)
                    {
                        <>f__mg$cache0 = new Comparison<Cookie>(CookieCollection.compareCookieWithinSorted);
                    }
                    list.Sort(<>f__mg$cache0);
                }
                return list;
            }
        }

        public int Count =>
            this._list.Count;

        public bool IsReadOnly =>
            true;

        public bool IsSynchronized =>
            false;

        public Cookie this[int index]
        {
            get
            {
                if ((index < 0) || (index >= this._list.Count))
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                return this._list[index];
            }
        }

        public Cookie this[string name]
        {
            get
            {
                Cookie cookie2;
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }
                using (IEnumerator<Cookie> enumerator = this.Sorted.GetEnumerator())
                {
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            Cookie current = enumerator.Current;
                            if (!current.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                            {
                                continue;
                            }
                            cookie2 = current;
                        }
                        else
                        {
                            return null;
                        }
                        break;
                    }
                }
                return cookie2;
            }
        }

        public object SyncRoot
        {
            get
            {
                object obj3 = this._sync;
                if (this._sync == null)
                {
                    object local1 = this._sync;
                    obj3 = this._sync = ((ICollection) this._list).SyncRoot;
                }
                return obj3;
            }
        }
    }
}

