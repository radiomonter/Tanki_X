namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public abstract class BaseWeaponRotationUpdateEvent<T> : Event where T: BaseWeaponRotationUpdateEvent<T>, new()
    {
        private static T INSTANCE;

        static BaseWeaponRotationUpdateEvent()
        {
            BaseWeaponRotationUpdateEvent<T>.INSTANCE = Activator.CreateInstance<T>();
        }

        protected BaseWeaponRotationUpdateEvent()
        {
        }

        public static T Instance =>
            BaseWeaponRotationUpdateEvent<T>.INSTANCE;
    }
}

