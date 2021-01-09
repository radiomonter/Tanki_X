namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ProtocolImpl : Protocol
    {
        private readonly Dictionary<CodecInfo, Codec> codecRegistry = new Dictionary<CodecInfo, Codec>(new CodecInfoComparer());
        private readonly Dictionary<Type, Codec> hierarchicalCodecRegistry = new Dictionary<Type, Codec>();
        private readonly Dictionary<long, Type> uidByType = new Dictionary<long, Type>();
        private readonly List<CodecFactory> factories = new List<CodecFactory>();

        public ProtocolImpl()
        {
            this.RegisterFactory(new ArrayCodecFactory());
            this.RegisterFactory(new ListCodecFactory());
            this.RegisterFactory(new SetCodecFactory());
            this.RegisterFactory(new DictionaryCodecFactory());
            this.RegisterFactory(new EnumCodecFactory());
            this.RegisterFactory(new VariedCodecFactory());
            this.RegisterFactory(new OptionalTypeCodecFactory());
            this.RegisterFactory(new StructCodecFactory());
            this.RegisterCodecForType<bool>(new BooleanCodec());
            this.RegisterCodecForType<sbyte>(new SByteCodec());
            this.RegisterCodecForType<byte>(new ByteCodec());
            this.RegisterCodecForType<short>(new ShortCodec());
            this.RegisterCodecForType<int>(new IntegerCodec());
            this.RegisterCodecForType<long>(new LongCodec());
            this.RegisterCodecForType<float>(new FloatCodec());
            this.RegisterCodecForType<double>(new DoubleCodec());
            this.RegisterCodecForType<char>(new CharacterCodec());
            this.RegisterCodecForType<string>(new StringCodec());
            this.RegisterCodecForType<DateTime>(new DateTimeCodec());
        }

        private Codec CreateCodecIfNecessary(CodecInfoWithAttributes codecInfoWithAttributes)
        {
            Codec codec;
            CodecInfo info = codecInfoWithAttributes.Info;
            if (ReflectionUtils.IsNullableType(info.type))
            {
                CodecInfo key = new CodecInfo(ReflectionUtils.GetNullableInnerType(info.type), info.optional, info.varied);
                if (this.codecRegistry.TryGetValue(key, out codec))
                {
                    return codec;
                }
            }
            for (int i = 0; i < this.factories.Count; i++)
            {
                codec = this.factories[i].CreateCodec(this, codecInfoWithAttributes);
                if (codec != null)
                {
                    if (info.optional)
                    {
                        codec = new OptionalCodec(codec);
                    }
                    this.RegisterCodec(info, codec);
                    return codec;
                }
            }
            for (Type type2 = info.type.BaseType; type2 != null; type2 = type2.BaseType)
            {
                CodecInfo key = new CodecInfo(type2, info.optional, info.varied);
                if (this.codecRegistry.TryGetValue(key, out codec))
                {
                    this.codecRegistry.Add(info, codec);
                    return codec;
                }
            }
            throw new CodecNotFoundForRequestException(info);
        }

        public void FreeProtocolBuffer(ProtocolBuffer protocolBuffer)
        {
            ClientProtocolInstancesCache.ReleaseProtocolBufferInstance(protocolBuffer);
        }

        private Codec GetCodec(CodecInfo info)
        {
            Codec codec;
            Codec codec2;
            if (this.codecRegistry.TryGetValue(info, out codec))
            {
                return codec;
            }
            using (Dictionary<Type, Codec>.KeyCollection.Enumerator enumerator = this.hierarchicalCodecRegistry.Keys.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        Type current = enumerator.Current;
                        if (!current.IsAssignableFrom(info.type))
                        {
                            continue;
                        }
                        codec2 = this.hierarchicalCodecRegistry[current];
                    }
                    else
                    {
                        CodecInfoWithAttributes codecInfoWithAttributes = new CodecInfoWithAttributes(info);
                        return this.CreateCodecIfNecessary(codecInfoWithAttributes);
                    }
                    break;
                }
            }
            return codec2;
        }

        public Codec GetCodec(CodecInfoWithAttributes infoWithAttributes)
        {
            Codec codec;
            Codec codec2;
            CodecInfo key = infoWithAttributes.Info;
            if (this.codecRegistry.TryGetValue(key, out codec))
            {
                return codec;
            }
            using (Dictionary<Type, Codec>.KeyCollection.Enumerator enumerator = this.hierarchicalCodecRegistry.Keys.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        Type current = enumerator.Current;
                        if (!current.IsAssignableFrom(key.type))
                        {
                            continue;
                        }
                        codec2 = this.hierarchicalCodecRegistry[current];
                    }
                    else
                    {
                        return this.CreateCodecIfNecessary(infoWithAttributes);
                    }
                    break;
                }
            }
            return codec2;
        }

        public Codec GetCodec(long uid)
        {
            Type typeByUid = this.GetTypeByUid(uid);
            return this.GetCodec(typeByUid);
        }

        public Codec GetCodec(Type type)
        {
            CodecInfo info = new CodecInfo(type, false, false);
            return this.GetCodec(info);
        }

        public Type GetTypeByUid(long uid)
        {
            Type type;
            this.uidByType.TryGetValue(uid, out type);
            if (type == null)
            {
                throw new TypeByUidNotFoundException(uid);
            }
            return type;
        }

        public long GetUidByType(Type cl) => 
            SerializationUidUtils.GetUid(cl);

        public ProtocolBuffer NewProtocolBuffer() => 
            ClientProtocolInstancesCache.GetProtocolBufferInstance();

        private void RegisterCodec(CodecInfo info, Codec codec)
        {
            this.codecRegistry.Add(info, codec);
            codec.Init(this);
        }

        public void RegisterCodecForType<T>(Codec codec)
        {
            this.RegisterCodecForType(typeof(T), codec);
        }

        public void RegisterCodecForType(Type type, Codec codec)
        {
            this.RegisterCodec(new CodecInfo(type, false, false), codec);
            this.RegisterCodec(new CodecInfo(type, true, false), new OptionalCodec(codec));
        }

        protected virtual void RegisterFactory(CodecFactory factory)
        {
            this.factories.Add(factory);
        }

        public void RegisterInheritanceCodecForType<T>(Codec codec)
        {
            this.hierarchicalCodecRegistry.Add(typeof(T), codec);
            this.RegisterCodecForType<T>(codec);
        }

        public void RegisterTypeWithSerialUid(Type type)
        {
            long uid = SerializationUidUtils.GetUid(type);
            if (!this.uidByType.ContainsKey(uid))
            {
                this.uidByType.Add(uid, type);
            }
            else
            {
                Type objA = this.uidByType[uid];
                if (!ReferenceEquals(objA, type))
                {
                    throw new TypeWithSameUidAlreadyRegisteredException(uid, objA, type);
                }
            }
        }

        public bool UnwrapPacket(StreamData source, ProtocolBuffer dest) => 
            PacketHelper.UnwrapPacket(source, dest);

        public void WrapPacket(ProtocolBuffer source, StreamData dest)
        {
            PacketHelper.WrapPacket(source, dest);
        }

        [Inject]
        public static Platform.Library.ClientProtocol.API.ClientProtocolInstancesCache ClientProtocolInstancesCache { get; set; }

        public int ServerProtocolVersion { get; set; }

        private class CodecInfoComparer : IEqualityComparer<CodecInfo>
        {
            public bool Equals(CodecInfo x, CodecInfo y) => 
                x.Equals(y);

            public int GetHashCode(CodecInfo obj) => 
                obj.GetHashCode();
        }
    }
}

