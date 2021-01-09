namespace log4net.Util
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Threading;

    public sealed class SystemInfo
    {
        private const string DEFAULT_NULL_TEXT = "(null)";
        private const string DEFAULT_NOT_AVAILABLE_TEXT = "NOT AVAILABLE";
        public static readonly Type[] EmptyTypes = new Type[0];
        private static readonly Type declaringType = typeof(SystemInfo);
        private static string s_hostName;
        private static string s_appFriendlyName;
        private static string s_nullText;
        private static string s_notAvailableText;
        private static DateTime s_processStartTime = DateTime.Now;

        static SystemInfo()
        {
            string str = "(null)";
            s_notAvailableText = "NOT AVAILABLE";
            s_nullText = str;
        }

        private SystemInfo()
        {
        }

        public static string AssemblyFileName(Assembly myAssembly) => 
            Path.GetFileName(myAssembly.Location);

        public static string AssemblyLocationInfo(Assembly myAssembly)
        {
            if (myAssembly.GlobalAssemblyCache)
            {
                return "Global Assembly Cache";
            }
            try
            {
                return (!(myAssembly is AssemblyBuilder) ? ((myAssembly.GetType().FullName != "System.Reflection.Emit.InternalAssemblyBuilder") ? myAssembly.Location : "Dynamic Assembly") : "Dynamic Assembly");
            }
            catch (NotSupportedException)
            {
                return "Dynamic Assembly";
            }
            catch (TargetInvocationException exception)
            {
                return ("Location Detect Failed (" + exception.Message + ")");
            }
            catch (ArgumentException exception2)
            {
                return ("Location Detect Failed (" + exception2.Message + ")");
            }
            catch (SecurityException)
            {
                return "Location Permission Denied";
            }
        }

        public static string AssemblyQualifiedName(Type type) => 
            type.FullName + ", " + type.Assembly.FullName;

        public static string AssemblyShortName(Assembly myAssembly)
        {
            string fullName = myAssembly.FullName;
            int index = fullName.IndexOf(',');
            if (index > 0)
            {
                fullName = fullName.Substring(0, index);
            }
            return fullName.Trim();
        }

        public static string ConvertToFullPath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            string localPath = string.Empty;
            try
            {
                string applicationBaseDirectory = ApplicationBaseDirectory;
                if (applicationBaseDirectory != null)
                {
                    Uri uri = new Uri(applicationBaseDirectory);
                    if (uri.IsFile)
                    {
                        localPath = uri.LocalPath;
                    }
                }
            }
            catch
            {
            }
            return (((localPath == null) || (localPath.Length <= 0)) ? Path.GetFullPath(path) : Path.GetFullPath(Path.Combine(localPath, path)));
        }

        public static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(string parameterName, object actualValue, string message) => 
            new ArgumentOutOfRangeException(parameterName, actualValue, message);

        public static Hashtable CreateCaseInsensitiveHashtable() => 
            new Hashtable(StringComparer.OrdinalIgnoreCase);

        public static Type GetTypeFromString(string typeName, bool throwOnError, bool ignoreCase) => 
            GetTypeFromString(Assembly.GetCallingAssembly(), typeName, throwOnError, ignoreCase);

        public static Type GetTypeFromString(Assembly relativeAssembly, string typeName, bool throwOnError, bool ignoreCase)
        {
            if (typeName.IndexOf(',') != -1)
            {
                return Type.GetType(typeName, throwOnError, ignoreCase);
            }
            Type type = relativeAssembly.GetType(typeName, false, ignoreCase);
            if (type != null)
            {
                return type;
            }
            Assembly[] assemblies = null;
            try
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }
            catch (SecurityException)
            {
            }
            if (assemblies != null)
            {
                foreach (Assembly assembly in assemblies)
                {
                    type = assembly.GetType(typeName, false, ignoreCase);
                    if (type != null)
                    {
                        string[] textArray1 = new string[] { "Loaded type [", typeName, "] from assembly [", assembly.FullName, "] by searching loaded assemblies." };
                        LogLog.Debug(declaringType, string.Concat(textArray1));
                        return type;
                    }
                }
            }
            if (!throwOnError)
            {
                return null;
            }
            string[] textArray2 = new string[] { "Could not load type [", typeName, "]. Tried assembly [", relativeAssembly.FullName, "] and all loaded assemblies" };
            throw new TypeLoadException(string.Concat(textArray2));
        }

        public static Type GetTypeFromString(Type relativeType, string typeName, bool throwOnError, bool ignoreCase) => 
            GetTypeFromString(relativeType.Assembly, typeName, throwOnError, ignoreCase);

        public static Guid NewGuid() => 
            Guid.NewGuid();

        public static bool TryParse(string s, out short val)
        {
            val = 0;
            try
            {
                double num;
                if (double.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
                {
                    val = Convert.ToInt16(num);
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public static bool TryParse(string s, out int val)
        {
            val = 0;
            try
            {
                double num;
                if (double.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
                {
                    val = Convert.ToInt32(num);
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public static bool TryParse(string s, out long val)
        {
            val = 0L;
            try
            {
                double num;
                if (double.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
                {
                    val = Convert.ToInt64(num);
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public static string NewLine =>
            Environment.NewLine;

        public static string ApplicationBaseDirectory =>
            AppDomain.CurrentDomain.BaseDirectory;

        public static string ConfigurationFileLocation =>
            AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

        public static string EntryAssemblyLocation =>
            "-";

        public static int CurrentThreadId =>
            Thread.CurrentThread.ManagedThreadId;

        public static string HostName
        {
            get
            {
                if (s_hostName == null)
                {
                    try
                    {
                        s_hostName = Dns.GetHostName();
                    }
                    catch (SocketException)
                    {
                        LogLog.Debug(declaringType, "Socket exception occurred while getting the dns hostname. Error Ignored.");
                    }
                    catch (SecurityException)
                    {
                        LogLog.Debug(declaringType, "Security exception occurred while getting the dns hostname. Error Ignored.");
                    }
                    catch (Exception exception)
                    {
                        LogLog.Debug(declaringType, "Some other exception occurred while getting the dns hostname. Error Ignored.", exception);
                    }
                    if ((s_hostName == null) || (s_hostName.Length == 0))
                    {
                        try
                        {
                            s_hostName = Environment.MachineName;
                        }
                        catch (InvalidOperationException)
                        {
                        }
                        catch (SecurityException)
                        {
                        }
                    }
                    if ((s_hostName == null) || (s_hostName.Length == 0))
                    {
                        s_hostName = s_notAvailableText;
                        LogLog.Debug(declaringType, "Could not determine the hostname. Error Ignored. Empty host name will be used");
                    }
                }
                return s_hostName;
            }
        }

        public static string ApplicationFriendlyName
        {
            get
            {
                if (s_appFriendlyName == null)
                {
                    try
                    {
                        s_appFriendlyName = AppDomain.CurrentDomain.FriendlyName;
                    }
                    catch (SecurityException)
                    {
                        LogLog.Debug(declaringType, "Security exception while trying to get current domain friendly name. Error Ignored.");
                    }
                    if ((s_appFriendlyName == null) || (s_appFriendlyName.Length == 0))
                    {
                        try
                        {
                            s_appFriendlyName = Path.GetFileName(EntryAssemblyLocation);
                        }
                        catch (SecurityException)
                        {
                        }
                    }
                    if ((s_appFriendlyName == null) || (s_appFriendlyName.Length == 0))
                    {
                        s_appFriendlyName = s_notAvailableText;
                    }
                }
                return s_appFriendlyName;
            }
        }

        public static DateTime ProcessStartTime =>
            s_processStartTime;

        public static string NullText
        {
            get => 
                s_nullText;
            set => 
                s_nullText = value;
        }

        public static string NotAvailableText
        {
            get => 
                s_notAvailableText;
            set => 
                s_notAvailableText = value;
        }
    }
}

