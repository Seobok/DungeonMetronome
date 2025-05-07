using System.Collections.Generic;
using Map;
using Utility;

namespace Unit.Enemy.BT
{
    public class DetectTargetPlayerAction : Node
    {
        private readonly Dungeon _dungeon;
        private readonly UnitManager _unitManager;
        private readonly BlackBoard _blackBoard;
        private readonly Enemy _enemy;

        public DetectTargetPlayerAction(Dungeon dungeon, UnitManager unitManager, BlackBoard blackBoard, Enemy enemy)
        {
            _dungeon = dungeon;
            _unitManager = unitManager;
            _blackBoard = blackBoard;
            _enemy = enemy;
        }

        public override Result Invoke()
        {
            List<Tile> detectTiles = _dungeon.GetTilesInDistance(_enemy.CurTile, _enemy.DetectRange);
            foreach (Tile tile in detectTiles)
            {
                if (tile.Coord == _unitManager.Knight.Position)
                {
                    _blackBoard.TargetPlayer = _unitManager.Knight;
                    return Result.Success;
                }
            }
            return Result.Failure;
        }
    }
}