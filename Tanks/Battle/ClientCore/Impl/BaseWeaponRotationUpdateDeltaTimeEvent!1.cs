namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using System.Runtime.CompilerServices;

    public abstract class BaseWeaponRotationUpdateDeltaTimeEvent<T> : BaseWeaponRotationUpdateEvent<T> where T: BaseWeaponRotationUpdateDeltaTimeEvent<T>, new()
    {
        protected BaseWeaponRotationUpdateDeltaTimeEvent()
        {
        }

        public static T GetInstance(float dt)
        {
            T instance = BaseWeaponRotationUpdateEvent<T>.Instance;
            instance.DeltaTime = dt;
            return instance;
        }

        public float DeltaTime { get; set; }
    }
}

