namespace Platform.Library.ClientUnityIntegration.API
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct Date : IComparable<Date>
    {
        public Date(float unityTime)
        {
            this = new Date();
            this.UnityTime = unityTime;
        }

        [Inject]
        public static Platform.Library.ClientUnityIntegration.API.UnityTime Time { get; set; }
        public float UnityTime { get; set; }
        public static Date Now =>
            new Date(Time.realtimeSinceStartup);
        public static float FromServerTime(long diffToServer, long serverTime) => 
            ((float) (serverTime - diffToServer)) / 1000f;

        public long ToServerTime(long diffToServer) => 
            ((long) (this.UnityTime * 1000f)) + diffToServer;

        public Date AddSeconds(float seconds) => 
            new Date(this.UnityTime + seconds);

        public Date AddMilliseconds(long milliseconds) => 
            new Date(this.UnityTime + (((float) milliseconds) / 1000f));

        public float GetProgress(Date beginDate, Date endDate) => 
            this.GetProgress(beginDate, (float) (endDate - beginDate));

        public float GetProgress(Date beginDate, float durationSeconds) => 
            Mathf.Clamp01((this.UnityTime - beginDate.UnityTime) / durationSeconds);

        public static Date operator +(Date self, float seconds) => 
            new Date(self.UnityTime + seconds);

        public static Date operator -(Date self, float seconds) => 
            new Date(self.UnityTime - seconds);

        public static float operator -(Date self, Date other) => 
            self.UnityTime - other.UnityTime;

        public static bool operator ==(Date t1, Date t2) => 
            t1.UnityTime == t2.UnityTime;

        public static bool operator !=(Date t1, Date t2) => 
            t1.UnityTime != t2.UnityTime;

        public static bool operator <(Date t1, Date t2) => 
            t1.UnityTime < t2.UnityTime;

        public static bool operator <=(Date t1, Date t2) => 
            t1.UnityTime <= t2.UnityTime;

        public static bool operator >(Date t1, Date t2) => 
            t1.UnityTime > t2.UnityTime;

        public static bool operator >=(Date t1, Date t2) => 
            t1.UnityTime >= t2.UnityTime;

        public override int GetHashCode() => 
            this.UnityTime.GetHashCode();

        public int CompareTo(Date other) => 
            this.UnityTime.CompareTo(other.UnityTime);

        public override string ToString() => 
            this.UnityTime.ToString();
    }
}

