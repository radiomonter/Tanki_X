namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Collections.Generic;

    public class ComponentRemoveOrderComparer : Comparer<Type>
    {
        public override int Compare(Type x, Type y) => 
            string.Compare(x.Name, y.Name, StringComparison.Ordinal);
    }
}

