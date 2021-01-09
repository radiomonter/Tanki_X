namespace Platform.Library.ClientDataStructures.API
{
    using System;

    public class BitSet : ICloneable
    {
        private const int ADDRESS_BITS_PER_WORD = 6;
        private long[] words = new long[20];
        private int wordsInUse;

        public void Clear()
        {
            while (this.wordsInUse > 0)
            {
                int num;
                this.wordsInUse = num = this.wordsInUse - 1;
                this.words[num] = 0L;
            }
        }

        public virtual unsafe void Clear(int bitIndex)
        {
            int index = WordIndex(bitIndex);
            if (index < this.wordsInUse)
            {
                long* numPtr1 = &(this.words[index]);
                numPtr1[0] &= ~(1L << (bitIndex & 0x3f));
                this.RecalculateWordsInUse();
            }
        }

        public object Clone()
        {
            BitSet set = (BitSet) base.MemberwiseClone();
            set.words = new long[this.words.Length];
            Array.Copy(this.words, set.words, this.words.Length);
            return set;
        }

        private void EnsureCapacity(int wordsRequired)
        {
            if (this.words.Length < wordsRequired)
            {
                int newSize = Math.Max(2 * this.words.Length, wordsRequired);
                Array.Resize<long>(ref this.words, newSize);
            }
        }

        public override bool Equals(object obj)
        {
            if (!ReferenceEquals(obj.GetType(), typeof(BitSet)))
            {
                return false;
            }
            if (!ReferenceEquals(this, obj))
            {
                BitSet set = (BitSet) obj;
                if (this.wordsInUse != set.wordsInUse)
                {
                    return false;
                }
                for (int i = 0; i < this.wordsInUse; i++)
                {
                    if (this.words[i] != set.words[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void ExpandTo(int wordIndex)
        {
            int wordsRequired = wordIndex + 1;
            if (this.wordsInUse < wordsRequired)
            {
                this.EnsureCapacity(wordsRequired);
                this.wordsInUse = wordsRequired;
            }
        }

        public bool Get(int bitIndex)
        {
            int index = WordIndex(bitIndex);
            return ((index < this.wordsInUse) && ((this.words[index] & (1L << (bitIndex & 0x3f))) != 0L));
        }

        public override int GetHashCode()
        {
            long num = 0x4d2L;
            int wordsInUse = this.wordsInUse;
            while (--wordsInUse >= 0)
            {
                num ^= this.words[wordsInUse] * (wordsInUse + 1);
            }
            return (int) ((num >> 0x20) ^ num);
        }

        public bool Mask(BitSet set)
        {
            if (this.wordsInUse < set.wordsInUse)
            {
                return false;
            }
            for (int i = Math.Min(this.wordsInUse, set.wordsInUse) - 1; i >= 0; i--)
            {
                if ((this.words[i] & set.words[i]) != set.words[i])
                {
                    return false;
                }
            }
            return true;
        }

        public virtual bool MaskNot(BitSet set)
        {
            for (int i = Math.Min(this.wordsInUse, set.wordsInUse) - 1; i >= 0; i--)
            {
                if ((~this.words[i] & set.words[i]) != set.words[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void RecalculateWordsInUse()
        {
            int index = this.wordsInUse - 1;
            while ((index >= 0) && (this.words[index] == 0L))
            {
                index--;
            }
            this.wordsInUse = index + 1;
        }

        public unsafe void Set(int bitIndex)
        {
            int wordIndex = WordIndex(bitIndex);
            this.ExpandTo(wordIndex);
            long* numPtr1 = &(this.words[wordIndex]);
            numPtr1[0] |= 1L << (bitIndex & 0x3f);
        }

        private static int WordIndex(int bitIndex) => 
            bitIndex >> 6;
    }
}

