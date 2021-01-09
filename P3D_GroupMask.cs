using System;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable, StructLayout(LayoutKind.Sequential)]
public struct P3D_GroupMask
{
    [SerializeField]
    private int mask;
    public P3D_GroupMask(int newMask)
    {
        this.mask = newMask;
    }

    public static implicit operator int(P3D_GroupMask groupMask) => 
        groupMask.mask;

    public static implicit operator P3D_GroupMask(int mask) => 
        new P3D_GroupMask(mask);
}

