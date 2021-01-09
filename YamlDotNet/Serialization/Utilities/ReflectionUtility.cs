namespace YamlDotNet.Serialization.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using YamlDotNet;

    internal static class ReflectionUtility
    {
        public static Type GetImplementedGenericInterface(Type type, Type genericInterfaceType)
        {
            Type type3;
            using (IEnumerator<Type> enumerator = GetImplementedInterfaces(type).GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        Type current = enumerator.Current;
                        if (!current.IsGenericType() || !ReferenceEquals(current.GetGenericTypeDefinition(), genericInterfaceType))
                        {
                            continue;
                        }
                        type3 = current;
                    }
                    else
                    {
                        return null;
                    }
                    break;
                }
            }
            return type3;
        }

        [DebuggerHidden]
        public static IEnumerable<Type> GetImplementedInterfaces(Type type) => 
            new <GetImplementedInterfaces>c__Iterator0 { 
                type = type,
                $PC = -2
            };

        [CompilerGenerated]
        private sealed class <GetImplementedInterfaces>c__Iterator0 : IEnumerable, IEnumerable<Type>, IEnumerator, IDisposable, IEnumerator<Type>
        {
            internal Type type;
            internal Type[] $locvar0;
            internal int $locvar1;
            internal Type <implementedInterface>__1;
            internal Type $current;
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
                        if (!this.type.IsInterface())
                        {
                            break;
                        }
                        this.$current = this.type;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto TR_0001;

                    case 1:
                        break;

                    case 2:
                        this.$locvar1++;
                        goto TR_0009;

                    default:
                        goto TR_0000;
                }
                this.$locvar0 = this.type.GetInterfaces();
                this.$locvar1 = 0;
                goto TR_0009;
            TR_0000:
                return false;
            TR_0001:
                return true;
            TR_0009:
                if (this.$locvar1 < this.$locvar0.Length)
                {
                    this.<implementedInterface>__1 = this.$locvar0[this.$locvar1];
                    this.$current = this.<implementedInterface>__1;
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                }
                else
                {
                    this.$PC = -1;
                    goto TR_0000;
                }
                goto TR_0001;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<Type> IEnumerable<Type>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new ReflectionUtility.<GetImplementedInterfaces>c__Iterator0 { type = this.type };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<System.Type>.GetEnumerator();

            Type IEnumerator<Type>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

