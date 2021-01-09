namespace YamlDotNet.Serialization.NodeDeserializers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using YamlDotNet.Core;
    using YamlDotNet.Serialization;

    public sealed class ArrayNodeDeserializer : INodeDeserializer
    {
        bool INodeDeserializer.Deserialize(EventReader reader, Type expectedType, Func<EventReader, Type, object> nestedObjectDeserializer, out object value)
        {
            if (!expectedType.IsArray)
            {
                value = false;
                return false;
            }
            Type elementType = expectedType.GetElementType();
            ArrayList result = new ArrayList();
            CollectionNodeDeserializer.DeserializeHelper(elementType, reader, expectedType, nestedObjectDeserializer, result, true);
            Array array = Array.CreateInstance(elementType, result.Count);
            result.CopyTo(array, 0);
            value = array;
            return true;
        }

        private sealed class ArrayList : IList, ICollection, IEnumerable
        {
            private object[] data;
            private int count;

            public ArrayList()
            {
                this.Clear();
            }

            public int Add(object value)
            {
                int num;
                if (this.count == this.data.Length)
                {
                    Array.Resize<object>(ref this.data, this.data.Length * 2);
                }
                this.data[this.count] = value;
                this.count = (num = this.count) + 1;
                return num;
            }

            public void Clear()
            {
                this.data = new object[10];
                this.count = 0;
            }

            public bool Contains(object value)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(Array array, int index)
            {
                Array.Copy(this.data, 0, array, index, this.count);
            }

            [DebuggerHidden]
            public IEnumerator GetEnumerator() => 
                new <GetEnumerator>c__Iterator0 { $this = this };

            public int IndexOf(object value)
            {
                throw new NotImplementedException();
            }

            public void Insert(int index, object value)
            {
                throw new NotImplementedException();
            }

            public void Remove(object value)
            {
                throw new NotImplementedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            public bool IsFixedSize =>
                false;

            public bool IsReadOnly =>
                false;

            public object this[int index]
            {
                get => 
                    this.data[index];
                set => 
                    this.data[index] = value;
            }

            public int Count =>
                this.count;

            public bool IsSynchronized =>
                false;

            public object SyncRoot =>
                this.data;

            [CompilerGenerated]
            private sealed class <GetEnumerator>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
            {
                internal int <i>__1;
                internal ArrayNodeDeserializer.ArrayList $this;
                internal object $current;
                internal bool $disposing;
                internal int $PC;

                [DebuggerHidden]
                public void Dispose()
                {
                    this.$disposing = true;
                    this.$PC = -1;
                }

                public bool MoveNext()
                {
                    uint num = (uint) this.$PC;
                    this.$PC = -1;
                    switch (num)
                    {
                        case 0:
                            this.<i>__1 = 0;
                            break;

                        case 1:
                            this.<i>__1++;
                            break;

                        default:
                            goto TR_0000;
                    }
                    if (this.<i>__1 < this.$this.count)
                    {
                        this.$current = this.$this.data[this.<i>__1];
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                    }
                    this.$PC = -1;
                TR_0000:
                    return false;
                }

                [DebuggerHidden]
                public void Reset()
                {
                    throw new NotSupportedException();
                }

                object IEnumerator<object>.Current =>
                    this.$current;

                object IEnumerator.Current =>
                    this.$current;
            }
        }
    }
}

