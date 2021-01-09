namespace Tanks.Battle.ClientCore.Impl
{
    using System;

    public class StatisticCollection
    {
        private readonly int maxValue;
        private int[] valueToCount;
        private int moda = -1;
        private float average = -1f;
        private float standardDeviation = -1f;
        private int totalCount;

        public StatisticCollection(int maxValue)
        {
            this.maxValue = maxValue;
            this.valueToCount = new int[maxValue];
        }

        public unsafe void Add(int value)
        {
            if (value >= this.maxValue)
            {
                value = this.maxValue - 1;
            }
            int* numPtr1 = &(this.valueToCount[value]);
            numPtr1[0]++;
            this.totalCount++;
            this.SetDirty();
        }

        public unsafe void Add(int value, int count)
        {
            if (count > 0)
            {
                if (value >= this.maxValue)
                {
                    value = this.maxValue - 1;
                }
                int* numPtr1 = &(this.valueToCount[value]);
                numPtr1[0] += count;
                this.totalCount += count;
                this.SetDirty();
            }
        }

        public StatisticCollection Clone()
        {
            StatisticCollection statistics = new StatisticCollection(this.maxValue) {
                valueToCount = new int[this.valueToCount.GetLength(0)]
            };
            this.valueToCount.CopyTo(statistics.valueToCount, 0);
            statistics.moda = this.moda;
            statistics.average = this.average;
            statistics.standardDeviation = this.standardDeviation;
            statistics.totalCount = this.totalCount;
            return statistics;
        }

        private void SetDirty()
        {
            this.moda = -1;
            this.average = -1f;
            this.standardDeviation = -1f;
        }

        public int Moda
        {
            get
            {
                if (this.moda == -1)
                {
                    int num = 0;
                    for (int i = 0; i < this.valueToCount.Length; i++)
                    {
                        int num3 = i;
                        int num4 = this.valueToCount[i];
                        if (num4 > num)
                        {
                            num = num4;
                            this.moda = num3;
                        }
                    }
                }
                return this.moda;
            }
        }

        public float Average
        {
            get
            {
                if (this.average.Equals((float) -1f))
                {
                    if (this.totalCount == 0)
                    {
                        return this.average;
                    }
                    int num = 0;
                    for (int i = 0; i < this.valueToCount.Length; i++)
                    {
                        int num3 = i;
                        int num4 = this.valueToCount[i];
                        num += num4 * num3;
                    }
                    this.average = ((float) num) / ((float) this.totalCount);
                }
                return this.average;
            }
        }

        public float StandartDeviation
        {
            get
            {
                if (this.standardDeviation.Equals((float) -1f))
                {
                    if (this.totalCount == 0)
                    {
                        return this.standardDeviation;
                    }
                    float num = 0f;
                    for (int i = 0; i < this.valueToCount.Length; i++)
                    {
                        int num3 = i;
                        int num4 = this.valueToCount[i];
                        num += ((num3 - this.Average) * (num3 - this.Average)) * num4;
                    }
                    this.standardDeviation = (int) Math.Sqrt((double) (num / ((float) this.totalCount)));
                }
                return this.standardDeviation;
            }
        }

        public int TotalCount =>
            this.totalCount;
    }
}

