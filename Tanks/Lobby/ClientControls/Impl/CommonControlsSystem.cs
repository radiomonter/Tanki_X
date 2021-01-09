namespace Tanks.Lobby.ClientControls.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;

    public class CommonControlsSystem : ECSSystem
    {
        [OnEventFire]
        public void SetLocalziedText(NodeAddedEvent e, LocalizedTextNode node)
        {
            node.textMapping.Text = node.localizedText.Text;
        }

        public class LocalizedTextNode : Node
        {
            public LocalizedTextComponent localizedText;
            public TextMappingComponent textMapping;
        }
    }
}

