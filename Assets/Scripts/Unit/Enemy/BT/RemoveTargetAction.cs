namespace Unit.Enemy.BT
{
    public class RemoveTargetAction : Node
    {
        private readonly BlackBoard _blackBoard;

        public RemoveTargetAction(BlackBoard blackBoard)
        {
            _blackBoard = blackBoard;
        }

        public override Result Invoke()
        {
            _blackBoard.TargetPlayer = null;
            return Result.Success;
        }
    }
}