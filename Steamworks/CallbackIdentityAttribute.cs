namespace Steamworks
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Struct, AllowMultiple=false)]
    internal class CallbackIdentityAttribute : Attribute
    {
        public CallbackIdentityAttribute(int callbackNum)
        {
            this.Identity = callbackNum;
        }

        public int Identity { get; set; }
    }
}

