using System.Collections.Generic;
using Map;
using UnityEngine;

namespace Unit.Enemy.BT
{
    public class CanAttackAction : Node
    {
        private readonly Dungeon _dungeon;
        private readonly BlackBoard _blackBoard;
        private readonly Enemy _enemy;

        public CanAttackAction(Dungeon dungeon, BlackBoard blackBoard, Enemy enemy)
        {
            _dungeon = dungeon;
            _blackBoard = blackBoard;
            _enemy = enemy;
        }

        public override Result Invoke()
        {
            foreach (var pattern in _blackBoard.Patterns)
            {
                if (pattern == null) continue;

                foreach (var offset in pattern)
                {
                    Tile tileToCheck = _dungeon.GetTile(
                        _enemy.CurTile.Coord.X + offset.X,
                        _enemy.CurTile.Coord.Y + offset.Y
                    );

                    if (tileToCheck.Coord == _blackBoard.TargetPlayer.Position)
                    {
                        // 적의 방향 전환
                        _enemy.FlipX = offset.X < 0;

                        // 공격 좌표 업데이트
                        _blackBoard.NextAttackCoords = _dungeon.Offsets2Coords(pattern, _enemy.CurTile);

                        return Result.Success;
                    }
                }
            }

            return Result.Failure;
        }
    }
}