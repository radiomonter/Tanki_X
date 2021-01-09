using System;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable, StructLayout(LayoutKind.Sequential)]
public struct P3D_Group
{
    [SerializeField]
    private int index;
    public P3D_Group(int newIndex)
    {
        this.index = (newIndex > 0) ? ((newIndex < 0x1f) ? newIndex : 0x1f) : 0;
    }

    public static implicit operator int(P3D_Group group) => 
        group.index;

    public static implicit operator P3D_Group(int index) => 
        new P3D_Group(index);
}

