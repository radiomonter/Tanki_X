namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Checker
    {
        public static void RequireNotEmpty(ICollection c)
        {
            if (c.Count != 0)
            {
                throw new EmptyCollectionNotSupportedException();
            }
        }

        public static void RequireOneOnly<T>(ICollection<T> c)
        {
            if (c.Count != 1)
            {
                throw new RequiredOneElementOnlyException();
            }
        }
    }
}

