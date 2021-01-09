namespace Tanks.Lobby.ClientControls.API
{
    using System;

    public class ListItemSelectSoundEffectController : UISoundEffectController
    {
        private void OnItemSelect(ListItem listItem)
        {
            base.PlaySoundEffect();
        }

        public override string HandlerName =>
            "ListItemSelect";
    }
}

