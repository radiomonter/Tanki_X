namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d37a872c12d608L)]
    public class LocalizedTimerComponent : FromConfigBehaviour, Component
    {
        private string AddLeadingZero(int seconds) => 
            ((seconds >= 10) ? string.Empty : "0") + seconds;

        public string GenerateTimerString(float time)
        {
            TimeSpan span = new TimeSpan(0, 0, 0, (int) time);
            int totalMinutes = (int) span.TotalMinutes;
            if (totalMinutes <= 0)
            {
                return (this.AddLeadingZero(span.Seconds) + this.Second);
            }
            object[] objArray1 = new object[] { totalMinutes, this.Minute, " ", this.AddLeadingZero(span.Seconds), this.Second };
            return string.Concat(objArray1);
        }

        public string Second { get; set; }

        public string Minute { get; set; }

        public override string YamlKey =>
            "localizedText";

        public override string ConfigPath =>
            "ui/element/timer";
    }
}

