using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

public class SmartConsole : MonoBehaviour
{
    public Font m_font;
    private const float k_animTime = 0.4f;
    private const float k_lineSpace = 0.05f;
    private const int k_historyLines = 120;
    private static Vector3 k_position = new Vector3(0.01f, 0.65f, 0f);
    private static Vector3 k_fullPosition = new Vector3(0.01f, 0.05f, 0f);
    private static Vector3 k_hidePosition = new Vector3(0.01f, 1.1f, 0f);
    private static Vector3 k_scale = new Vector3(0.5f, 0.5f, 1f);
    private static int s_flippy = 0;
    private static bool s_blink = false;
    private static bool s_first = true;
    private const float k_toogleCDTime = 0.35f;
    private static float s_toggleCooldown = 0f;
    private static int s_currentCommandHistoryIndex = 0;
    private static Font s_font = null;
    private static Variable<bool> s_drawFPS = null;
    private static Variable<bool> s_drawFullConsole = null;
    private static Variable<bool> s_consoleLock = null;
    private static Variable<bool> s_logging = null;
    private static GameObject s_fps = null;
    private static GameObject s_textInput = null;
    private static GameObject[] s_historyDisplay = null;
    private static AutoCompleteDictionary<Command> s_commandDictionary = new AutoCompleteDictionary<Command>();
    private static AutoCompleteDictionary<Command> s_variableDictionary = new AutoCompleteDictionary<Command>();
    private static AutoCompleteDictionary<Command> s_masterDictionary = new AutoCompleteDictionary<Command>();
    private static List<string> s_commandHistory = new List<string>();
    private static List<string> s_outputHistory = new List<string>();
    private static string s_lastExceptionCallStack = "(none yet)";
    private static string s_lastErrorCallStack = "(none yet)";
    private static string s_lastWarningCallStack = "(none yet)";
    private static string s_currentInputLine = string.Empty;
    private static float s_visiblityLerp = 0f;
    private static bool s_showConsole = false;
    [CompilerGenerated]
    private static Application.LogCallback <>f__mg$cache0;
    [CompilerGenerated]
    private static ConsoleCommandFunction <>f__mg$cache1;
    [CompilerGenerated]
    private static ConsoleCommandFunction <>f__mg$cache2;
    [CompilerGenerated]
    private static ConsoleCommandFunction <>f__mg$cache3;
    [CompilerGenerated]
    private static ConsoleCommandFunction <>f__mg$cache4;
    [CompilerGenerated]
    private static ConsoleCommandFunction <>f__mg$cache5;
    [CompilerGenerated]
    private static ConsoleCommandFunction <>f__mg$cache6;
    [CompilerGenerated]
    private static ConsoleCommandFunction <>f__mg$cache7;
    [CompilerGenerated]
    private static ConsoleCommandFunction <>f__mg$cache8;
    [CompilerGenerated]
    private static ConsoleCommandFunction <>f__mg$cache9;
    [CompilerGenerated]
    private static ConsoleCommandFunction <>f__mg$cacheA;

    private GameObject AddChildWithComponent<T>(string name) where T: Component
    {
        GameObject obj2 = new GameObject();
        obj2.AddComponent<T>();
        obj2.transform.parent = base.transform;
        obj2.name = name;
        return obj2;
    }

    private GameObject AddChildWithGUIText(string name) => 
        this.AddChildWithComponent<GUIText>(name);

    private static void AutoComplete()
    {
        string[] strArray = CComParameterSplit(s_currentInputLine);
        if (strArray.Length != 0)
        {
            Command command = s_masterDictionary.AutoCompleteLookup(strArray[0]);
            int index = 0;
            while (true)
            {
                index = command.m_name.IndexOf(".", (int) (index + 1));
                if ((index <= 0) || (index >= strArray[0].Length))
                {
                    string name = command.m_name;
                    if (index >= 0)
                    {
                        name = command.m_name.Substring(0, index + 1);
                    }
                    if (name.Length < s_currentInputLine.Length)
                    {
                        if (!AutoCompleteTailString("true") && (AutoCompleteTailString("false") || (!AutoCompleteTailString("True") && (AutoCompleteTailString("False") || (!AutoCompleteTailString("TRUE") && !AutoCompleteTailString("FALSE"))))))
                        {
                        }
                    }
                    else if (name.Length >= s_currentInputLine.Length)
                    {
                        s_currentInputLine = name;
                    }
                    return;
                }
            }
        }
    }

