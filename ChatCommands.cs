using Platform.Kernel.ECS.ClientEntitySystem.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tanks.Lobby.ClientCommunicator.Impl;

public class ChatCommands
{
    private static List<Command> CommandList;
    [CompilerGenerated]
    private static Func<string[], Event> <>f__mg$cache0;
    [CompilerGenerated]
    private static Func<string[], Event> <>f__mg$cache1;
    [CompilerGenerated]
    private static Func<string[], Event> <>f__mg$cache2;
    [CompilerGenerated]
    private static Func<string[], Event> <>f__mg$cache3;
    [CompilerGenerated]
    private static Func<string[], Event> <>f__mg$cache4;
    [CompilerGenerated]
    private static Func<string[], Event> <>f__mg$cache5;
    [CompilerGenerated]
    private static Func<string[], Event> <>f__mg$cache6;
    [CompilerGenerated]
    private static Func<string[], Event> <>f__mg$cache7;

    static ChatCommands()
    {
        List<Command> list = new List<Command>();
        Command item = new Command {
            CommandText = "/leave",
            ParamsCount = 0
        };
        if (<>f__mg$cache0 == null)
        {
            <>f__mg$cache0 = new Func<string[], Event>(ChatCommands.Leave);
        }
        item.CreateEventFunc = <>f__mg$cache0;
        list.Add(item);
        item = new Command {
            CommandText = "/invite"
        };
        <>f__mg$cache1 ??= new Func<string[], Event>(ChatCommands.Invite);
        item.CreateEventFunc = <>f__mg$cache1;
        list.Add(item);
        item = new Command {
            CommandText = "/w"
        };
        <>f__mg$cache2 ??= new Func<string[], Event>(ChatCommands.CreatePersonal);
        item.CreateEventFunc = <>f__mg$cache2;
        list.Add(item);
        item = new Command {
            CommandText = "/block"
        };
        <>f__mg$cache3 ??= new Func<string[], Event>(ChatCommands.Mute);
        item.CreateEventFunc = <>f__mg$cache3;
        list.Add(item);
        item = new Command {
            CommandText = "/mute"
        };
        <>f__mg$cache4 ??= new Func<string[], Event>(ChatCommands.Mute);
        item.CreateEventFunc = <>f__mg$cache4;
        list.Add(item);
        item = new Command {
            CommandText = "/unblock"
        };
        <>f__mg$cache5 ??= new Func<string[], Event>(ChatCommands.Unmute);
        item.CreateEventFunc = <>f__mg$cache5;
        list.Add(item);
        item = new Command {
            CommandText = "/unmute"
        };
        <>f__mg$cache6 ??= new Func<string[], Event>(ChatCommands.Unmute);
        item.CreateEventFunc = <>f__mg$cache6;
        list.Add(item);
        item = new Command {
            CommandText = "/kick"
        };
        <>f__mg$cache7 ??= new Func<string[], Event>(ChatCommands.Kick);
        item.CreateEventFunc = <>f__mg$cache7;
        list.Add(item);
        CommandList = list;
    }

    private static Event CreatePersonal(string[] parameters) => 
        new OpenPersonalChannelEvent { UserUid = parameters[0] };

    private static Event Invite(string[] parameters) => 
        new InviteUserToChatEvent { UserUid = parameters[0] };

    public static bool IsCommand(string message, out Event commandEvent)
    {
        commandEvent = null;
        if (message.StartsWith("/"))
        {
            char[] separator = new char[] { ' ' };
            string[] source = message.Split(separator);
            using (List<Command>.Enumerator enumerator = CommandList.GetEnumerator())
            {
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                    Command current = enumerator.Current;
                    if (source[0] == current.CommandText)
                    {
                        bool flag;
                        if (source.Length != (current.ParamsCount + 1))
                        {
                            flag = false;
                        }
                        else
                        {
                            List<string> list = source.ToList<string>();
                            list.RemoveAt(0);
                            commandEvent = current.CreateEventFunc(list.ToArray());
                            flag = true;
                        }
                        return flag;
                    }
                }
            }
        }
        return false;
    }

    private static Event Kick(string[] parameters) => 
        new KickUserFromChatEvent { UserUid = parameters[0] };

    private static Event Leave(string[] parameters) => 
        new LeaveFromChatEvent();

    private static Event Mute(string[] parameters) => 
        new MuteUserEvent { UserUid = parameters[0] };

    private static Event Unmute(string[] parameters) => 
        new UnmuteUserEvent { UserUid = parameters[0] };

    private class Command
    {
        public string CommandText;
        public int ParamsCount = 1;
        public Func<string[], Event> CreateEventFunc;
    }
}

