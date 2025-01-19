using System.Collections.Generic;

namespace Unit.Enemy.BT
{
    public abstract class Composite : Node
    {
        protected List<Node> Children = new List<Node>();
        protected int CurrentChildIndex;


        public void Attach(Node child)
        {
            Children.Add(child);
        }
    }
}
