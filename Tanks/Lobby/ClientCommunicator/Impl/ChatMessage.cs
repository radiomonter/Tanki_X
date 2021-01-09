namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class ChatMessage
    {
        public static Color SystemColor = Color.yellow;

        public string GetEllipsis(Func<Tanks.Lobby.ClientCommunicator.Impl.ChatType, Color> getChatColorFunc)
        {
            string str = string.Empty;
            string message = this.Message;
            if (!this.Self)
            {
                str = this.System ? this.Author : ("@" + this.Author);
                message = str + ": " + this.Message;
            }
            int length = 0x20;
            bool flag = false;
            if (message.Length <= length)
            {
                length = message.Length;
            }
            else
            {
                flag = true;
            }
            int index = message.IndexOf("\n");
            if (index > 0)
            {
                flag = true;
                if (index < length)
                {
                    length = index;
                }
            }
            string str3 = getChatColorFunc(this.ChatType).ToHexString();
            message = message.Substring(str.Length, length - str.Length);
            if (this.System)
            {
                str3 = SystemColor.ToHexString();
            }
            else
            {
                message = $"<noparse>{message}</noparse>";
            }
            string str4 = !flag ? string.Empty : "...";
            return $"<color=#{str3}>{str}</color>{message}{str4}";
        }

        public string GetMessageText() => 
            !this.System ? $"<noparse>{this.Message}</noparse>" : this.Message;

        public string GetNickText()
        {
            string author = this.Author;
            string str2 = author;
            if (!this.System)
            {
                str2 = "@" + str2;
            }
            return $"<link="{author}">{str2}</link>";
        }

        public string Author { get; set; }

        public string Message { get; set; }

        public string Time { get; set; }

        public bool System { get; set; }

        public bool Self { get; set; }

        public Tanks.Lobby.ClientCommunicator.Impl.ChatType ChatType { get; set; }

        public long ChatId { get; set; }

        public string AvatarId { get; set; }
    }
}

