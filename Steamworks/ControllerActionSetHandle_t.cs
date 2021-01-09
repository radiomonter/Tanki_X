namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct ControllerActionSetHandle_t : IEquatable<ControllerActionSetHandle_t>, IComparable<ControllerActionSetHandle_t>
    {
        public ulong m_ControllerActionSetHandle;
        public ControllerActionSetHandle_t(ulong value)
        {
            this.m_ControllerActionSetHandle = value;
        }

        public override string ToString() => 
            this.m_ControllerActionSetHandle.ToString();

        public override bool Equals(object other) => 
            (other is ControllerActionSetHandle_t) && (this == ((ControllerActionSetHandle_t) other));

        public override int GetHashCode() => 
            this.m_ControllerActionSetHandle.GetHashCode();

        public static bool operator ==(ControllerActionSetHandle_t x, ControllerActionSetHandle_t y) => 
            x.m_ControllerActionSetHandle == y.m_ControllerActionSetHandle;

        public static bool operator !=(ControllerActionSetHandle_t x, ControllerActionSetHandle_t y) => 
            !(x == y);

        public static explicit operator ControllerActionSetHandle_t(ulong value) => 
            new ControllerActionSetHandle_t(value);

        public static explicit operator ulong(ControllerActionSetHandle_t that) => 
            that.m_ControllerActionSetHandle;

        public bool Equals(ControllerActionSetHandle_t other) => 
            this.m_ControllerActionSetHandle == other.m_ControllerActionSetHandle;

        public int CompareTo(ControllerActionSetHandle_t other) => 
            this.m_ControllerActionSetHandle.CompareTo(other.m_ControllerActionSetHandle);
    }
}

