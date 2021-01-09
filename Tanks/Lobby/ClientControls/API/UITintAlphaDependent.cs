namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    public class UITintAlphaDependent : UITint
    {
        public override void SetTint(Color tint)
        {
            Color color = tint;
            color.a = 1f;
            color.r = Mathf.Lerp(color.r, 1f, 1f - tint.a);
            color.g = Mathf.Lerp(color.g, 1f, 1f - tint.a);
            color.b = Mathf.Lerp(color.b, 1f, 1f - tint.a);
            base.SetTint(color);
        }
    }
}

