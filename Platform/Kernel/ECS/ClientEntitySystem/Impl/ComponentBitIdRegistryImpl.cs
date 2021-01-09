namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ComponentBitIdRegistryImpl : TypeByIdRegistry, ComponentBitIdRegistry, EngineHandlerRegistrationListener
    {
        private static long bitSequence;
        [CompilerGenerated]
        private static Func<Type, long> <>f__mg$cache0;

        public ComponentBitIdRegistryImpl() : this(<>f__mg$cache0)
        {
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Func<Type, long>(ComponentBitIdRegistryImpl.GetNextBitNumber);
            }
        }

        public int GetComponentBitId(Type componentClass) => 
            (int) this.GetId(componentClass);

        private static long GetNextBitNumber(Type clazz) => 
            bitSequence += 1L;

        public void OnHandlerAdded(Handler handler)
        {
            handler.HandlerArgumentsDescription.ComponentClasses.ForEach<Type>(t => this.Register(t));
        }
    }
}

