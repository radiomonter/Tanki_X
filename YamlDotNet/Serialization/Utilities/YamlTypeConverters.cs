namespace YamlDotNet.Serialization.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.Converters;

    internal static class YamlTypeConverters
    {
        [DebuggerHidden]
        public static IEnumerable<IYamlTypeConverter> GetBuiltInConverters(bool jsonCompatible) => 
            new <GetBuiltInConverters>c__Iterator0 { 
                jsonCompatible = jsonCompatible,
                $PC = -2
            };

        [CompilerGenerated]
        private sealed class <GetBuiltInConverters>c__Iterator0 : IEnumerable, IEnumerable<IYamlTypeConverter>, IEnumerator, IDisposable, IEnumerator<IYamlTypeConverter>
        {
            internal bool jsonCompatible;
            internal IYamlTypeConverter $current;
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
                        this.$current = new GuidConverter(this.jsonCompatible);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$PC = -1;
                        break;

                    default:
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<IYamlTypeConverter> IEnumerable<IYamlTypeConverter>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new YamlTypeConverters.<GetBuiltInConverters>c__Iterator0 { jsonCompatible = this.jsonCompatible };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<YamlDotNet.Serialization.IYamlTypeConverter>.GetEnumerator();

            IYamlTypeConverter IEnumerator<IYamlTypeConverter>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

