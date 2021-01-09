namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using System;
    using System.Collections.Generic;

    public class CommandsCountStatistics
    {
        private float warnLimitTime;
        private const int RANGES = 100;
        private List<KeyValuePair<float, int>> entries = new List<KeyValuePair<float, int>>(100);
        private int newestPos;

        public CommandsCountStatistics(float warnLimitTime)
        {
            this.warnLimitTime = warnLimitTime;
            for (int i = 0; i < 100; i++)
            {
                this.entries.Add(new KeyValuePair<float, int>(0f, 0));
            }
        }

        public void AddCommands(int commandsCount, float time)
        {
            this.ResetOldEntries(time);
            float key = this.GetTime(this.newestPos);
            if (this.IsOldTime(key, time))
            {
                this.entries[this.newestPos] = new KeyValuePair<float, int>(time, commandsCount);
            }
            else
            {
                key = this.entries[this.newestPos].Key;
                int offset = (int) (((time - key) / this.warnLimitTime) * 100f);
                this.newestPos = this.OffsetPos(this.newestPos, offset);
                this.entries[this.newestPos] = new KeyValuePair<float, int>(this.entries[this.newestPos].Key, this.entries[this.newestPos].Value + commandsCount);
            }
        }

        public int GetCommandsInFixedPeriod(float time)
        {
            this.ResetOldEntries(time);
            int newestPos = this.newestPos;
            int num2 = 0;
            int num3 = 0;
            while (!this.IsOldTime(this.GetTime(newestPos), time) && (num3++ < 100))
            {
                num2 += this.entries[newestPos].Value;
                newestPos = this.PrevPos(newestPos);
            }
            return num2;
        }

        private float GetTime(int pos) => 
            this.entries[pos].Key;

        private bool IsOldTime(float time, float nowTime) => 
            (nowTime - this.warnLimitTime) > time;

        private int NextPos(int pos) => 
            this.OffsetPos(pos, 1);

        private int OffsetPos(int pos, int offset) => 
            (pos + offset) % 100;

        private int PrevPos(int pos) => 
            this.OffsetPos(pos, 0x63);

        private void ResetOldEntries(float time)
        {
            int num = 0;
            for (int i = this.NextPos(this.newestPos); this.IsOldTime(this.GetTime(i), time) && (num++ < 100); i = this.NextPos(i))
            {
                this.entries[i] = new KeyValuePair<float, int>(0f, 0);
            }
        }
    }
}

