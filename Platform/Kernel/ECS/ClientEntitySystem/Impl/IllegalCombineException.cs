namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Text;

    public class IllegalCombineException : Exception
    {
        public IllegalCombineException(Handler handler, ArgumentNode argumentNode) : base($"Expected one entity, but found more:
 handler {EcsToStringUtil.ToString(handler)}, argument {handler.Method.GetParameters()[argumentNode.argument.NodeNumber + 1].Name}, entities [{GetEntities(argumentNode)}]")
        {
        }

        public static string GetEntities(ArgumentNode argumentNode)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < argumentNode.entityNodes.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(",");
                }
                builder.Append(argumentNode.entityNodes[i].entity);
            }
            return builder.ToString();
        }
    }
}

