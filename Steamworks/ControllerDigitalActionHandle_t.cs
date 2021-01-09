namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct ControllerDigitalActionHandle_t : IEquatable<ControllerDigitalActionHandle_t>, IComparable<ControllerDigitalActionHandle_t>
    {
        public ulong m_ControllerDigitalActionHandle;
        public ControllerDigitalActionHandle_t(ulong value)
        {
            this.m_ControllerDigitalActionHandle = value;
        }

        public override string ToString() => 
            this.m_ControllerDigitalActionHandle.ToString();

        public override bool Equals(object other) => 
            (other is ControllerDigitalActionHandle_t) && (this == ((ControllerDigitalActionHandle_t) other));

        public override int GetHashCode() => 
            this.m_ControllerDigitalActionHandle.GetHashCode();

        public static bool operator ==(ControllerDigitalActionHandle_t x, ControllerDigitalActionHandle_t y) => 
            x.m_ControllerDigitalActionHandle == y.m_ControllerDigitalActionHandle;

        public static bool operator !=(ControllerDigitalActionHandle_t x, ControllerDigitalActionHandle_t y) => 
            !(x == y);

        public static explicit operator ControllerDigitalActionHandle_t(ulong value) => 
            new ControllerDigitalActionHandle_t(value);

        public static explicit operator ulong(ControllerDigitalActionHandle_t that) => 
            that.m_ControllerDigitalActionHandle;

        public bool Equals(ControllerDigitalActionHandle_t other) => 
            this.m_ControllerDigitalActionHandle == other.m_ControllerDigitalActionHandle;

        public int CompareTo(ControllerDigitalActionHandle_t other) => 
            this.m_ControllerDigitalActionHandle.CompareTo(other.m_ControllerDigitalActionHandle);
    }
}

