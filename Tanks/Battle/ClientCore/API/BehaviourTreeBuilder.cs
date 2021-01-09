namespace Tanks.Battle.ClientCore.API
{
    using System;
    using System.Collections;

    public class BehaviourTreeBuilder
    {
        private readonly Stack builderStack = new Stack();
        private BehaviourTreeNode root;
        private string treeName;

        public BehaviourTreeBuilder(string name)
        {
            this.treeName = name;
        }

        private void AddChild(BehaviourTreeNode child)
        {
            object obj2 = this.builderStack.Peek();
            if (ReferenceEquals(obj2.GetType().BaseType, typeof(CompositeNode)))
            {
                (obj2 as CompositeNode).AddChild(child);
            }
            if (ReferenceEquals(obj2.GetType().BaseType, typeof(DecoratorNode)))
            {
                (obj2 as DecoratorNode).AddChild(child);
                this.builderStack.Pop();
            }
        }

        public BehaviourTreeNode Build()
        {
            if (this.builderStack.Count != 0)
            {
                throw new Exception("One of composite nodes in tree wasn't closed! Tree name:" + this.treeName);
            }
            return this.root;
        }

        public BehaviourTreeBuilder ConnectTree(BehaviourTreeBuilder treePart)
        {
            BehaviourTreeNode child = treePart.Build();
            if (this.root == null)
            {
                this.root = child;
                return this;
            }
            this.AddChild(child);
            return this;
        }

        public BehaviourTreeBuilder Do(ActionNode action)
        {
            this.AddChild(action);
            return this;
        }

        public BehaviourTreeBuilder End()
        {
            this.builderStack.Pop();
            return this;
        }

        public BehaviourTreeBuilder ForTime(float time)
        {
            TimerNode child = new TimerNode {
                Time = time
            };
            this.AddChild(child);
            this.builderStack.Push(child);
            return this;
        }

        public BehaviourTreeBuilder If(ConditionNode condition)
        {
            this.AddChild(condition);
            return this;
        }

        public BehaviourTreeBuilder StartDoOnceIn(float time)
        {
            OnceInTimeNode node = new OnceInTimeNode {
                Time = time
            };
            if (this.root == null)
            {
                this.root = node;
                this.builderStack.Push(node);
                return this;
            }
            this.AddChild(node);
            this.builderStack.Push(node);
            return this;
        }

        public BehaviourTreeBuilder StartParallel()
        {
            ParallelNode node = new ParallelNode();
            if (this.root == null)
            {
                this.root = node;
                this.builderStack.Push(node);
                return this;
            }
            this.AddChild(node);
            this.builderStack.Push(node);
            return this;
        }

        public BehaviourTreeBuilder StartPreconditionSequence()
        {
            PreconditionSequence sequence = new PreconditionSequence();
            if (this.root == null)
            {
                this.root = sequence;
                this.builderStack.Push(sequence);
                return this;
            }
            this.AddChild(sequence);
            this.builderStack.Push(sequence);
            return this;
        }

        public BehaviourTreeBuilder StartSelector()
        {
            SelectorNode node = new SelectorNode();
            if (this.root == null)
            {
                this.root = node;
                this.builderStack.Push(node);
                return this;
            }
            this.AddChild(node);
            this.builderStack.Push(node);
            return this;
        }

        public BehaviourTreeBuilder StartSequence()
        {
            SequenceNode node = new SequenceNode();
            if (this.root == null)
            {
                this.root = node;
                this.builderStack.Push(node);
                return this;
            }
            this.AddChild(node);
            this.builderStack.Push(node);
            return this;
        }
    }
}