    private static bool AutoCompleteTailString(string tailString)
    {
        for (int i = 1; i < tailString.Length; i++)
        {
            if (s_currentInputLine.EndsWith(" " + tailString.Substring(0, i)))
            {
                s_currentInputLine = s_currentInputLine.Substring(0, s_currentInputLine.Length - 1) + tailString.Substring(i - 1);
                return true;
            }
        }
        return false;
    }

    private void Awake()
    {
        DontDestroyOnLoad(base.gameObject);
        if (this.m_font == null)
        {
            Debug.LogError("SmartConsole requires a font to be set in the inspector");
        }
        Initialise(this);
    }

    public static string[] CComParameterSplit(string parameters)
    {
        char[] separator = new char[] { ' ' };
        return parameters.Split(separator, StringSplitOptions.RemoveEmptyEntries);
    }

    public static string[] CComParameterSplit(string parameters, int requiredParameters)
    {
        string[] strArray = CComParameterSplit(parameters);
        if (strArray.Length < (requiredParameters + 1))
        {
            object[] objArray1 = new object[] { "Error: not enough parameters for command. Expected ", requiredParameters, " found ", strArray.Length - 1 };
            WriteLine(string.Concat(objArray1));
        }
        if (strArray.Length > (requiredParameters + 1))
        {
            int num = (strArray.Length - 1) - requiredParameters;
            WriteLine("Warning: " + num + "additional parameters will be dropped:");
            for (int i = strArray.Length - num; i < strArray.Length; i++)
            {
                WriteLine("\"" + strArray[i] + "\"");
            }
        }
        return strArray;
    }

    public static void Clear()
    {
        s_outputHistory.Clear();
        SetStringsOnHistoryElements();
    }

    private static void Clear(string parameters)
    {
        Clear();
    }

    public static Variable<T> CreateVariable<T>(string name) where T: new() => 
        CreateVariable<T>(name, string.Empty);

    public static Variable<T> CreateVariable<T>(string name, string description) where T: new() => 
        CreateVariable<T>(name, description, Activator.CreateInstance<T>());

    public static Variable<T> CreateVariable<T>(string name, string description, T initialValue) where T: new()
    {
        if (s_variableDictionary.ContainsKey(name))
        {
            Debug.LogError("Tried to add already existing console variable!");
            return null;
        }
        Variable<T> variable = new Variable<T>(name, description, initialValue);
        s_variableDictionary.Add(name, variable);
        s_masterDictionary.Add(name, variable);
        return variable;
    }

    private static string[] CVarParameterSplit(string parameters)
    {
        string[] strArray = CComParameterSplit(parameters);
        if (strArray.Length == 0)
        {
            WriteLine("Error: not enough parameters to set or display the value of a console variable.");
        }
        if (strArray.Length > 2)
        {
            int num = strArray.Length - 3;
            WriteLine("Warning: " + num + "additional parameters will be dropped:");
            for (int i = strArray.Length - num; i < strArray.Length; i++)
            {
                WriteLine("\"" + strArray[i] + "\"");
            }
        }
        return strArray;
    }

    private static string DeNewLine(string message) => 
        message.Replace("\n", " | ");

    public static void DestroyVariable<T>(Variable<T> variable) where T: new()
    {
        s_variableDictionary.Remove(variable.m_name);
        s_masterDictionary.Remove(variable.m_name);
    }

    private static void DumpCallStack(string stackString)
    {
        char[] separator = new char[] { '\r', '\n' };
        string[] strArray = stackString.Split(separator);
        if (strArray.Length != 0)
        {
            int num = 0;
            while ((strArray[(strArray.Length - 1) - num].Length == 0) && (num < strArray.Length))
            {
                num++;
            }
            int num2 = strArray.Length - num;
            for (int i = 0; i < num2; i++)
            {
                WriteLine((i + 1).ToString() + ((i >= 9) ? " " : "  ") + strArray[i]);
            }
        }
    }

    private static void Echo(string parameters)
    {
        string message = string.Empty;
        string[] strArray = CComParameterSplit(parameters);
        for (int i = 1; i < strArray.Length; i++)
        {
            message = message + strArray[i] + " ";
        }
        if (message.EndsWith(" "))
        {
            message.Substring(0, message.Length - 1);
        }
        WriteLine(message);
    }

    private static void ExecuteCurrentLine()
    {
        ExecuteLine(s_currentInputLine);
    }

