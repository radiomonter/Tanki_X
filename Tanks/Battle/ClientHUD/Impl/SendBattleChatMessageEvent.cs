namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x151d3563205L)]
    public class SendBattleChatMessageEvent : Event
    {
        public SendBattleChatMessageEvent()
        {
        }

        public SendBattleChatMessageEvent(string message)
        {
            this.Message = message;
        }

        public string Message { get; set; }
    }
}

