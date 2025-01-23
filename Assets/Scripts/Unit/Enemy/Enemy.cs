using System.Collections.Generic;
using Controller;
using DG.Tweening;
using Map;
using Unit.Enemy.BT;
using Unit.Player;
using UnityEngine;
using Utility;

namespace Unit.Enemy
{
    public abstract class Enemy : UnitBase
    {
        public override Coord Position { get; set; }
        public abstract int Hp { get; set; }
        public abstract int DetectRange { get; set; }
        public abstract int MoveSpeed { get; set; }

        private Tile PlayerTile => _targetPlayer != null ? Manager.Knight.CurTile : default;


        //BehaviourTree용 변수
        private readonly Vector2[] _direction = new Vector2[4] { Vector2.up, Vector2.right, Vector2.down, Vector2.down };
        private const float MOVE_TIME = 0.2f;
        private Knight _targetPlayer;
        private Tile _nextMoveTile;
        private readonly GameObject _enemy;
        protected BehaviourTree BehaviourTree;


        public abstract void Act();

        private void Move(Tile nextMoveTile)
        {
            // 갈 수 없으면 (플레이어가 있는지는 확인 X)
            if (nextMoveTile.Status != StatusFlag.Empty)
            {
                //제자리 점프
                _enemy.transform.DOMoveY(_enemy.transform.position.y + 0.5f, MOVE_TIME / 2).SetLoops(2, LoopType.Yoyo);
            }
            // 플레이어가 있는 위치인 경우
            else if (nextMoveTile.Coord == Manager.Knight.Position)
            {
                // 1데미지

                // 플레이어에게 박치기 후 다시 돌아오기
                _enemy.transform.DOMove(new Vector3((nextMoveTile.Coord.X + _enemy.transform.position.x) / 2,
                    (nextMoveTile.Coord.Y + _enemy.transform.position.y) / 2,
                    _enemy.transform.position.z), MOVE_TIME / 2).SetLoops(2, LoopType.Yoyo);
            }
            // 갈 수 있는 경우
            else
            {
                // 실제 타일 이동 X
                Manager.Enemies[CurTile.Coord.X][CurTile.Coord.Y] = null;
                Manager.Dungeon.GetTile(Position.X, Position.Y , out Tile tile);
                tile.Status |= StatusFlag.Empty;
                tile.Status &= ~StatusFlag.Unit;
                
                Position = nextMoveTile.Coord;
                
                Manager.Enemies[CurTile.Coord.X][CurTile.Coord.Y] = this;
                Manager.Dungeon.GetTile(Position.X, Position.Y , out tile);
                tile.Status &= ~StatusFlag.Empty;
                tile.Status |= StatusFlag.Unit;
            
                // 실제 타일 이동
                _enemy.transform.DOMoveX(CurTile.Coord.X, MOVE_TIME).SetEase(Ease.InOutCubic);
                _enemy.transform.DOMoveY(_enemy.transform.position.y + 0.5f, MOVE_TIME / 2).OnComplete(() => 
                    _enemy.transform.DOMoveY(CurTile.Coord.Y, MOVE_TIME / 2));
            }

            _nextMoveTile = default;
        }

        //TODO :: 추후 개별 클래스로 제작
        #region BT
         protected Result HasTargetPlayer()
        {
            return _targetPlayer != null ? Result.Success : Result.Failure;
        }

        protected Result DetectTargetPlayer()
        {
            List<Tile> detectTiles = Manager.Dungeon.GetTilesInDistance(CurTile, DetectRange);
            foreach (Tile tile in detectTiles)
            {
                if (Manager.Knight.Position == tile.Coord)
                {
                    _targetPlayer = Manager.Knight;
                    return Result.Success;
                }
            }
            return Result.Failure;
        }

        protected Result CheckTooFar()
        {
            int distance = PathFind.GetDistance(CurTile, PlayerTile);
            
            return distance > DetectRange ? Result.Success : Result.Failure;
        }

        protected Result RemoveTarget()
        {
            _targetPlayer = null;

            return Result.Success;
        }

        protected Result CanAttack()
        {
            //공격 범위 안에 있으면 Success
            //아니면 Failure

            return Result.Failure;
        }

        protected Result Attack()
        {
            return Result.Success;
        }

        protected Result MoveToTarget()
        {
            if (_nextMoveTile.Status == StatusFlag.Empty)
            {
                //실제로 움직이는 부분
                Move(_nextMoveTile);
                return Result.Success;
            }
            
            if(_targetPlayer == null || PlayerTile.Status != StatusFlag.Empty)
                return Result.Failure;
            
            Stack<Tile> path = Manager.Dungeon.FindPath(CurTile, PlayerTile);
            if (path == null)
                return Result.Failure;
            
            int moveCnt = MoveSpeed;
            _nextMoveTile = CurTile;
            while (moveCnt > 0)
            {
                if (path.Count > 0) break;
                _nextMoveTile = path.Pop();
                moveCnt--;
            }
            
            if(_nextMoveTile.Coord.X < CurTile.Coord.X)
                Manager.EnemyObjects[Position].GetComponent<SpriteRenderer>().flipX = true;
            else
                Manager.EnemyObjects[Position].GetComponent<SpriteRenderer>().flipX = false;

            return Result.Running;
        }
        
        /// <summary>
        /// 다음에 이동할 칸을 랜덤으로 정하는 함수
        /// </summary>
        /// <returns> 이동할 수 있는 칸이 없으면 false, 이동할 수 있는 칸이 있으면 true </returns>
        protected Result MoveRandomly()
        {
            if (_nextMoveTile.Status == StatusFlag.Empty)
            {
                Move(_nextMoveTile);
                return Result.Success;
            }
            
            _nextMoveTile = CurTile;
            int moveCnt = MoveSpeed;

            while (moveCnt > 0)
            {
                List<Tile> nextTileList = new List<Tile>(4);
                for (int i = 0; i < _direction.Length; i++)
                {
                    Manager.Dungeon.GetTile(_nextMoveTile.Coord.X + (int)_direction[i].x, _nextMoveTile.Coord.Y + (int)_direction[i].y, out Tile nextTile);
                    if(nextTile.Status != StatusFlag.Empty)
                        continue;
                    
                    if(Manager.Enemies[nextTile.Coord.X][nextTile.Coord.Y] == null)
                        nextTileList.Add(nextTile);
                }

                if (nextTileList.Count > 0)
                {
                    _nextMoveTile = nextTileList[Random.Range(0, nextTileList.Count)];
                    moveCnt--;
                }
                else
                {
                    //이동할 수 있는 칸이 없음
                    return Result.Failure;
                }
            }
            
            if(_nextMoveTile.Coord.X < CurTile.Coord.X)
                Manager.EnemyObjects[Position].GetComponent<SpriteRenderer>().flipX = true;
            else
                Manager.EnemyObjects[Position].GetComponent<SpriteRenderer>().flipX = false;

            return Result.Running;
        }
        #endregion
       
    }
}
