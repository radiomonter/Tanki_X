namespace log4net
{
    using System;
    using System.Runtime.CompilerServices;

    public sealed class AssemblyInfo
    {
        public const string Version = "1.2.13";
        public const decimal TargetFrameworkVersion = 2.0M;
        public const string TargetFramework = "Mono";
        public const bool ClientProfile = false;

        public static string Info =>
            $"Apache log4net version {"1.2.13"} compiled for {"Mono"}{string.Empty} {2.0M}";
    }
}

