namespace Edelweiss.DecalSystem
{
    using System;
    using System.Collections.Generic;

    internal class RemovedIndices
    {
        private List<int> m_RemovedIndices = new List<int>();
        private int m_GreatestUnremovedValue = -1;

        public void AddRemovedIndex(int a_Index)
        {
            if (a_Index < this.Count)
            {
                this.m_RemovedIndices[a_Index] = -1;
                this.m_GreatestUnremovedValue = -1;
                for (int i = a_Index + 1; i < this.Count; i++)
                {
                    int num2 = this.m_RemovedIndices[i] - 1;
                    if (num2 >= 0)
                    {
                        this.m_GreatestUnremovedValue = num2;
                    }
                    this.m_RemovedIndices[i] = num2;
                }
            }
            else
            {
                int item = this.m_GreatestUnremovedValue + 1;
                int count = this.Count;
                while (true)
                {
                    if (count >= a_Index)
                    {
                        this.m_RemovedIndices.Add(-1);
                        break;
                    }
                    this.m_RemovedIndices.Add(item);
                    this.m_GreatestUnremovedValue = item;
                    item++;
                    count++;
                }
            }
        }

        public int AdjustedIndex(int a_Index) => 
            (a_Index < this.Count) ? this.m_RemovedIndices[a_Index] : (((this.m_GreatestUnremovedValue + a_Index) - this.Count) + 1);

        public void Clear()
        {
            this.m_RemovedIndices.Clear();
            this.m_GreatestUnremovedValue = -1;
        }

        public bool IsRemovedIndex(int a_Index) => 
            (a_Index < this.Count) ? (this.m_RemovedIndices[a_Index] < 0) : false;

        public int Count =>
            this.m_RemovedIndices.Count;
    }
}

