namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using UnityEngine;

    public class KillAssistTotalScoreComponent : MonoBehaviour
    {
        public KillAssistComponent killAssist;

        public void SetDisappearing()
        {
            this.killAssist.SetVisible(false);
        }

        public void SetTotalNumberToZero()
        {
            this.killAssist.SetTotalNumberToZero();
        }

        public void SetVisible()
        {
            this.killAssist.SetVisible(true);
        }
    }
}

