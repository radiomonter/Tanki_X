namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class EntityState
    {
        private readonly IDictionary<Type, FieldInfo> fields;
        private readonly NodeDescription nodeDescription;

        public EntityState(Type nodeClass, NodeDescription nodeDescription)
        {
            this.nodeDescription = nodeDescription;
            this.fields = new Dictionary<Type, FieldInfo>();
            this.CreateNode(nodeClass);
            this.ParseFields();
        }

        public void AssignValue(Type componentClass, Component value)
        {
            this.fields[componentClass].SetValue(this.Node, value);
        }

        private void CreateNode(Type nodeClass)
        {
            this.Node = (Platform.Kernel.ECS.ClientEntitySystem.API.Node) nodeClass.GetConstructors()[0].Invoke(Collections.EmptyArray);
        }

        private void ParseFields()
        {
            foreach (FieldInfo info in this.Node.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (typeof(Component).IsAssignableFrom(info.FieldType))
                {
                    this.fields[info.FieldType] = info;
                }
            }
        }

        public Platform.Kernel.ECS.ClientEntitySystem.API.Node Node { get; private set; }

        public Platform.Kernel.ECS.ClientEntitySystem.API.Entity Entity
        {
            set => 
                this.Node.Entity = value;
        }

        public ICollection<Type> Components =>
            this.nodeDescription.BaseComponents;
    }
}

