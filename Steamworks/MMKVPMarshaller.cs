﻿namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public class MMKVPMarshaller
    {
        private IntPtr m_pNativeArray;
        private IntPtr m_pArrayEntries;

        public MMKVPMarshaller(MatchMakingKeyValuePair_t[] filters)
        {
            if (filters != null)
            {
                int num = Marshal.SizeOf(typeof(MatchMakingKeyValuePair_t));
                this.m_pNativeArray = Marshal.AllocHGlobal((int) (Marshal.SizeOf(typeof(IntPtr)) * filters.Length));
                this.m_pArrayEntries = Marshal.AllocHGlobal((int) (num * filters.Length));
                for (int i = 0; i < filters.Length; i++)
                {
                    Marshal.StructureToPtr(filters[i], new IntPtr(this.m_pArrayEntries.ToInt64() + (i * num)), false);
                }
                Marshal.WriteIntPtr(this.m_pNativeArray, this.m_pArrayEntries);
            }
        }

        ~MMKVPMarshaller()
        {
            if (this.m_pArrayEntries != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.m_pArrayEntries);
            }
            if (this.m_pNativeArray != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.m_pNativeArray);
            }
        }

        public static implicit operator IntPtr(MMKVPMarshaller that) => 
            that.m_pNativeArray;
    }
}

