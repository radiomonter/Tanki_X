namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;

    public class JoinAllLogPart : LogPart
    {
        private readonly ICollection<Entity> resolvedEntities;
        private readonly HandlerArgument handlerArgument;

        public JoinAllLogPart(HandlerArgument handlerArgument, ICollection<Entity> resolvedEntities)
        {
            this.resolvedEntities = resolvedEntities;
            this.handlerArgument = handlerArgument;
        }

        public Optional<string> GetSkipReason() => 
            (this.resolvedEntities.Count != 0) ? Optional<string>.empty() : Optional<string>.of($"	Missing JoinAll node={this.GetNodeClassName}, parameter=[{this.handlerArgument.NodeNumber + 1}]
	");

        private string GetNodeClassName =>
            this.handlerArgument.ClassInstanceDescription.NodeClass.Name;
    }
}

