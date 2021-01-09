namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CommandPacket
    {
        public CommandPacket()
        {
        }

        public CommandPacket(List<Command> commands)
        {
            this.Commands = commands;
        }

        public void Append(CommandPacket newPacket)
        {
            this.Commands.AddRange(newPacket.Commands);
        }

        public List<Command> Commands { get; internal set; }
    }
}

