namespace Platform.System.Data.Statics.ClientYaml.Impl
{
    using System;
    using System.Reflection;

    public static class CloneObjectUtil
    {
        public static object CloneObject(object objSource)
        {
            if (objSource == null)
            {
                return null;
            }
            Type objA = objSource.GetType();
            if (objA.IsPrimitive || (objA.IsEnum || ReferenceEquals(objA, typeof(string))))
            {
                return objSource;
            }
            if (objA.IsArray)
            {
                Array array = (Array) objSource;
                Array array2 = Array.CreateInstance(objA.GetElementType(), array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    array2.SetValue(CloneObject(array.GetValue(i)), i);
                }
                return array2;
            }
            if (!objA.IsClass && !objA.IsValueType)
            {
                return null;
            }
            if (!HasDefaultConstructor(objA))
            {
                return objSource;
            }
            object copiedObject = Activator.CreateInstance(objA);
            CopyFields(objA, objSource, copiedObject);
            while (objA.BaseType != null)
            {
                objA = objA.BaseType;
                CopyFields(objA, objSource, copiedObject);
            }
            return copiedObject;
        }

        public static void CopyFields(Type type, object objSource, object copiedObject)
        {
            foreach (FieldInfo info in type.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                object obj2 = info.GetValue(objSource);
                if (obj2 != null)
                {
                    info.SetValue(copiedObject, CloneObject(obj2));
                }
            }
        }

        public static bool HasDefaultConstructor(Type type) => 
            type.IsValueType || !ReferenceEquals(type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null), null);
    }
}

