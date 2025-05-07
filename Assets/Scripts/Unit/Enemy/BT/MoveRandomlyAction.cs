using System.Collections.Generic;
using Map;
using UnityEngine;

namespace Unit.Enemy.BT
{
    public class MoveRandomlyAction : Node
    {
        private readonly Dungeon _dungeon;
        private readonly BlackBoard _blackBoard;
        private readonly Enemy _enemy;
        private readonly Vector2[] _directions = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

        public MoveRandomlyAction(Dungeon dungeon, BlackBoard blackBoard, Enemy enemy)
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
            
            Tile nextTile = _enemy.CurTile;
            int moveCount = _enemy.MoveSpeed;

            while (moveCount > 0)
            {
                List<Tile> nextTileList = new List<Tile>();
                foreach (Vector2 direction in _directions)
                {
                    Tile candidateTile = _dungeon.GetTile(
                        nextTile.Coord.X + (int)direction.x,
                        nextTile.Coord.Y + (int)direction.y
                    );

                    // 이동 가능한 타일만 추가
                    if (candidateTile.Status == StatusFlag.Empty)
                    {
                        nextTileList.Add(candidateTile);
                    }
                }

                if (nextTileList.Count > 0)
                {
                    // 랜덤하게 타일 선택
                    nextTile = nextTileList[Random.Range(0, nextTileList.Count)];
                    moveCount--;
                }
                else
                {
                    // 이동할 수 있는 칸이 없음
                    return Result.Failure;
                }
            }

            // 이동 좌표 설정
            _enemy.SetNextMoveTile(nextTile.Coord);
            _blackBoard.IsReadyToMove = true;
            
            return Result.Running;
        }
    }
}