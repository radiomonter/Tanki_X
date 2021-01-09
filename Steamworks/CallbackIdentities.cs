namespace Steamworks
{
    using System;

    internal class CallbackIdentities
    {
        public static int GetCallbackIdentity(Type callbackStruct)
        {
            object[] customAttributes = callbackStruct.GetCustomAttributes(typeof(CallbackIdentityAttribute), false);
            int index = 0;
            if (index >= customAttributes.Length)
            {
                throw new Exception("Callback number not found for struct " + callbackStruct);
            }
            return ((CallbackIdentityAttribute) customAttributes[index]).Identity;
        }
    }
}

