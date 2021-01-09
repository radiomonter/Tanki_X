namespace log4net
{
    using log4net.Util;
    using System;
    using System.Collections;

    public sealed class NDC
    {
        private NDC()
        {
        }

        public static void Clear()
        {
            ThreadContext.Stacks["NDC"].Clear();
        }

        public static Stack CloneStack() => 
            ThreadContext.Stacks["NDC"].InternalStack;

        public static void Inherit(Stack stack)
        {
            ThreadContext.Stacks["NDC"].InternalStack = stack;
        }

        public static string Pop() => 
            ThreadContext.Stacks["NDC"].Pop();

        public static IDisposable Push(string message) => 
            ThreadContext.Stacks["NDC"].Push(message);

        public static void Remove()
        {
        }

        public static void SetMaxDepth(int maxDepth)
        {
            if (maxDepth >= 0)
            {
                ThreadContextStack stack = ThreadContext.Stacks["NDC"];
                if (maxDepth == 0)
                {
                    stack.Clear();
                }
                else
                {
                    while (stack.Count > maxDepth)
                    {
                        stack.Pop();
                    }
                }
            }
        }

        public static int Depth =>
            ThreadContext.Stacks["NDC"].Count;
    }
}