    public static void ExecuteLine(string inputLine)
    {
        WriteLine(">" + inputLine);
        string[] strArray = CComParameterSplit(inputLine);
        if (strArray.Length > 0)
        {
            if (!s_masterDictionary.ContainsKey(strArray[0]))
            {
                WriteLine("Unrecognised command or variable name: " + strArray[0]);
            }
            else
            {
                s_commandHistory.Add(inputLine);
                s_masterDictionary[strArray[0]].m_callback(inputLine);
            }
        }
    }

    private static void HandleInput()
    {
        s_toggleCooldown += (Time.deltaTime >= 0.0166f) ? Time.deltaTime : 0.0166f;
        if (s_toggleCooldown >= 0.35f)
        {
            bool flag = false;
            if (Input.touchCount > 0)
            {
                flag = IsInputCoordInBounds(Input.touches[0].position);
            }
            else if (Input.GetMouseButton(0))
            {
                flag = IsInputCoordInBounds(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            }
            if (flag || Input.GetKeyUp(KeyCode.BackQuote))
            {
                if (s_consoleLock == null)
                {
                    InputManagerImpl.SUSPENDED = false;
                    s_showConsole = !s_showConsole;
                    if (s_showConsole)
                    {
                        InputManagerImpl.SUSPENDED = true;
                        s_currentInputLine = string.Empty;
                    }
                }
                s_toggleCooldown = 0f;
            }
            if (s_commandHistory.Count > 0)
            {
                bool flag2 = false;
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    flag2 = true;
                    s_currentCommandHistoryIndex--;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    flag2 = true;
                    s_currentCommandHistoryIndex++;
                }
                if (flag2)
                {
                    s_currentCommandHistoryIndex = Mathf.Clamp(s_currentCommandHistoryIndex, 0, s_commandHistory.Count - 1);
                    s_currentInputLine = s_commandHistory[s_currentCommandHistoryIndex];
                }
            }
            HandleTextInput();
        }
    }

    private static void HandleTextInput()
    {
        bool flag = false;
        foreach (char ch in Input.inputString)
        {
            switch (ch)
            {
                case '\b':
                    s_currentInputLine = (s_currentInputLine.Length <= 0) ? string.Empty : s_currentInputLine.Substring(0, s_currentInputLine.Length - 1);
                    break;

                case '\t':
                    AutoComplete();
                    flag = true;
                    break;

                case '\n':
                case '\r':
                    ExecuteCurrentLine();
                    s_currentInputLine = string.Empty;
                    break;

                default:
                    s_currentInputLine = s_currentInputLine + ch;
                    break;
            }
        }
        if (!flag && Input.GetKeyDown(KeyCode.Tab))
        {
            AutoComplete();
        }
    }

    private static void Help(string parameters)
    {
        string str = string.Empty;
        try
        {
            char[] separator = new char[] { ' ' };
            str = parameters.Split(separator)[1];
        }
        catch (Exception)
        {
        }
        foreach (Command command in s_commandDictionary.Values)
        {
            if (string.IsNullOrEmpty(str) || command.m_name.Contains(str))
            {
                string name = command.m_name;
                int length = command.m_name.Length;
                while (true)
                {
                    if (length >= 0x19)
                    {
                        name = (command.m_paramsExample.Length <= 0) ? (name + "          ") : (name + " example: " + command.m_paramsExample);
                        int num2 = command.m_paramsExample.Length;
                        while (true)
                        {
                            if (num2 >= 0x23)
                            {
                                WriteLine(name + command.m_help);
                                break;
                            }
                            name = name + " ";
                            num2++;
                        }
                        break;
                    }
                    name = name + " ";
                    length++;
                }
            }
        }
    }

    private static void Initialise(SmartConsole instance)
    {
        if (s_textInput == null)
        {
            <>f__mg$cache0 ??= new Application.LogCallback(SmartConsole.LogHandler);
            Application.RegisterLogCallback(<>f__mg$cache0);
            InitialiseCommands();
            InitialiseVariables();
            InitialiseUI(instance);
        }
    }

