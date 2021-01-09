namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class SendEventCommand : AbstractCommand
    {
        public SendEventCommand()
        {
        }

        public SendEventCommand(Entity[] entities, Event e)
        {
            this.Entities = entities;
            this.E = e;
        }

        private int CalculateHashCode()
        {
            int num = 0;
            foreach (Entity entity in this.Entities)
            {
                num += entity.GetHashCode();
            }
            return num;
        }

        protected bool Equals(SendEventCommand other) => 
            this.E.Equals(other.E) && this.EqualsCollection(this.Entities, other.Entities);

        public override bool Equals(object obj) => 
            !ReferenceEquals(null, obj) ? (!ReferenceEquals(this, obj) ? (ReferenceEquals(obj.GetType(), base.GetType()) ? this.Equals((SendEventCommand) obj) : false) : true) : false;

        private bool EqualsCollection(Entity[] a, Entity[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (!a.Contains<Entity>(b[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public override void Execute(Engine engine)
        {
            Flow.Current.SendEventSilent(this.E, this.Entities);
        }

        public override int GetHashCode() => 
            (((this.E == null) ? 0 : this.E.GetHashCode()) * 0x18d) ^ ((this.Entities == null) ? 0 : this.CalculateHashCode());

        public SendEventCommand Init(Entity[] entities, Event e)
        {
            this.Entities = entities;
            this.E = e;
            return this;
        }

        public override string ToString() => 
            $"SendEventCommand: Event={this.E} Entities={EcsToStringUtil.EnumerableWithoutTypeToString(this.Entities, ", ")}";

        [ProtocolVaried, ProtocolParameterOrder(0)]
        public Event E { get; set; }

        [ProtocolParameterOrder(1)]
        public Entity[] Entities { get; set; }
    }
}

