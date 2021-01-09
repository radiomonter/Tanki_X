namespace WebSocketSharp.Net
{
    using System;

    public class NetworkCredential
    {
        private string _domain;
        private string _password;
        private string[] _roles;
        private string _userName;

        public NetworkCredential(string userName, string password) : this(userName, password, null, null)
        {
        }

        public NetworkCredential(string userName, string password, string domain, params string[] roles)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }
            if (userName.Length == 0)
            {
                throw new ArgumentException("An empty string.", "userName");
            }
            this._userName = userName;
            this._password = password;
            this._domain = domain;
            this._roles = roles;
        }

        public string Domain
        {
            get => 
                this._domain ?? string.Empty;
            internal set => 
                this._domain = value;
        }

        public string Password
        {
            get => 
                this._password ?? string.Empty;
            internal set => 
                this._password = value;
        }

        public string[] Roles
        {
            get
            {
                string[] textArray2 = this._roles;
                if (this._roles == null)
                {
                    string[] local1 = this._roles;
                    textArray2 = this._roles = new string[0];
                }
                return textArray2;
            }
            internal set => 
                this._roles = value;
        }

        public string UserName
        {
            get => 
                this._userName;
            internal set => 
                this._userName = value;
        }
    }
}

