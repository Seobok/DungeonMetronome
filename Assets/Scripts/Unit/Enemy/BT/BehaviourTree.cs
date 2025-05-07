using System;
using System.Collections.Generic;

namespace Unit.Enemy.BT
{
    public class BehaviourTree
    {
        private Node _root;
        private Node _current;
        private Stack<Node> _stack = new Stack<Node>();


        public void Invoke()
        {
            _root.Invoke();
        }
        
        public BehaviourTree SetRoot(Node node)
        {
            _root = node;
            _current = _root;
            _stack.Push(_root);

            return this;
        }

        public BehaviourTree Selector()
        {
            if (_current is Composite composite)
            {
                Selector selector = new Selector();
                composite.Attach(selector);
                _stack.Push(selector);
                _current = selector;
            }
            else
            {
                throw new Exception("Selector can only be attached to Composite Nodes.");
            }

            return this;
        }

        public BehaviourTree Sequence()
        {
            if (_current is Composite composite)
            {
                Sequence sequence = new Sequence();
                composite.Attach(sequence);
                _stack.Push(sequence);
                _current = sequence;
            }
            else
            {
                throw new Exception("Sequence can only be attached to Composite Nodes.");
            }

            return this;
        }

        public BehaviourTree CloseComposite()
        {
            if(_stack.Count > 0)
                _stack.Pop();
            if(_stack.Count > 0)
                _current = _stack.Peek();

            return this;
        }

        public BehaviourTree Execution(Func<Result> action)
        {
            if (_current is Composite composite)
            {
                Execution execution = new Execution(action);
                composite.Attach(execution);
            }

            return this;
        }

        public BehaviourTree Execution(Node node)
        {
            if (_current is Composite composite)
            {
                Execution execution = new Execution(node);
                composite.Attach(execution);
            }

            return this;
        }
    }
}