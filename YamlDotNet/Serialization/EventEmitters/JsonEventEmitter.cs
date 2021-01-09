namespace YamlDotNet.Serialization.EventEmitters
{
    using System;
    using System.Globalization;
    using YamlDotNet;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;

    public sealed class JsonEventEmitter : ChainedEventEmitter
    {
        public JsonEventEmitter(IEventEmitter nextEmitter) : base(nextEmitter)
        {
        }

        public override void Emit(AliasEventInfo eventInfo)
        {
            throw new NotSupportedException("Aliases are not supported in JSON");
        }

        public override void Emit(MappingStartEventInfo eventInfo)
        {
            eventInfo.Style = MappingStyle.Flow;
            base.Emit(eventInfo);
        }

        public override void Emit(ScalarEventInfo eventInfo)
        {
            eventInfo.IsPlainImplicit = true;
            eventInfo.Style = ScalarStyle.Plain;
            TypeCode code = (eventInfo.Source.Value == null) ? TypeCode.Empty : eventInfo.Source.Type.GetTypeCode();
            switch (code)
            {
                case TypeCode.Empty:
                    eventInfo.RenderedValue = "null";
                    break;

                case TypeCode.Boolean:
                    eventInfo.RenderedValue = YamlFormatter.FormatBoolean(eventInfo.Source.Value);
                    break;

                case TypeCode.Char:
                case TypeCode.String:
                    eventInfo.RenderedValue = eventInfo.Source.Value.ToString();
                    eventInfo.Style = ScalarStyle.DoubleQuoted;
                    break;

                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    eventInfo.RenderedValue = YamlFormatter.FormatNumber(eventInfo.Source.Value);
                    break;

                case TypeCode.DateTime:
                    eventInfo.RenderedValue = YamlFormatter.FormatDateTime(eventInfo.Source.Value);
                    break;

                default:
                {
                    if (ReferenceEquals(eventInfo.Source.Type, typeof(TimeSpan)))
                    {
                        eventInfo.RenderedValue = YamlFormatter.FormatTimeSpan(eventInfo.Source.Value);
                        break;
                    }
                    object[] args = new object[] { code };
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "TypeCode.{0} is not supported.", args));
                }
            }
            base.Emit(eventInfo);
        }

        public override void Emit(SequenceStartEventInfo eventInfo)
        {
            eventInfo.Style = SequenceStyle.Flow;
            base.Emit(eventInfo);
        }
    }
}

