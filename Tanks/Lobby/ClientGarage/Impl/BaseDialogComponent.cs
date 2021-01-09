namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class BaseDialogComponent : BehaviourComponent
    {
        public virtual void Hide()
        {
        }

        public virtual void Show(List<Animator> animators = null)
        {
        }
    }
}

