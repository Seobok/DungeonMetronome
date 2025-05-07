using System;

namespace Unit.Enemy.BT
{
    public class Execution : Node
    {
        public Execution(Func<Result> execute)
        {
            _execute = execute;
        }

        public Execution(Node node)
        {
            _execute = node.Invoke;
        }
        
        
        private Func<Result> _execute;
        
        
        public override Result Invoke()
        {
            return _execute.Invoke();
        }
    }
}