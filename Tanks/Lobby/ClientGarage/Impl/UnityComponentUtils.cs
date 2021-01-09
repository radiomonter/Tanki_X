namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class UnityComponentUtils
    {
        public static T GetComponentInChildrenIncludeInactive<T>(this GameObject go) where T: MonoBehaviour => 
            go.GetComponentsInChildren<T>(true).Single<T>();

        public static T GetComponentInChildrenIncludeInactive<T>(this MonoBehaviour monoBehaviour) where T: MonoBehaviour => 
            monoBehaviour.gameObject.GetComponentInChildrenIncludeInactive<T>();
    }
}

