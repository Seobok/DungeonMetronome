namespace Unit.Enemy.BT
{
    public class HasTargetPlayerAction : Node
    {
        private readonly BlackBoard _blackBoard;

        public HasTargetPlayerAction(BlackBoard blackBoard)
        {
            _blackBoard = blackBoard;
        }

        public override Result Invoke()
        {
            return _blackBoard.TargetPlayer != null ? Result.Success : Result.Failure;
        }
    }
}