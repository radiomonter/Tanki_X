namespace YamlDotNet.Serialization.NodeDeserializers
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    using YamlDotNet;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.Utilities;

    public sealed class ScalarNodeDeserializer : INodeDeserializer
    {
        private object DeserializeIntegerHelper(TypeCode typeCode, string value, IFormatProvider formatProvider)
        {
            StringBuilder builder = new StringBuilder();
            int startIndex = 0;
            bool flag = false;
            int fromBase = 0;
            long num3 = 0L;
            if (value[0] == '-')
            {
                startIndex++;
                flag = true;
            }
            else if (value[0] == '+')
            {
                startIndex++;
            }
            if (value[startIndex] != '0')
            {
                char[] separator = new char[] { ':' };
                string[] strArray = value.Substring(startIndex).Split(separator);
                num3 = 0L;
                for (int i = 0; i < strArray.Length; i++)
                {
                    num3 = (num3 * 60) + long.Parse(strArray[i].Replace("_", string.Empty));
                }
            }
            else
            {
                if (startIndex == (value.Length - 1))
                {
                    fromBase = 10;
                    num3 = 0L;
                }
                else
                {
                    startIndex++;
                    if (value[startIndex] == 'b')
                    {
                        fromBase = 2;
                        startIndex++;
                    }
                    else if (value[startIndex] != 'x')
                    {
                        fromBase = 8;
                    }
                    else
                    {
                        fromBase = 0x10;
                        startIndex++;
                    }
                }
                while (true)
                {
                    if (startIndex < value.Length)
                    {
                        if (value[startIndex] != '_')
                        {
                            builder.Append(value[startIndex]);
                        }
                        startIndex++;
                        continue;
                    }
                    switch (fromBase)
                    {
                        case 8:
                            break;

                        case 10:
                            goto TR_000B;

                        default:
                            if (fromBase == 2)
                            {
                                break;
                            }
                            if (fromBase == 0x10)
                            {
                                num3 = long.Parse(builder.ToString(), NumberStyles.HexNumber, YamlFormatter.NumberFormat);
                            }
                            goto TR_000B;
                    }
                    num3 = Convert.ToInt64(builder.ToString(), fromBase);
                    break;
                }
            }
        TR_000B:
            if (flag)
            {
                num3 = -num3;
            }
            switch (typeCode)
            {
                case TypeCode.SByte:
                    return (sbyte) num3;

                case TypeCode.Byte:
                    return (byte) num3;

                case TypeCode.Int16:
                    return (short) num3;

                case TypeCode.UInt16:
                    return (ushort) num3;

                case TypeCode.Int32:
                    return (int) num3;

                case TypeCode.UInt32:
                    return (uint) num3;

                case TypeCode.Int64:
                    return num3;

                case TypeCode.UInt64:
                    return (ulong) num3;
            }
            return num3;
        }

        bool INodeDeserializer.Deserialize(EventReader reader, Type expectedType, Func<EventReader, Type, object> nestedObjectDeserializer, out object value)
        {
            Scalar scalar = reader.Allow<Scalar>();
            if (scalar == null)
            {
                value = null;
                return false;
            }
            if (expectedType.IsEnum())
            {
                value = Enum.Parse(expectedType, scalar.Value, true);
            }
            else
            {
                TypeCode typeCode = expectedType.GetTypeCode();
                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        value = bool.Parse(scalar.Value);
                        break;

                    case TypeCode.Char:
                        value = scalar.Value[0];
                        break;

                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        value = this.DeserializeIntegerHelper(typeCode, scalar.Value, YamlFormatter.NumberFormat);
                        break;

                    case TypeCode.Single:
                        value = float.Parse(scalar.Value, YamlFormatter.NumberFormat);
                        break;

                    case TypeCode.Double:
                        value = double.Parse(scalar.Value, YamlFormatter.NumberFormat);
                        break;

                    case TypeCode.Decimal:
                        value = decimal.Parse(scalar.Value, YamlFormatter.NumberFormat);
                        break;

                    case TypeCode.DateTime:
                        value = DateTime.Parse(scalar.Value, CultureInfo.InvariantCulture);
                        break;

                    case TypeCode.String:
                        value = scalar.Value;
                        break;

                    default:
                        value = !ReferenceEquals(expectedType, typeof(object)) ? TypeConverter.ChangeType(scalar.Value, expectedType) : scalar.Value;
                        break;
                }
            }
            return true;
        }
    }
}

