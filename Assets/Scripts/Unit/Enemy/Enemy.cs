using System;
using System.Collections.Generic;
using Controller;
using DG.Tweening;
using Map;
using Unit.Enemy.BT;
using Unit.Player;
using UnityEngine;
using Utility;
using VContainer;
using Random = UnityEngine.Random;

namespace Unit.Enemy
{
    public struct EnemyData
    {
        public UInt16 Id;
        public string Name;
        public int Hp;
        public int DetectRange;
        public int MoveSpeed;
    }
    
    public abstract class Enemy : UnitBase
    {
        public Enemy(Dungeon dungeon, UnitManager unitManager) : base(dungeon, unitManager)
        {
            
        }
        
        
        public abstract int Hp { get; set; }
        public abstract int DetectRange { get; set; }
        public abstract int MoveSpeed { get; set; }


        //BehaviourTree용 변수
        private readonly Vector2[] _direction = new Vector2[4] { Vector2.up, Vector2.right, Vector2.down, Vector2.down };
        private const float MOVE_TIME = 0.2f;
        private Knight _targetPlayer;
        private Tile _nextMoveTile;
        protected BehaviourTree BehaviourTree;


        /// <summary>
        /// 턴이 종료될 때 마다 실행될 함수
        /// </summary>
        public abstract void Act();

        private void Move(Tile cachedTile)
        {
            Tile nextMoveTile = Dungeon.GetTile(cachedTile.Coord.X, cachedTile.Coord.Y);
            // 갈 수 없으면 (플레이어가 있는지는 확인 X)
            if (nextMoveTile.Status != StatusFlag.Empty)
            {
                //제자리 점프
                Transform.DOMoveY(Transform.position.y + 0.5f, MOVE_TIME / 2).SetLoops(2, LoopType.Yoyo);
            }
            // 플레이어가 있는 위치인 경우
            else if (nextMoveTile.Coord == UnitManager.Knight.Position)
            {
                // 1데미지

                // 플레이어에게 박치기 후 다시 돌아오기
                Transform.DOMove(new Vector3((nextMoveTile.Coord.X + Transform.position.x) / 2,
                    (nextMoveTile.Coord.Y + Transform.position.y) / 2,
                    Transform.position.z), MOVE_TIME / 2).SetLoops(2, LoopType.Yoyo);
            }
            // 갈 수 있는 경우
            else
            {
                // 실제 타일 이동 X
                Tile tile = Dungeon.GetTile(Position.X, Position.Y);
                tile.Status = StatusFlag.Empty;
                tile.Unit = null;
                Dungeon.SetTile(Position.X, Position.Y, tile);
                
                Position = nextMoveTile.Coord;
                
                tile = Dungeon.GetTile(Position.X, Position.Y);
                tile.Status = StatusFlag.Unit;
                tile.Unit = this;
                Dungeon.SetTile(Position.X, Position.Y, tile);
            
                // 실제 타일 이동
                Transform.DOMoveX(CurTile.Coord.X, MOVE_TIME).SetEase(Ease.InOutCubic);
                Transform.DOMoveY(Transform.position.y + 0.5f, MOVE_TIME / 2).OnComplete(() => 
                    Transform.DOMoveY(CurTile.Coord.Y, MOVE_TIME / 2));
            }

            ClearNextMoveTile();
        }

        public void TakeDamage(int damage)
        {
            Hp -= damage;
            if (Hp <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            ClearNextMoveTile();
            
            //현재 좌표 기반으로 타일 정보 초기화
            Tile tile = Dungeon.GetTile(Position.X, Position.Y); // 현재 적이 위치한 타일 가져오기

            tile.Status = StatusFlag.Empty; // 타일 상태를 빈 상태로 초기화
            tile.Unit = null;               // 유닛 정보 제거

            Dungeon.SetTile(Position.X, Position.Y, tile); // 초기화된 타일 정보 반영
            
            UnitManager.RemoveEnemy(this); // 적 관리 리스트에서 제거
            // 적 객체 소멸 등 다른 작업
            GameObject.Destroy(GameObject);
        }

        //TODO :: 추후 개별 클래스로 제작
        #region BT
         protected Result HasTargetPlayer()
        {
            return _targetPlayer != null ? Result.Success : Result.Failure;
        }

        protected Result DetectTargetPlayer()
        {
            List<Tile> detectTiles = Dungeon.GetTilesInDistance(CurTile, DetectRange);
            foreach (Tile tile in detectTiles)
            {
                if (UnitManager.Knight.Position == tile.Coord)
                {
                    _targetPlayer = UnitManager.Knight;
                    return Result.Success;
                }
            }
            return Result.Failure;
        }

        protected Result CheckTooFar()
        {
            int distance = PathFind.GetDistance(CurTile, _targetPlayer.CurTile);
            
            return distance > DetectRange ? Result.Success : Result.Failure;
        }

        protected Result RemoveTarget()
        {
            _targetPlayer = null;

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
            
            if(_targetPlayer == null || _targetPlayer.CurTile.Status != StatusFlag.Empty)
                return Result.Failure;
            
            Stack<Tile> path = Dungeon.FindPath(CurTile, _targetPlayer.CurTile);
            if (path == null)
                return Result.Failure;
            
            int moveCnt = MoveSpeed;
            Tile nextMoveTile = CurTile;
            while (moveCnt > 0)
            {
                if (path.Count == 0) break;
                nextMoveTile = path.Pop();
                moveCnt--;
            }
            
            SetNextMoveTile(nextMoveTile);

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
            
            Tile nextMoveTile = CurTile;
            int moveCnt = MoveSpeed;

            while (moveCnt > 0)
            {
                List<Tile> nextTileList = new List<Tile>(4);
                for (int i = 0; i < _direction.Length; i++)
                {
                    Tile nextTile = Dungeon.GetTile(nextMoveTile.Coord.X + (int)_direction[i].x, nextMoveTile.Coord.Y + (int)_direction[i].y);
                    if(nextTile.Status != StatusFlag.Empty)
                        continue;
                    
                    if(nextMoveTile.Unit == null)
                        nextTileList.Add(nextTile);
                }

                if (nextTileList.Count > 0)
                {
                    nextMoveTile = nextTileList[Random.Range(0, nextTileList.Count)];
                    moveCnt--;
                }
                else
                {
                    //이동할 수 있는 칸이 없음
                    return Result.Failure;
                }
            }
            
            SetNextMoveTile(nextMoveTile);

            return Result.Running;
        }
        #endregion

        private void SetNextMoveTile(Tile nextMoveTile)
        {
            _nextMoveTile = nextMoveTile;
            
            //이동할 타일은 초록색
            Dungeon.TileObjects[nextMoveTile.Coord].GetComponent<SpriteRenderer>().color = Color.green;
            
            //이동할 방향을 바라봄
            if(nextMoveTile.Coord.X < CurTile.Coord.X)
                FlipX = true;
            else
                FlipX = false;
        }

        private void ClearNextMoveTile()
        {
            //이동한 타일에 대한 색 초기화
            Dungeon.TileObjects[_nextMoveTile.Coord].GetComponent<SpriteRenderer>().color = Color.white;
            
            _nextMoveTile = default;
        }
    }
}
