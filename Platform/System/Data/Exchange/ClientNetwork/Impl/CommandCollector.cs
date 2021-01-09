namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CommandCollector
    {
        public CommandCollector()
        {
            this.Commands = new List<Command>();
        }

        public void Add(Command command)
        {
            this.Commands.Add(command);
        }

        public void Clear()
        {
            this.Commands.Clear();
        }

        public List<Command> Commands { get; set; }
    }
}

