namespace WebSocketSharp.Net
{
    using System;
    using System.Security.Principal;

    public class HttpBasicIdentity : GenericIdentity
    {
        private string _password;

        internal HttpBasicIdentity(string username, string password) : base(username, "Basic")
        {
            this._password = password;
        }

        public virtual string Password =>
            this._password;
    }
}