    private static void InitialiseCommands()
    {
        if (<>f__mg$cache1 == null)
        {
            <>f__mg$cache1 = new ConsoleCommandFunction(SmartConsole.Clear);
        }
        RegisterCommand("clear", "clear the console log", <>f__mg$cache1);
        <>f__mg$cache2 ??= new ConsoleCommandFunction(SmartConsole.Clear);
        RegisterCommand("cls", "clear the console log (alias for Clear)", <>f__mg$cache2);
        if (<>f__mg$cache3 == null)
        {
            <>f__mg$cache3 = new ConsoleCommandFunction(SmartConsole.Echo);
        }
        RegisterCommand("writes <string> to the console log (alias for echo)", "echo <string>", "echo", <>f__mg$cache3);
        if (<>f__mg$cache4 == null)
        {
            <>f__mg$cache4 = new ConsoleCommandFunction(SmartConsole.Help);
        }
        RegisterCommand("help", "displays help information for console command where available. Add parameter to search by filter", <>f__mg$cache4);
        <>f__mg$cache5 ??= new ConsoleCommandFunction(SmartConsole.ListCvars);
        RegisterCommand("list", "lists all currently registered console variables", <>f__mg$cache5);
        if (<>f__mg$cache6 == null)
        {
            <>f__mg$cache6 = new ConsoleCommandFunction(SmartConsole.Echo);
        }
        RegisterCommand("writes <string> to the console log", "print <string>", "print", <>f__mg$cache6);
        if (<>f__mg$cache7 == null)
        {
            <>f__mg$cache7 = new ConsoleCommandFunction(SmartConsole.Quit);
        }
        RegisterCommand("quit", "quit the game (not sure this works with iOS/Android)", <>f__mg$cache7);
        <>f__mg$cache8 ??= new ConsoleCommandFunction(SmartConsole.LastWarningCallStack);
        RegisterCommand("callstack.warning", "display the call stack for the last warning message", <>f__mg$cache8);
        <>f__mg$cache9 ??= new ConsoleCommandFunction(SmartConsole.LastErrorCallStack);
        RegisterCommand("callstack.error", "display the call stack for the last error message", <>f__mg$cache9);
        <>f__mg$cacheA ??= new ConsoleCommandFunction(SmartConsole.LastExceptionCallStack);
        RegisterCommand("callstack.exception", "display the call stack for the last exception message", <>f__mg$cacheA);
    }

    private static void InitialiseUI(SmartConsole instance)
    {
        s_font = instance.m_font;
        if (s_font == null)
        {
            Debug.LogError("SmartConsole needs to have a font set on an instance in the editor!");
            s_font = new Font("Arial");
        }
        s_fps = instance.AddChildWithGUIText("FPSCounter");
        s_textInput = instance.AddChildWithGUIText("SmartConsoleInputField");
        s_historyDisplay = new GameObject[120];
        for (int i = 0; i < 120; i++)
        {
            s_historyDisplay[i] = instance.AddChildWithGUIText("SmartConsoleHistoryDisplay" + i);
        }
        instance.Layout();
    }

    private static void InitialiseVariables()
    {
        s_drawFPS = CreateVariable<bool>("show.fps", "whether to draw framerate counter or not", false);
        s_drawFullConsole = CreateVariable<bool>("console.fullscreen", "whether to draw the console over the whole screen or not", false);
        s_consoleLock = CreateVariable<bool>("console.lock", "whether to allow showing/hiding the console", false);
        s_logging = CreateVariable<bool>("console.log", "whether to redirect log to the console", true);
    }

    private static bool IsInputCoordInBounds(Vector2 inputCoordinate) => 
        (inputCoordinate.x < (0.05f * Screen.width)) && (inputCoordinate.y > (0.95f * Screen.height));

    private static void LastErrorCallStack(string parameters)
    {
        DumpCallStack(s_lastErrorCallStack);
    }

    private static void LastExceptionCallStack(string parameters)
    {
        DumpCallStack(s_lastExceptionCallStack);
    }

    private static void LastWarningCallStack(string parameters)
    {
        DumpCallStack(s_lastWarningCallStack);
    }

    private void Layout()
    {
        float y = 0f;
        LayoutTextAtY(s_textInput, y);
        LayoutTextAtY(s_fps, y);
        y += 0.05f;
        for (int i = 0; i < 120; i++)
        {
            LayoutTextAtY(s_historyDisplay[i], y);
            y += 0.05f;
        }
    }

    private static void LayoutTextAtY(GameObject o, float y)
    {
        o.transform.localPosition = new Vector3(0f, y, 0f);
        o.GetComponent<GUIText>().fontStyle = FontStyle.Normal;
        o.GetComponent<GUIText>().font = s_font;
    }

    private static void ListCvars(string parameters)
    {
        foreach (Command command in s_variableDictionary.Values)
        {
            string name = command.m_name;
            int length = command.m_name.Length;
            while (true)
            {
                if (length >= 50)
                {
                    WriteLine(name + command.m_help);
                    break;
                }
                name = name + " ";
                length++;
            }
        }
    }

