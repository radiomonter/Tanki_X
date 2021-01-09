namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using UnityEngine;

    public class KeyboardSettingsUtil
    {
        public static string KeyCodeToString(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.Keypad0:
                    return "Num0";

                case KeyCode.Keypad1:
                    return "Num1";

                case KeyCode.Keypad2:
                    return "Num2";

                case KeyCode.Keypad3:
                    return "Num3";

                case KeyCode.Keypad4:
                    return "Num4";

                case KeyCode.Keypad5:
                    return "Num5";

                case KeyCode.Keypad6:
                    return "Num6";

                case KeyCode.Keypad7:
                    return "Num7";

                case KeyCode.Keypad8:
                    return "Num8";

                case KeyCode.Keypad9:
                    return "Num9";

                case KeyCode.KeypadPeriod:
                    goto TR_0010;

                case KeyCode.KeypadDivide:
                    break;

                case KeyCode.KeypadMultiply:
                    return "*";

                case KeyCode.KeypadMinus:
                    goto TR_000F;

                case KeyCode.KeypadPlus:
                    return "+";

                case KeyCode.UpArrow:
                    return "↑";

                case KeyCode.DownArrow:
                    return "↓";

                case KeyCode.RightArrow:
                    return "→";

                case KeyCode.LeftArrow:
                    return "←";

                case KeyCode.Insert:
                    return "Ins";

                default:
                    switch (keyCode)
                    {
                        case KeyCode.Comma:
                            return ",";

                        case KeyCode.Minus:
                            goto TR_000F;

                        case KeyCode.Period:
                            goto TR_0010;

                        case KeyCode.Slash:
                            break;

                        case KeyCode.Alpha0:
                            return "0";

                        case KeyCode.Alpha1:
                            return "1";

                        case KeyCode.Alpha2:
                            return "2";

                        case KeyCode.Alpha3:
                            return "3";

                        case KeyCode.Alpha4:
                            return "4";

                        case KeyCode.Alpha5:
                            return "5";

                        case KeyCode.Alpha6:
                            return "6";

                        case KeyCode.Alpha7:
                            return "7";

                        case KeyCode.Alpha8:
                            return "8";

                        case KeyCode.Alpha9:
                            return "9";

                        case KeyCode.Semicolon:
                            return ";";

                        case KeyCode.Equals:
                            return "=";

                        default:
                            switch (keyCode)
                            {
                                case KeyCode.Mouse0:
                                    return "LMB";

                                case KeyCode.Mouse1:
                                    return "RMB";

                                case KeyCode.Mouse2:
                                    return "MMB";

                                case KeyCode.Mouse3:
                                    return "MB3";

                                case KeyCode.Mouse4:
                                    return "MB4";

                                case KeyCode.Mouse5:
                                    return "MB5";

                                case KeyCode.Mouse6:
                                    return "MB6";
                            }
                            switch (keyCode)
                            {
                                case KeyCode.LeftBracket:
                                    return "[";

                                case KeyCode.Backslash:
                                    return @"\";

                                case KeyCode.RightBracket:
                                    return "]";

                                case KeyCode.BackQuote:
                                    return "`";
                            }
                            return ((keyCode == KeyCode.None) ? string.Empty : ((keyCode == KeyCode.Quote) ? "'" : ((keyCode == KeyCode.Delete) ? "Del" : keyCode.ToString())));
                    }
                    break;
            }
            return "/";
        TR_000F:
            return "-";
        TR_0010:
            return ".";
        }
    }
}

