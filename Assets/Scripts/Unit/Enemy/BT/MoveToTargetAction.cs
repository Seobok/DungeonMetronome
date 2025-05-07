using System.Collections.Generic;
using Map;
using Utility;

namespace Unit.Enemy.BT
{
    public class MoveToTargetAction : Node
    {
        private readonly Dungeon _dungeon;
        private readonly BlackBoard _blackBoard;
        private readonly Enemy _enemy;

        public MoveToTargetAction(Dungeon dungeon, BlackBoard blackBoard, Enemy enemy)
        {
            _dungeon = dungeon;
            _blackBoard = blackBoard;
            _enemy = enemy;
        }

        public override Result Invoke()
        {
            if (_blackBoard.IsReadyToMove)
            {
                _enemy.Move(_blackBoard.NextMoveCoord);
                return Result.Success;
            }

            if (_blackBoard.TargetPlayer == null || _blackBoard.TargetPlayer.CurTile.Status != StatusFlag.Empty)
                return Result.Failure;

            Stack<Tile> path = _dungeon.FindPath(_enemy.CurTile, _blackBoard.TargetPlayer.CurTile);
            if (path == null)
                return Result.Failure;

            int moveCount = _enemy.MoveSpeed;
            Tile nextMoveTile = _enemy.CurTile;
            while (moveCount > 0 && path.Count > 0)
            {
                nextMoveTile = path.Pop();
                moveCount--;
            }

            _enemy.SetNextMoveTile(nextMoveTile.Coord);
            _blackBoard.IsReadyToMove = true;
            return Result.Running;
        }
    }
}