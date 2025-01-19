namespace Unit.Enemy.BT
{
    public enum Result
    {
        None,
        Success,
        Failure,
        Running,
    }
    
    
    public abstract class Node
    {
        public abstract Result Invoke();
    }
}
