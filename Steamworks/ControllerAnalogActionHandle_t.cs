namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct ControllerAnalogActionHandle_t : IEquatable<ControllerAnalogActionHandle_t>, IComparable<ControllerAnalogActionHandle_t>
    {
        public ulong m_ControllerAnalogActionHandle;
        public ControllerAnalogActionHandle_t(ulong value)
        {
            this.m_ControllerAnalogActionHandle = value;
        }

        public override string ToString() => 
            this.m_ControllerAnalogActionHandle.ToString();

        public override bool Equals(object other) => 
            (other is ControllerAnalogActionHandle_t) && (this == ((ControllerAnalogActionHandle_t) other));

        public override int GetHashCode() => 
            this.m_ControllerAnalogActionHandle.GetHashCode();

        public static bool operator ==(ControllerAnalogActionHandle_t x, ControllerAnalogActionHandle_t y) => 
            x.m_ControllerAnalogActionHandle == y.m_ControllerAnalogActionHandle;

        public static bool operator !=(ControllerAnalogActionHandle_t x, ControllerAnalogActionHandle_t y) => 
            !(x == y);

        public static explicit operator ControllerAnalogActionHandle_t(ulong value) => 
            new ControllerAnalogActionHandle_t(value);

        public static explicit operator ulong(ControllerAnalogActionHandle_t that) => 
            that.m_ControllerAnalogActionHandle;

        public bool Equals(ControllerAnalogActionHandle_t other) => 
            this.m_ControllerAnalogActionHandle == other.m_ControllerAnalogActionHandle;

        public int CompareTo(ControllerAnalogActionHandle_t other) => 
            this.m_ControllerAnalogActionHandle.CompareTo(other.m_ControllerAnalogActionHandle);
    }
}

