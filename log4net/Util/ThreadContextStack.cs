namespace log4net.Util
{
    using log4net.Core;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    public sealed class ThreadContextStack : IFixingRequired
    {
        private Stack m_stack = new Stack();

        internal ThreadContextStack()
        {
        }

        public void Clear()
        {
            this.m_stack.Clear();
        }

        internal string GetFullMessage()
        {
            Stack stack = this.m_stack;
            return ((stack.Count <= 0) ? null : ((StackFrame) stack.Peek()).FullMessage);
        }

        object IFixingRequired.GetFixedObject() => 
            this.GetFullMessage();

        public string Pop()
        {
            Stack stack = this.m_stack;
            return ((stack.Count <= 0) ? string.Empty : ((StackFrame) stack.Pop()).Message);
        }

        public IDisposable Push(string message)
        {
            Stack frameStack = this.m_stack;
            frameStack.Push(new StackFrame(message, (frameStack.Count <= 0) ? null : ((StackFrame) frameStack.Peek())));
            return new AutoPopStackFrame(frameStack, frameStack.Count - 1);
        }

        public override string ToString() => 
            this.GetFullMessage();

        public int Count =>
            this.m_stack.Count;

        internal Stack InternalStack
        {
            get => 
                this.m_stack;
            set => 
                this.m_stack = value;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct AutoPopStackFrame : IDisposable
        {
            private Stack m_frameStack;
            private int m_frameDepth;
            internal AutoPopStackFrame(Stack frameStack, int frameDepth)
            {
                this.m_frameStack = frameStack;
                this.m_frameDepth = frameDepth;
            }

            public void Dispose()
            {
                if ((this.m_frameDepth >= 0) && (this.m_frameStack != null))
                {
                    while (this.m_frameStack.Count > this.m_frameDepth)
                    {
                        this.m_frameStack.Pop();
                    }
                }
            }
        }

        private sealed class StackFrame
        {
            private readonly string m_message;
            private readonly ThreadContextStack.StackFrame m_parent;
            private string m_fullMessage;

            internal StackFrame(string message, ThreadContextStack.StackFrame parent)
            {
                this.m_message = message;
                this.m_parent = parent;
                if (parent == null)
                {
                    this.m_fullMessage = message;
                }
            }

            internal string Message =>
                this.m_message;

            internal string FullMessage
            {
                get
                {
                    if ((this.m_fullMessage == null) && (this.m_parent != null))
                    {
                        this.m_fullMessage = this.m_parent.FullMessage + " " + this.m_message;
                    }
                    return this.m_fullMessage;
                }
            }
        }
    }
}

