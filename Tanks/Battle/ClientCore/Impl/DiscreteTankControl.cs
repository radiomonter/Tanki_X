namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct DiscreteTankControl
    {
        private static readonly int BIT_LEFT;
        private static readonly int BIT_RIGHT;
        private static readonly int BIT_DOWN;
        private static readonly int BIT_UP;
        private static readonly int BIT_WEAPON_LEFT;
        private static readonly int BIT_WEAPON_RIGHT;
        public byte Control { get; set; }
        public int MoveAxis
        {
            get => 
                this.GetBit(BIT_UP) - this.GetBit(BIT_DOWN);
            set => 
                this.SetDiscreteControl(value, BIT_DOWN, BIT_UP);
        }
        public int TurnAxis
        {
            get => 
                this.GetBit(BIT_RIGHT) - this.GetBit(BIT_LEFT);
            set => 
                this.SetDiscreteControl(value, BIT_LEFT, BIT_RIGHT);
        }
        public int WeaponControl
        {
            get => 
                this.GetBit(BIT_WEAPON_RIGHT) - this.GetBit(BIT_WEAPON_LEFT);
            set => 
                this.SetDiscreteControl(value, BIT_WEAPON_LEFT, BIT_WEAPON_RIGHT);
        }
        private int GetBit(int bitNumber) => 
            (this.Control >> (bitNumber & 0x1f)) & 1;

        private void SetBit(int bitNumber, int value)
        {
            int num = ~(1 << (bitNumber & 0x1f));
            this.Control = (byte) ((this.Control & num) | ((value & 1) << (bitNumber & 0x1f)));
        }

        private void SetDiscreteControl(int value, int negativeBit, int positiveBit)
        {
            this.SetBit(negativeBit, 0);
            this.SetBit(positiveBit, 0);
            if (value > 0)
            {
                this.SetBit(positiveBit, 1);
            }
            else if (value < 0)
            {
                this.SetBit(negativeBit, 1);
            }
        }

        static DiscreteTankControl()
        {
            BIT_RIGHT = 1;
            BIT_DOWN = 2;
            BIT_UP = 3;
            BIT_WEAPON_LEFT = 4;
            BIT_WEAPON_RIGHT = 5;
        }
    }
}

