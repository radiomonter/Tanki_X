namespace Platform.Library.ClientProtocol.API
{
    using System;

    public static class ReflectionUtils
    {
        public static Type GetNullableInnerType(Type nullableType) => 
            nullableType.GetGenericArguments()[0];

        public static bool IsNullableType(Type type) => 
            type.IsGenericType && ReferenceEquals(type.GetGenericTypeDefinition(), typeof(Nullable<>));
    }
}

