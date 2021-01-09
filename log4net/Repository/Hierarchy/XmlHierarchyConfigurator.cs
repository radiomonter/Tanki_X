namespace log4net.Repository.Hierarchy
{
    using log4net.Appender;
    using log4net.Core;
    using log4net.ObjectRenderer;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Reflection;
    using System.Security;
    using System.Xml;

    public class XmlHierarchyConfigurator
    {
        private const string CONFIGURATION_TAG = "log4net";
        private const string RENDERER_TAG = "renderer";
        private const string APPENDER_TAG = "appender";
        private const string APPENDER_REF_TAG = "appender-ref";
        private const string PARAM_TAG = "param";
        private const string CATEGORY_TAG = "category";
        private const string PRIORITY_TAG = "priority";
        private const string LOGGER_TAG = "logger";
        private const string NAME_ATTR = "name";
        private const string TYPE_ATTR = "type";
        private const string VALUE_ATTR = "value";
        private const string ROOT_TAG = "root";
        private const string LEVEL_TAG = "level";
        private const string REF_ATTR = "ref";
        private const string ADDITIVITY_ATTR = "additivity";
        private const string THRESHOLD_ATTR = "threshold";
        private const string CONFIG_DEBUG_ATTR = "configDebug";
        private const string INTERNAL_DEBUG_ATTR = "debug";
        private const string EMIT_INTERNAL_DEBUG_ATTR = "emitDebug";
        private const string CONFIG_UPDATE_MODE_ATTR = "update";
        private const string RENDERING_TYPE_ATTR = "renderingClass";
        private const string RENDERED_TYPE_ATTR = "renderedClass";
        private const string INHERITED = "inherited";
        private Hashtable m_appenderBag;
        private readonly log4net.Repository.Hierarchy.Hierarchy m_hierarchy;
        private static readonly Type declaringType = typeof(XmlHierarchyConfigurator);

        public XmlHierarchyConfigurator(log4net.Repository.Hierarchy.Hierarchy hierarchy)
        {
            this.m_hierarchy = hierarchy;
            this.m_appenderBag = new Hashtable();
        }

        public void Configure(XmlElement element)
        {
            if ((element != null) && (this.m_hierarchy != null))
            {
                if (element.LocalName != "log4net")
                {
                    LogLog.Error(declaringType, "Xml element is - not a <log4net> element.");
                }
                else
                {
                    if (!LogLog.EmitInternalMessages)
                    {
                        string argValue = element.GetAttribute("emitDebug");
                        LogLog.Debug(declaringType, "emitDebug attribute [" + argValue + "].");
                        if ((argValue.Length > 0) && (argValue != "null"))
                        {
                            LogLog.EmitInternalMessages = OptionConverter.ToBoolean(argValue, true);
                        }
                        else
                        {
                            LogLog.Debug(declaringType, "Ignoring emitDebug attribute.");
                        }
                    }
                    if (!LogLog.InternalDebugging)
                    {
                        string argValue = element.GetAttribute("debug");
                        LogLog.Debug(declaringType, "debug attribute [" + argValue + "].");
                        if ((argValue.Length > 0) && (argValue != "null"))
                        {
                            LogLog.InternalDebugging = OptionConverter.ToBoolean(argValue, true);
                        }
                        else
                        {
                            LogLog.Debug(declaringType, "Ignoring debug attribute.");
                        }
                        string str4 = element.GetAttribute("configDebug");
                        if ((str4.Length > 0) && (str4 != "null"))
                        {
                            LogLog.Warn(declaringType, "The \"configDebug\" attribute is deprecated.");
                            LogLog.Warn(declaringType, "Use the \"debug\" attribute instead.");
                            LogLog.InternalDebugging = OptionConverter.ToBoolean(str4, true);
                        }
                    }
                    ConfigUpdateMode merge = ConfigUpdateMode.Merge;
                    string attribute = element.GetAttribute("update");
                    if ((attribute != null) && (attribute.Length > 0))
                    {
                        try
                        {
                            merge = (ConfigUpdateMode) OptionConverter.ConvertStringTo(typeof(ConfigUpdateMode), attribute);
                        }
                        catch
                        {
                            LogLog.Error(declaringType, "Invalid update attribute value [" + attribute + "]");
                        }
                    }
                    LogLog.Debug(declaringType, "Configuration update mode [" + merge.ToString() + "].");
                    if (merge == ConfigUpdateMode.Overwrite)
                    {
                        this.m_hierarchy.ResetConfiguration();
                        LogLog.Debug(declaringType, "Configuration reset before reading config.");
                    }
                    IEnumerator enumerator = element.ChildNodes.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            XmlNode current = (XmlNode) enumerator.Current;
                            if (current.NodeType == XmlNodeType.Element)
                            {
                                XmlElement loggerElement = (XmlElement) current;
                                if (loggerElement.LocalName == "logger")
                                {
                                    this.ParseLogger(loggerElement);
                                    continue;
                                }
                                if (loggerElement.LocalName == "category")
                                {
                                    this.ParseLogger(loggerElement);
                                    continue;
                                }
                                if (loggerElement.LocalName == "root")
                                {
                                    this.ParseRoot(loggerElement);
                                    continue;
                                }
                                if (loggerElement.LocalName == "renderer")
                                {
                                    this.ParseRenderer(loggerElement);
                                    continue;
                                }
                                if (loggerElement.LocalName != "appender")
                                {
                                    this.SetParameter(loggerElement, this.m_hierarchy);
                                }
                            }
                        }
                    }
                    finally
                    {
                        IDisposable disposable = enumerator as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }
                    string str6 = element.GetAttribute("threshold");
                    LogLog.Debug(declaringType, "Hierarchy Threshold [" + str6 + "]");
                    if ((str6.Length > 0) && (str6 != "null"))
                    {
                        Level level = (Level) this.ConvertStringTo(typeof(Level), str6);
                        if (level != null)
                        {
                            this.m_hierarchy.Threshold = level;
                        }
                        else
                        {
                            LogLog.Warn(declaringType, "Unable to set hierarchy threshold using value [" + str6 + "] (with acceptable conversion types)");
                        }
                    }
                }
            }
        }

        protected object ConvertStringTo(Type type, string value)
        {
            if (!ReferenceEquals(typeof(Level), type))
            {
                return OptionConverter.ConvertStringTo(type, value);
            }
            Level level = this.m_hierarchy.LevelMap[value];
            if (level == null)
            {
                LogLog.Error(declaringType, "XmlHierarchyConfigurator: Unknown Level Specified [" + value + "]");
            }
            return level;
        }

        private IDictionary CreateCaseInsensitiveWrapper(IDictionary dict)
        {
            if (dict == null)
            {
                return dict;
            }
            Hashtable hashtable = SystemInfo.CreateCaseInsensitiveHashtable();
            IDictionaryEnumerator enumerator = dict.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    hashtable[current.Key] = current.Value;
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            return hashtable;
        }

        protected object CreateObjectFromXml(XmlElement element, Type defaultTargetType, Type typeConstraint)
        {
            Type c = null;
            string attribute = element.GetAttribute("type");
            if ((attribute == null) || (attribute.Length == 0))
            {
                if (defaultTargetType == null)
                {
                    LogLog.Error(declaringType, "Object type not specified. Cannot create object of type [" + typeConstraint.FullName + "]. Missing Value or Type.");
                    return null;
                }
                c = defaultTargetType;
            }
            else
            {
                try
                {
                    c = SystemInfo.GetTypeFromString(attribute, true, true);
                }
                catch (Exception exception)
                {
                    LogLog.Error(declaringType, "Failed to find type [" + attribute + "]", exception);
                    return null;
                }
            }
            bool flag = false;
            if ((typeConstraint != null) && !typeConstraint.IsAssignableFrom(c))
            {
                if (!OptionConverter.CanConvertTypeTo(c, typeConstraint))
                {
                    string[] textArray1 = new string[] { "Object type [", c.FullName, "] is not assignable to type [", typeConstraint.FullName, "]. There are no acceptable type conversions." };
                    LogLog.Error(declaringType, string.Concat(textArray1));
                    return null;
                }
                flag = true;
            }
            object target = null;
            try
            {
                target = Activator.CreateInstance(c);
            }
            catch (Exception exception2)
            {
                LogLog.Error(declaringType, "XmlHierarchyConfigurator: Failed to construct object of type [" + c.FullName + "] Exception: " + exception2.ToString());
            }
            IEnumerator enumerator = element.ChildNodes.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    XmlNode current = (XmlNode) enumerator.Current;
                    if (current.NodeType == XmlNodeType.Element)
                    {
                        this.SetParameter((XmlElement) current, target);
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            IOptionHandler handler = target as IOptionHandler;
            if (handler != null)
            {
                handler.ActivateOptions();
            }
            return (!flag ? target : OptionConverter.ConvertTypeTo(target, typeConstraint));
        }

        protected IAppender FindAppenderByReference(XmlElement appenderRef)
        {
            string attribute = appenderRef.GetAttribute("ref");
            IAppender appender = (IAppender) this.m_appenderBag[attribute];
            if (appender == null)
            {
                XmlElement appenderElement = null;
                if ((attribute != null) && (attribute.Length > 0))
                {
                    IEnumerator enumerator = appenderRef.OwnerDocument.GetElementsByTagName("appender").GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            XmlElement current = (XmlElement) enumerator.Current;
                            if (current.GetAttribute("name") == attribute)
                            {
                                appenderElement = current;
                                break;
                            }
                        }
                    }
                    finally
                    {
                        IDisposable disposable = enumerator as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }
                }
                if (appenderElement == null)
                {
                    LogLog.Error(declaringType, "XmlHierarchyConfigurator: No appender named [" + attribute + "] could be found.");
                    return null;
                }
                appender = this.ParseAppender(appenderElement);
                if (appender != null)
                {
                    this.m_appenderBag[attribute] = appender;
                }
            }
            return appender;
        }

        private MethodInfo FindMethodInfo(Type targetType, string name)
        {
            string strB = name;
            string str2 = "Add" + name;
            foreach (MethodInfo info in targetType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if ((!info.IsStatic && ((string.Compare(info.Name, strB, true, CultureInfo.InvariantCulture) == 0) || (string.Compare(info.Name, str2, true, CultureInfo.InvariantCulture) == 0))) && (info.GetParameters().Length == 1))
                {
                    return info;
                }
            }
            return null;
        }

        private bool HasAttributesOrElements(XmlElement element)
        {
            bool flag;
            IEnumerator enumerator = element.ChildNodes.GetEnumerator();
            try
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        XmlNode current = (XmlNode) enumerator.Current;
                        if ((current.NodeType != XmlNodeType.Attribute) && (current.NodeType != XmlNodeType.Element))
                        {
                            continue;
                        }
                        flag = true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            return flag;
        }

        private static bool IsTypeConstructible(Type type)
        {
            if (!type.IsClass || type.IsAbstract)
            {
                return false;
            }
            ConstructorInfo constructor = type.GetConstructor(new Type[0]);
            return ((constructor != null) && (!constructor.IsAbstract && !constructor.IsPrivate));
        }

        protected IAppender ParseAppender(XmlElement appenderElement)
        {
            string attribute = appenderElement.GetAttribute("name");
            string typeName = appenderElement.GetAttribute("type");
            string[] textArray1 = new string[] { "Loading Appender [", attribute, "] type: [", typeName, "]" };
            LogLog.Debug(declaringType, string.Concat(textArray1));
            try
            {
                IAppender target = (IAppender) Activator.CreateInstance(SystemInfo.GetTypeFromString(typeName, true, true));
                target.Name = attribute;
                IEnumerator enumerator = appenderElement.ChildNodes.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        XmlNode current = (XmlNode) enumerator.Current;
                        if (current.NodeType == XmlNodeType.Element)
                        {
                            XmlElement element = (XmlElement) current;
                            if (element.LocalName != "appender-ref")
                            {
                                this.SetParameter(element, target);
                                continue;
                            }
                            string str3 = element.GetAttribute("ref");
                            IAppenderAttachable attachable = target as IAppenderAttachable;
                            if (attachable == null)
                            {
                                string[] textArray3 = new string[] { "Requesting attachment of appender named [", str3, "] to appender named [", target.Name, "] which does not implement log4net.Core.IAppenderAttachable." };
                                LogLog.Error(declaringType, string.Concat(textArray3));
                                continue;
                            }
                            string[] textArray2 = new string[] { "Attaching appender named [", str3, "] to appender named [", target.Name, "]." };
                            LogLog.Debug(declaringType, string.Concat(textArray2));
                            IAppender appender2 = this.FindAppenderByReference(element);
                            if (appender2 != null)
                            {
                                attachable.AddAppender(appender2);
                            }
                        }
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                IOptionHandler handler = target as IOptionHandler;
                if (handler != null)
                {
                    handler.ActivateOptions();
                }
                LogLog.Debug(declaringType, "Created Appender [" + attribute + "]");
                return target;
            }
            catch (Exception exception)
            {
                string[] textArray4 = new string[] { "Could not create Appender [", attribute, "] of type [", typeName, "]. Reported error follows." };
                LogLog.Error(declaringType, string.Concat(textArray4), exception);
                return null;
            }
        }

        protected void ParseChildrenOfLoggerElement(XmlElement catElement, Logger log, bool isRoot)
        {
            log.RemoveAllAppenders();
            IEnumerator enumerator = catElement.ChildNodes.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    XmlNode current = (XmlNode) enumerator.Current;
                    if (current.NodeType == XmlNodeType.Element)
                    {
                        XmlElement element = (XmlElement) current;
                        if (element.LocalName != "appender-ref")
                        {
                            if ((element.LocalName != "level") && (element.LocalName != "priority"))
                            {
                                this.SetParameter(element, log);
                                continue;
                            }
                            this.ParseLevel(element, log, isRoot);
                            continue;
                        }
                        IAppender newAppender = this.FindAppenderByReference(element);
                        string attribute = element.GetAttribute("ref");
                        if (newAppender == null)
                        {
                            LogLog.Error(declaringType, "Appender named [" + attribute + "] not found.");
                            continue;
                        }
                        string[] textArray1 = new string[] { "Adding appender named [", attribute, "] to logger [", log.Name, "]." };
                        LogLog.Debug(declaringType, string.Concat(textArray1));
                        log.AddAppender(newAppender);
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            IOptionHandler handler = log as IOptionHandler;
            if (handler != null)
            {
                handler.ActivateOptions();
            }
        }

        protected void ParseLevel(XmlElement element, Logger log, bool isRoot)
        {
            string name = log.Name;
            if (isRoot)
            {
                name = "root";
            }
            string attribute = element.GetAttribute("value");
            string[] textArray1 = new string[] { "Logger [", name, "] Level string is [", attribute, "]." };
            LogLog.Debug(declaringType, string.Concat(textArray1));
            if ("inherited" == attribute)
            {
                if (isRoot)
                {
                    LogLog.Error(declaringType, "Root level cannot be inherited. Ignoring directive.");
                }
                else
                {
                    LogLog.Debug(declaringType, "Logger [" + name + "] level set to inherit from parent.");
                    log.Level = null;
                }
            }
            else
            {
                log.Level = log.Hierarchy.LevelMap[attribute];
                if (log.Level == null)
                {
                    string[] textArray2 = new string[] { "Undefined level [", attribute, "] on Logger [", name, "]." };
                    LogLog.Error(declaringType, string.Concat(textArray2));
                }
                else
                {
                    object[] objArray1 = new object[] { "Logger [", name, "] level set to [name=\"", log.Level.Name, "\",value=", log.Level.Value, "]." };
                    LogLog.Debug(declaringType, string.Concat(objArray1));
                }
            }
        }

        protected void ParseLogger(XmlElement loggerElement)
        {
            string attribute = loggerElement.GetAttribute("name");
            LogLog.Debug(declaringType, "Retrieving an instance of log4net.Repository.Logger for logger [" + attribute + "].");
            Logger log = this.m_hierarchy.GetLogger(attribute) as Logger;
            lock (log)
            {
                bool flag = OptionConverter.ToBoolean(loggerElement.GetAttribute("additivity"), true);
                object[] objArray1 = new object[] { "Setting [", log.Name, "] additivity to [", flag, "]." };
                LogLog.Debug(declaringType, string.Concat(objArray1));
                log.Additivity = flag;
                this.ParseChildrenOfLoggerElement(loggerElement, log, false);
            }
        }

        protected void ParseRenderer(XmlElement element)
        {
            string attribute = element.GetAttribute("renderingClass");
            string typeName = element.GetAttribute("renderedClass");
            string[] textArray1 = new string[] { "Rendering class [", attribute, "], Rendered class [", typeName, "]." };
            LogLog.Debug(declaringType, string.Concat(textArray1));
            IObjectRenderer renderer = (IObjectRenderer) OptionConverter.InstantiateByClassName(attribute, typeof(IObjectRenderer), null);
            if (renderer == null)
            {
                LogLog.Error(declaringType, "Could not instantiate renderer [" + attribute + "].");
            }
            else
            {
                try
                {
                    this.m_hierarchy.RendererMap.Put(SystemInfo.GetTypeFromString(typeName, true, true), renderer);
                }
                catch (Exception exception)
                {
                    LogLog.Error(declaringType, "Could not find class [" + typeName + "].", exception);
                }
            }
        }

        protected void ParseRoot(XmlElement rootElement)
        {
            Logger root = this.m_hierarchy.Root;
            lock (root)
            {
                this.ParseChildrenOfLoggerElement(rootElement, root, true);
            }
        }

        protected void SetParameter(XmlElement element, object target)
        {
            string attribute = element.GetAttribute("name");
            if ((element.LocalName != "param") || ((attribute == null) || (attribute.Length == 0)))
            {
                attribute = element.LocalName;
            }
            Type targetType = target.GetType();
            Type objA = null;
            PropertyInfo property = null;
            MethodInfo info2 = null;
            property = targetType.GetProperty(attribute, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if ((property != null) && property.CanWrite)
            {
                objA = property.PropertyType;
            }
            else
            {
                property = null;
                info2 = this.FindMethodInfo(targetType, attribute);
                if (info2 != null)
                {
                    objA = info2.GetParameters()[0].ParameterType;
                }
            }
            if (objA == null)
            {
                string[] textArray1 = new string[] { "XmlHierarchyConfigurator: Cannot find Property [", attribute, "] to set object on [", target.ToString(), "]" };
                LogLog.Error(declaringType, string.Concat(textArray1));
            }
            else
            {
                string str2 = null;
                if (element.GetAttributeNode("value") != null)
                {
                    str2 = element.GetAttribute("value");
                }
                else if (element.HasChildNodes)
                {
                    IEnumerator enumerator = element.ChildNodes.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            XmlNode current = (XmlNode) enumerator.Current;
                            if ((current.NodeType == XmlNodeType.CDATA) || (current.NodeType == XmlNodeType.Text))
                            {
                                str2 = (str2 != null) ? (str2 + current.InnerText) : current.InnerText;
                            }
                        }
                    }
                    finally
                    {
                        IDisposable disposable = enumerator as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }
                }
                if (str2 == null)
                {
                    object obj3 = null;
                    if (ReferenceEquals(objA, typeof(string)) && !this.HasAttributesOrElements(element))
                    {
                        obj3 = string.Empty;
                    }
                    else
                    {
                        Type defaultTargetType = null;
                        if (IsTypeConstructible(objA))
                        {
                            defaultTargetType = objA;
                        }
                        obj3 = this.CreateObjectFromXml(element, defaultTargetType, objA);
                    }
                    if (obj3 == null)
                    {
                        LogLog.Error(declaringType, "Failed to create object to set param: " + attribute);
                    }
                    else if (property != null)
                    {
                        object[] objArray5 = new object[] { "Setting Property [", property.Name, "] to object [", obj3, "]" };
                        LogLog.Debug(declaringType, string.Concat(objArray5));
                        try
                        {
                            property.SetValue(target, obj3, BindingFlags.SetProperty, null, null, CultureInfo.InvariantCulture);
                        }
                        catch (TargetInvocationException exception4)
                        {
                            object[] objArray6 = new object[] { "Failed to set parameter [", property.Name, "] on object [", target, "] using value [", obj3, "]" };
                            LogLog.Error(declaringType, string.Concat(objArray6), exception4.InnerException);
                        }
                    }
                    else if (info2 != null)
                    {
                        object[] objArray7 = new object[] { "Setting Collection Property [", info2.Name, "] to object [", obj3, "]" };
                        LogLog.Debug(declaringType, string.Concat(objArray7));
                        try
                        {
                            object[] parameters = new object[] { obj3 };
                            info2.Invoke(target, BindingFlags.InvokeMethod, null, parameters, CultureInfo.InvariantCulture);
                        }
                        catch (TargetInvocationException exception5)
                        {
                            object[] objArray9 = new object[] { "Failed to set parameter [", info2.Name, "] on object [", target, "] using value [", obj3, "]" };
                            LogLog.Error(declaringType, string.Concat(objArray9), exception5.InnerException);
                        }
                    }
                }
                else
                {
                    try
                    {
                        IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                        if (this.HasCaseInsensitiveEnvironment)
                        {
                            environmentVariables = this.CreateCaseInsensitiveWrapper(environmentVariables);
                        }
                        str2 = OptionConverter.SubstituteVariables(str2, environmentVariables);
                    }
                    catch (SecurityException)
                    {
                        LogLog.Debug(declaringType, "Security exception while trying to expand environment variables. Error Ignored. No Expansion.");
                    }
                    Type type3 = null;
                    string typeName = element.GetAttribute("type");
                    if ((typeName != null) && (typeName.Length > 0))
                    {
                        try
                        {
                            Type c = SystemInfo.GetTypeFromString(typeName, true, true);
                            string[] textArray2 = new string[] { "Parameter [", attribute, "] specified subtype [", c.FullName, "]" };
                            LogLog.Debug(declaringType, string.Concat(textArray2));
                            if (objA.IsAssignableFrom(c))
                            {
                                objA = c;
                            }
                            else if (OptionConverter.CanConvertTypeTo(c, objA))
                            {
                                type3 = objA;
                                objA = c;
                            }
                            else
                            {
                                string[] textArray3 = new string[] { "subtype [", c.FullName, "] set on [", attribute, "] is not a subclass of property type [", objA.FullName, "] and there are no acceptable type conversions." };
                                LogLog.Error(declaringType, string.Concat(textArray3));
                            }
                        }
                        catch (Exception exception)
                        {
                            string[] textArray4 = new string[] { "Failed to find type [", typeName, "] set on [", attribute, "]" };
                            LogLog.Error(declaringType, string.Concat(textArray4), exception);
                        }
                    }
                    object sourceInstance = this.ConvertStringTo(objA, str2);
                    if ((sourceInstance != null) && (type3 != null))
                    {
                        string[] textArray5 = new string[] { "Performing additional conversion of value from [", sourceInstance.GetType().Name, "] to [", type3.Name, "]" };
                        LogLog.Debug(declaringType, string.Concat(textArray5));
                        sourceInstance = OptionConverter.ConvertTypeTo(sourceInstance, type3);
                    }
                    if (sourceInstance == null)
                    {
                        object[] objArray4 = new object[] { "Unable to set property [", attribute, "] on object [", target, "] using value [", str2, "] (with acceptable conversion types)" };
                        LogLog.Warn(declaringType, string.Concat(objArray4));
                    }
                    else if (property != null)
                    {
                        string[] textArray6 = new string[] { "Setting Property [", property.Name, "] to ", sourceInstance.GetType().Name, " value [", sourceInstance.ToString(), "]" };
                        LogLog.Debug(declaringType, string.Concat(textArray6));
                        try
                        {
                            property.SetValue(target, sourceInstance, BindingFlags.SetProperty, null, null, CultureInfo.InvariantCulture);
                        }
                        catch (TargetInvocationException exception2)
                        {
                            object[] objArray1 = new object[] { "Failed to set parameter [", property.Name, "] on object [", target, "] using value [", sourceInstance, "]" };
                            LogLog.Error(declaringType, string.Concat(objArray1), exception2.InnerException);
                        }
                    }
                    else if (info2 != null)
                    {
                        string[] textArray7 = new string[] { "Setting Collection Property [", info2.Name, "] to ", sourceInstance.GetType().Name, " value [", sourceInstance.ToString(), "]" };
                        LogLog.Debug(declaringType, string.Concat(textArray7));
                        try
                        {
                            object[] parameters = new object[] { sourceInstance };
                            info2.Invoke(target, BindingFlags.InvokeMethod, null, parameters, CultureInfo.InvariantCulture);
                        }
                        catch (TargetInvocationException exception3)
                        {
                            object[] objArray3 = new object[] { "Failed to set parameter [", attribute, "] on object [", target, "] using value [", sourceInstance, "]" };
                            LogLog.Error(declaringType, string.Concat(objArray3), exception3.InnerException);
                        }
                    }
                }
            }
        }

        private bool HasCaseInsensitiveEnvironment
        {
            get
            {
                PlatformID platform = Environment.OSVersion.Platform;
                return ((platform != PlatformID.Unix) && (platform != PlatformID.MacOSX));
            }
        }

        private enum ConfigUpdateMode
        {
            Merge,
            Overwrite
        }
    }
}

