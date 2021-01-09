namespace Platform.Library.ClientProtocol.API
{
    using System;

    public interface IOptionalMap
    {
        void Add(bool optional);
        void Clear();
        void Concat(IOptionalMap optionalMap);
        IOptionalMap Duplicate();
        void Fill(byte[] map, int size);
        void Flip();
        bool Get();
        bool GetLast();
        int GetSize();
        bool Has();
    }
}

