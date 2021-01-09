namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Collections.Generic;

    public class ClassUtils
    {
        public static IList<Type> GetClasses(Type cls, Type to, IList<Type> classes)
        {
            if (!ReferenceEquals(cls, to))
            {
                classes.Add(cls);
                Type baseType = cls.BaseType;
                while (!ReferenceEquals(baseType, to))
                {
                    classes.Add(baseType);
                    baseType = baseType.BaseType;
                    if (baseType == null)
                    {
                        object[] objArray1 = new object[] { "cls = ", cls, ", to = ", to };
                        throw new ArgumentException(string.Concat(objArray1));
                    }
                }
            }
            return classes;
        }
    }
}

