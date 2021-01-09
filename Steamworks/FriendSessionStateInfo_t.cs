namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct FriendSessionStateInfo_t
    {
        public uint m_uiOnlineSessionInstances;
        public byte m_uiPublishedToFriendsSessionInstance;
    }
}

