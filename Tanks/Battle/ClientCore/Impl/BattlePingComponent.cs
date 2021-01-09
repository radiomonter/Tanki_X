namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class BattlePingComponent : Component
    {
        private long summ;
        private long count;
        private HashSet<int> values = new HashSet<int>();

        public void add(int ping)
        {
            this.summ += ping;
            this.count += 1L;
            this.values.Add(ping);
        }

        public int getAveragePing() => 
            (this.count <= 0L) ? 0 : ((int) (this.summ / this.count));

        public int getMediana()
        {
            int[] array = this.values.ToArray<int>();
            Array.Sort<int>(array);
            return ((array.Length <= 0) ? 0 : array[array.Length / 2]);
        }

        public ScheduleManager PeriodicEventManager { get; set; }

        public float LastPingTime { get; set; }
    }
}