    private static void LogHandler(string message, string stack, LogType type)
    {
        if (s_logging != null)
        {
            string str = "[Assert]:             ";
            string str2 = "[Debug.LogError]:     ";
            string str3 = "[Debug.LogException]: ";
            string str4 = "[Debug.LogWarning]:   ";
            string str6 = "[Debug.Log]:          ";
            switch (type)
            {
                case LogType.Error:
                    str6 = str2;
                    s_lastErrorCallStack = stack;
                    break;

                case LogType.Assert:
                    str6 = str;
                    break;

                case LogType.Warning:
                    str6 = str4;
                    s_lastWarningCallStack = stack;
                    break;

                case LogType.Exception:
                    str6 = str3;
                    s_lastExceptionCallStack = stack;
                    break;

                default:
                    break;
            }
            WriteLine(str6 + message);
        }
    }

    public static void Print(string message)
    {
        WriteLine(message);
    }

    private static void Quit(string parameters)
    {
        Application.Quit();
    }

    public static void RegisterCommand(string name, ConsoleCommandFunction callback)
    {
        RegisterCommand(name, string.Empty, "(no description)", callback);
    }

    public static void RegisterCommand(string name, string helpDescription, ConsoleCommandFunction callback)
    {
        RegisterCommand(name, string.Empty, helpDescription, callback);
    }

    public static void RegisterCommand(string name, string exampleUsage, string helpDescription, ConsoleCommandFunction callback)
    {
        if (!s_commandDictionary.ContainsKey(name))
        {
            Command command = new Command {
                m_name = name,
                m_paramsExample = exampleUsage,
                m_help = helpDescription,
                m_callback = callback
            };
            s_commandDictionary.Add(name, command);
            s_masterDictionary.Add(name, command);
        }
    }

    public static void RemoveCommandIfExists(string name)
    {
        s_commandDictionary.Remove(name);
        s_masterDictionary.Remove(name);
    }

    private static void SetStringsOnHistoryElements()
    {
        for (int i = 0; i < 120; i++)
        {
            int num2 = (s_outputHistory.Count - 1) - i;
            s_historyDisplay[i].GetComponent<GUIText>().text = (num2 < 0) ? string.Empty : s_outputHistory[(s_outputHistory.Count - 1) - i];
        }
    }

    private static void SetTopDrawOrderOnGUIText(GUIText text)
    {
    }

    private float SmootherStep(float t) => 
        ((((((6f * t) - 15f) * t) + 10f) * t) * t) * t;

    private void Update()
    {
        if (base.gameObject.activeSelf)
        {
            if (s_first)
            {
                if ((s_fps == null) || (s_textInput == null))
                {
                    Debug.LogWarning("Some variables are null that really shouldn't be! Did you make code changes whilst paused? Be aware that such changes are not safe in general!");
                    return;
                }
                SetTopDrawOrderOnGUIText(s_fps.GetComponent<GUIText>());
                SetTopDrawOrderOnGUIText(s_textInput.GetComponent<GUIText>());
                GameObject[] objArray = s_historyDisplay;
                int index = 0;
                while (true)
                {
                    if (index >= objArray.Length)
                    {
                        s_first = false;
                        break;
                    }
                    SetTopDrawOrderOnGUIText(objArray[index].GetComponent<GUIText>());
                    index++;
                }
            }
            HandleInput();
            s_visiblityLerp = !s_showConsole ? (s_visiblityLerp - (Time.deltaTime / 0.4f)) : (s_visiblityLerp + (Time.deltaTime / 0.4f));
            s_visiblityLerp = Mathf.Clamp01(s_visiblityLerp);
            base.transform.position = Vector3.Lerp(k_hidePosition, (s_drawFullConsole == null) ? k_position : k_fullPosition, this.SmootherStep(s_visiblityLerp));
            base.transform.localScale = k_scale;
            if ((s_textInput != null) && (s_textInput.GetComponent<GUIText>() != null))
            {
                s_textInput.GetComponent<GUIText>().text = ">" + s_currentInputLine + (!s_blink ? string.Empty : "_");
            }
            s_flippy++;
            s_flippy &= 7;
            if (s_flippy == 0)
            {
                s_blink = !s_blink;
            }
            if (s_drawFPS == null)
            {
                s_fps.transform.position = new Vector3(1f, 10f, 0f);
            }
            else
            {
                s_fps.GetComponent<GUIText>().text = string.Empty + (1f / Time.deltaTime) + " fps ";
                s_fps.transform.position = new Vector3(0.8f, 1f, 0f);
            }
        }
    }

