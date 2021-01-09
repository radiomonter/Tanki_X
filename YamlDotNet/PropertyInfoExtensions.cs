namespace YamlDotNet
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    internal static class PropertyInfoExtensions
    {
        public static object ReadValue(this PropertyInfo property, object target) => 
            property.GetValue(target, BindingFlags.Default, null, null, null);
    }
}

