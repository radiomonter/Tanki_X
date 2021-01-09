namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    public class Node
    {
        private Platform.Kernel.ECS.ClientEntitySystem.API.Entity entity;

        public override bool Equals(object o)
        {
            if (ReferenceEquals(this, o))
            {
                return true;
            }
            if (o is Platform.Kernel.ECS.ClientEntitySystem.API.Entity)
            {
                return this.entity.Equals(o);
            }
            if ((o == null) || !ReferenceEquals(base.GetType(), o.GetType()))
            {
                return false;
            }
            Node node = (Node) o;
            return !((this.entity == null) ? !ReferenceEquals(node.entity, null) : !this.entity.Equals(node.entity));
        }

        public override int GetHashCode() => 
            (this.entity == null) ? 0 : this.entity.GetHashCode();

        public T SendEvent<T>(T eventInstance) where T: Event => 
            this.Entity.SendEvent<T>(eventInstance);

        public override string ToString() => 
            this.Entity.ToString();

        public virtual Platform.Kernel.ECS.ClientEntitySystem.API.Entity Entity
        {
            get => 
                this.entity;
            set => 
                this.entity = value;
        }
    }
}

