namespace YamlDotNet.Serialization.ValueDeserializers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.Utilities;

    public sealed class AliasValueDeserializer : IValueDeserializer
    {
        private readonly IValueDeserializer innerDeserializer;

        public AliasValueDeserializer(IValueDeserializer innerDeserializer)
        {
            if (innerDeserializer == null)
            {
                throw new ArgumentNullException("innerDeserializer");
            }
            this.innerDeserializer = innerDeserializer;
        }

        public object DeserializeValue(EventReader reader, Type expectedType, SerializerState state, IValueDeserializer nestedObjectDeserializer)
        {
            AnchorAlias alias = reader.Allow<AnchorAlias>();
            if (alias != null)
            {
                ValuePromise promise;
                AliasState state2 = state.Get<AliasState>();
                if (!state2.TryGetValue(alias.Value, out promise))
                {
                    promise = new ValuePromise(alias);
                    state2.Add(alias.Value, promise);
                }
                return (!promise.HasValue ? promise : promise.Value);
            }
            string key = null;
            NodeEvent event2 = reader.Peek<NodeEvent>();
            if ((event2 != null) && !string.IsNullOrEmpty(event2.Anchor))
            {
                key = event2.Anchor;
            }
            object obj2 = this.innerDeserializer.DeserializeValue(reader, expectedType, state, nestedObjectDeserializer);
            if (key != null)
            {
                ValuePromise promise2;
                AliasState state3 = state.Get<AliasState>();
                if (!state3.TryGetValue(key, out promise2))
                {
                    state3.Add(key, new ValuePromise(obj2));
                }
                else
                {
                    if (promise2.HasValue)
                    {
                        throw new DuplicateAnchorException(event2.Start, event2.End, $"Anchor '{key}' already defined");
                    }
                    promise2.Value = obj2;
                }
            }
            return obj2;
        }

        private sealed class AliasState : Dictionary<string, AliasValueDeserializer.ValuePromise>, IPostDeserializationCallback
        {
            public void OnDeserialization()
            {
                foreach (AliasValueDeserializer.ValuePromise promise in base.Values)
                {
                    if (!promise.HasValue)
                    {
                        throw new AnchorNotFoundException(promise.Alias.Start, promise.Alias.End, $"Anchor '{promise.Alias.Value}' not found");
                    }
                }
            }
        }

        private sealed class ValuePromise : IValuePromise
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Action<object> ValueAvailable;
            private object value;
            public readonly AnchorAlias Alias;

            public event Action<object> ValueAvailable
            {
                add
                {
                    Action<object> valueAvailable = this.ValueAvailable;
                    while (true)
                    {
                        Action<object> objB = valueAvailable;
                        valueAvailable = Interlocked.CompareExchange<Action<object>>(ref this.ValueAvailable, objB + value, valueAvailable);
                        if (ReferenceEquals(valueAvailable, objB))
                        {
                            return;
                        }
                    }
                }
                remove
                {
                    Action<object> valueAvailable = this.ValueAvailable;
                    while (true)
                    {
                        Action<object> objB = valueAvailable;
                        valueAvailable = Interlocked.CompareExchange<Action<object>>(ref this.ValueAvailable, objB - value, valueAvailable);
                        if (ReferenceEquals(valueAvailable, objB))
                        {
                            return;
                        }
                    }
                }
            }

            public ValuePromise(object value)
            {
                this.HasValue = true;
                this.value = value;
            }

            public ValuePromise(AnchorAlias alias)
            {
                this.Alias = alias;
            }

            public bool HasValue { get; private set; }

            public object Value
            {
                get
                {
                    if (!this.HasValue)
                    {
                        throw new InvalidOperationException("Value not set");
                    }
                    return this.value;
                }
                set
                {
                    if (this.HasValue)
                    {
                        throw new InvalidOperationException("Value already set");
                    }
                    this.HasValue = true;
                    this.value = value;
                    if (this.ValueAvailable != null)
                    {
                        this.ValueAvailable(value);
                    }
                }
            }
        }
    }
}

