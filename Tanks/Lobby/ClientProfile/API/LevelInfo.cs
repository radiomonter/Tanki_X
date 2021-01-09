namespace Tanks.Lobby.ClientProfile.API
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct LevelInfo
    {
        public LevelInfo(int level)
        {
            this = new LevelInfo();
            this.Level = level;
        }

        public bool Equals(LevelInfo other) => 
            this.AbsolutExperience == other.AbsolutExperience;

        public override bool Equals(object obj) => 
            !ReferenceEquals(null, obj) ? ((obj is LevelInfo) && this.Equals((LevelInfo) obj)) : false;

        public override int GetHashCode() => 
            this.AbsolutExperience.GetHashCode();

        public int Experience { get; private set; }
        public int Level { get; private set; }
        public int MaxExperience { get; private set; }
        public long AbsolutExperience { get; private set; }
        public bool IsMaxLevel { get; private set; }
        public float Progress =>
            Mathf.Clamp01(((float) this.Experience) / ((float) this.MaxExperience));
        public void ClampExp()
        {
            this.Experience = Mathf.Min(this.MaxExperience, this.Experience);
        }

        public static LevelInfo Get(long absExp, int[] levels)
        {
            int index = Math.Abs((int) (Array.BinarySearch<int>(levels, (int) absExp) + 1));
            bool flag = false;
            if (index >= levels.Length)
            {
                index = levels.Length - 1;
                flag = true;
            }
            int num2 = (index != 0) ? levels[index - 1] : 0;
            int num3 = levels[index];
            return new LevelInfo { 
                Experience = ((int) absExp) - num2,
                Level = !flag ? index : (index + 1),
                MaxExperience = num3 - num2,
                AbsolutExperience = absExp,
                IsMaxLevel = flag
            };
        }

        public static bool operator ==(LevelInfo left, LevelInfo right) => 
            (left.Experience == right.Experience) && (left.Level == right.Level);

        public static bool operator !=(LevelInfo left, LevelInfo right) => 
            (left.Experience != right.Experience) || (left.Level != right.Level);
    }
}

