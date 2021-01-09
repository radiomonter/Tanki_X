namespace YamlDotNet.Serialization.EventEmitters
{
    using System;
    using System.Globalization;
    using YamlDotNet;
    using YamlDotNet.Core;
    using YamlDotNet.Serialization;

    public sealed class TypeAssigningEventEmitter : ChainedEventEmitter
    {
        private readonly bool _assignTypeWhenDifferent;

        public TypeAssigningEventEmitter(IEventEmitter nextEmitter, bool assignTypeWhenDifferent) : base(nextEmitter)
        {
            this._assignTypeWhenDifferent = assignTypeWhenDifferent;
        }

        private void AssignTypeIfDifferent(ObjectEventInfo eventInfo)
        {
            if (this._assignTypeWhenDifferent && ((eventInfo.Source.Value != null) && !ReferenceEquals(eventInfo.Source.Type, eventInfo.Source.StaticType)))
            {
                eventInfo.Tag = "!" + eventInfo.Source.Type.AssemblyQualifiedName;
            }
        }

        public override void Emit(MappingStartEventInfo eventInfo)
        {
            this.AssignTypeIfDifferent(eventInfo);
            base.Emit(eventInfo);
        }

        public override void Emit(ScalarEventInfo eventInfo)
        {
            ScalarStyle plain = ScalarStyle.Plain;
            TypeCode code = (eventInfo.Source.Value == null) ? TypeCode.Empty : eventInfo.Source.Type.GetTypeCode();
            switch (code)
            {
                case TypeCode.Empty:
                    eventInfo.Tag = "tag:yaml.org,2002:null";
                    eventInfo.RenderedValue = string.Empty;
                    break;

                case TypeCode.Boolean:
                    eventInfo.Tag = "tag:yaml.org,2002:bool";
                    eventInfo.RenderedValue = YamlFormatter.FormatBoolean(eventInfo.Source.Value);
                    break;

                case TypeCode.Char:
                case TypeCode.String:
                    eventInfo.Tag = "tag:yaml.org,2002:str";
                    eventInfo.RenderedValue = eventInfo.Source.Value.ToString();
                    plain = ScalarStyle.Any;
                    break;

                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    eventInfo.Tag = "tag:yaml.org,2002:int";
                    eventInfo.RenderedValue = YamlFormatter.FormatNumber(eventInfo.Source.Value);
                    break;

                case TypeCode.Single:
                    eventInfo.Tag = "tag:yaml.org,2002:float";
                    eventInfo.RenderedValue = YamlFormatter.FormatNumber((float) eventInfo.Source.Value);
                    break;

                case TypeCode.Double:
                    eventInfo.Tag = "tag:yaml.org,2002:float";
                    eventInfo.RenderedValue = YamlFormatter.FormatNumber((double) eventInfo.Source.Value);
                    break;

                case TypeCode.Decimal:
                    eventInfo.Tag = "tag:yaml.org,2002:float";
                    eventInfo.RenderedValue = YamlFormatter.FormatNumber(eventInfo.Source.Value);
                    break;

                case TypeCode.DateTime:
                    eventInfo.Tag = "tag:yaml.org,2002:timestamp";
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
            eventInfo.IsPlainImplicit = true;
            eventInfo.Style ??= plain;
            base.Emit(eventInfo);
        }

        public override void Emit(SequenceStartEventInfo eventInfo)
        {
            this.AssignTypeIfDifferent(eventInfo);
            base.Emit(eventInfo);
        }
    }
}