    public static void WriteLine(string message)
    {
        s_outputHistory.Add(DeNewLine(message));
        s_currentCommandHistoryIndex = s_outputHistory.Count - 1;
        SetStringsOnHistoryElements();
    }

    private class AutoCompleteDictionary<T> : SortedDictionary<string, T>
    {
        private AutoCompleteComparer<T> m_comparer;

        public AutoCompleteDictionary() : base(new AutoCompleteComparer<T>())
        {
            this.m_comparer = base.Comparer as AutoCompleteComparer<T>;
        }

        public T AutoCompleteLookup(string lookupString)
        {
            this.m_comparer.Reset();
            base.ContainsKey(lookupString);
            string str = (this.m_comparer.UpperBound != null) ? this.m_comparer.UpperBound : this.m_comparer.LowerBound;
            return base[str];
        }

        public T LowerBound(string lookupString)
        {
            this.m_comparer.Reset();
            base.ContainsKey(lookupString);
            return base[this.m_comparer.LowerBound];
        }

        public T UpperBound(string lookupString)
        {
            this.m_comparer.Reset();
            base.ContainsKey(lookupString);
            return base[this.m_comparer.UpperBound];
        }

        private class AutoCompleteComparer : IComparer<string>
        {
            private string m_lowerBound;
            private string m_upperBound;

            public int Compare(string x, string y)
            {
                int num = Comparer<string>.Default.Compare(x, y);
                if (num >= 0)
                {
                    this.m_lowerBound = y;
                }
                if (num <= 0)
                {
                    this.m_upperBound = y;
                }
                return num;
            }

            public void Reset()
            {
                this.m_lowerBound = null;
                this.m_upperBound = null;
            }

            public string LowerBound =>
                this.m_lowerBound;

            public string UpperBound =>
                this.m_upperBound;
        }
    }

    public class Command
    {
        public SmartConsole.ConsoleCommandFunction m_callback;
        public string m_name;
        public string m_paramsExample = string.Empty;
        public string m_help = "(no description)";
    }

    public delegate void ConsoleCommandFunction(string parameters);

    public class Variable<T> : SmartConsole.Command where T: new()
    {
        private T m_value;
        [CompilerGenerated]
        private static SmartConsole.ConsoleCommandFunction <>f__mg$cache0;

        public Variable(string name)
        {
            this.Initialise(name, string.Empty, Activator.CreateInstance<T>());
        }

        public Variable(string name, string description)
        {
            this.Initialise(name, description, Activator.CreateInstance<T>());
        }

        public Variable(string name, T initialValue)
        {
            this.Initialise(name, string.Empty, initialValue);
        }

        public Variable(string name, string description, T initalValue)
        {
            this.Initialise(name, description, initalValue);
        }

        private static void CommandFunction(string parameters)
        {
            string[] strArray = SmartConsole.CVarParameterSplit(parameters);
            if ((strArray.Length != 0) && SmartConsole.s_variableDictionary.ContainsKey(strArray[0]))
            {
                SmartConsole.Variable<T> variable = SmartConsole.s_variableDictionary[strArray[0]] as SmartConsole.Variable<T>;
                string str = " is set to ";
                if (strArray.Length == 2)
                {
                    variable.SetFromString(strArray[1]);
                    str = " has been set to ";
                }
                SmartConsole.WriteLine(variable.m_name + str + variable.m_value);
            }
        }

        private void Initialise(string name, string description, T initalValue)
        {
            base.m_name = name;
            base.m_help = description;
            base.m_paramsExample = string.Empty;
            this.m_value = initalValue;
            if (SmartConsole.Variable<T>.<>f__mg$cache0 == null)
            {
                SmartConsole.Variable<T>.<>f__mg$cache0 = new SmartConsole.ConsoleCommandFunction(SmartConsole.Variable<T>.CommandFunction);
            }
            this.m_callback = SmartConsole.Variable<T>.<>f__mg$cache0;
        }

        public static implicit operator T(SmartConsole.Variable<T> var) => 
            var.m_value;

        public void Set(T val)
        {
            this.m_value = val;
        }

        private void SetFromString(string value)
        {
            this.m_value = (T) Convert.ChangeType(value, typeof(T));
        }
    }
}

