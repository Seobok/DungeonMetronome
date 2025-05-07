using Utility;

namespace Unit.Enemy.BT
{
    public class CheckTooFarAction : Node
    {
        private readonly BlackBoard _blackBoard;
        private readonly Enemy _enemy;

        public CheckTooFarAction(BlackBoard blackBoard, Enemy enemy)
        {
            _blackBoard = blackBoard;
            _enemy = enemy;
        }

        public override Result Invoke()
        {
            if (_blackBoard.TargetPlayer == null) return Result.Failure;

            int distance = PathFind.GetDistance(_enemy.CurTile, _blackBoard.TargetPlayer.CurTile);
            return distance > _enemy.DetectRange ? Result.Success : Result.Failure;
        }
    }
}