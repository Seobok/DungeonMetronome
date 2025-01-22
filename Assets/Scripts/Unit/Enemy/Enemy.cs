using System.Collections.Generic;
using Controller;
using DG.Tweening;
using Map;
using Unit.Enemy.BT;
using UnityEngine;
using Utility;

namespace Unit.Enemy
{
    public class Enemy : UnitBase
    {
        public int Hp { get; private set; }
        private Tile PlayerTile => _targetPlayer ? _targetPlayer.Knight.CurTile : null;


        private readonly Vector2[] _direction = new Vector2[4] { Vector2.up, Vector2.right, Vector2.down, Vector2.down };
        private const float MOVE_TIME = 0.2f;
        private EnemySpec _enemySpec;
        private PlayerController _targetPlayer;
        private SpriteRenderer _spriteRenderer;
        private int _moveSpeed;
        private Tile _nextMoveTile;
        private GameObject _enemy;
        

        
        public Enemy()
        {
            GameObject enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy/Enemy");
            _enemy = GameObject.Instantiate(enemyPrefab);
        }

        public void SetEnemySpec(EnemySpec enemySpec)
        {
            _enemySpec = enemySpec;
            Hp = enemySpec._hp;
            _spriteRenderer.sprite = enemySpec._sprite;
            _moveSpeed = enemySpec._moveSpeed;
        }

        public void Move()
        {
            if (_nextMoveTile == null) return;
            
            if (_nextMoveTile.OnTilePlayer)
            {
                // 1데미지

                // 플레이어에게 박치기 후 다시 돌아오기
                _enemy.transform.DOMove(new Vector3((_nextMoveTile.Position.x + _enemy.transform.position.x) / 2,
                    (_nextMoveTile.Position.y + _enemy.transform.position.y) / 2,
                    _enemy.transform.position.z), MOVE_TIME / 2).SetLoops(2, LoopType.Yoyo);
            }
            else
            {
                // 실제 타일 이동 X
                CurTile.OnTileUnit = null;
                PosX = _nextMoveTile.X;
                PosY = _nextMoveTile.Y;
                CurTile.OnTileUnit = this;
            
                // 실제 타일 이동
                _enemy.transform.DOMoveX(CurTile.Position.x, MOVE_TIME).SetEase(Ease.InOutCubic);
                _enemy.transform.DOMoveY(_enemy.transform.position.y + 0.5f, MOVE_TIME / 2).OnComplete(() => 
                    _enemy.transform.DOMoveY(CurTile.Position.y, MOVE_TIME / 2));
            }

            _nextMoveTile = null;
        }

        public Result HasTargetPlayer()
        {
            return _targetPlayer ? Result.Success : Result.Failure;
        }
        
        public Result DetectTargetPlayer()
        {
            List<Tile> detectTiles = PathFind.GetTilesInDistance(CurTile, _enemySpec._detectRange, true);
            foreach (Tile tile in detectTiles)
            {
                if (tile.OnTilePlayer)
                {
                    _targetPlayer = tile.OnTilePlayer;
                    return Result.Success;
                }
            }
            return Result.Failure;
        }

        public Result CheckTooFar()
        {
            int distance = PathFind.GetDistance(CurTile, PlayerTile);
            
            return distance > _enemySpec._detectRange ? Result.Success : Result.Failure;
        }

        public Result RemoveTarget()
        {
            _targetPlayer = null;

            return Result.Success;
        }

        public Result CanAttack()
        {
            //공격 범위 안에 있으면 Success
            //아니면 Failure

            return Result.Failure;
        }

        public Result Attack()
        {
            return Result.Success;
        }

        public Result MoveToTarget()
        {
            if (_nextMoveTile != null)
            {
                //실제로 움직이는 부분
                Move();
                return Result.Success;
            }
            
            if(_targetPlayer == null || PlayerTile == null)
                return Result.Failure;
            
            Stack<Tile> path = PathFind.FindPath(CurTile, PlayerTile);
            if (path == null)
                return Result.Failure;
            
            int moveCnt = _moveSpeed;
            _nextMoveTile = CurTile;
            while (moveCnt > 0)
            {
                if (path.Count > 0) break;
                _nextMoveTile = path.Pop();
                moveCnt--;
            }
            
            if(_nextMoveTile.Position.x < CurTile.Position.x)
                _spriteRenderer.flipX = true;
            else
                _spriteRenderer.flipX = false;

            return Result.Running;
        }
        
        /// <summary>
        /// 다음에 이동할 칸을 랜덤으로 정하는 함수
        /// </summary>
        /// <returns> 이동할 수 있는 칸이 없으면 false, 이동할 수 있는 칸이 있으면 true </returns>
        public Result MoveRandomly()
        {
            if (_nextMoveTile != null)
            {
                Move();
                return Result.Success;
            }
            
            _nextMoveTile = CurTile;
            int moveCnt = _moveSpeed;

            while (moveCnt > 0)
            {
                List<Tile> nextTileList = new List<Tile>(4);
                for (int i = 0; i < _direction.Length; i++)
                {
                    Tile nextTile = _nextMoveTile.Room.GetTile(_nextMoveTile.X + (int)_direction[i].x, _nextMoveTile.Y + (int)_direction[i].y);
                    if(nextTile == null)
                        continue;
                    
                    if(nextTile.OnTileUnit == null)
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
            
            if(_nextMoveTile.Position.x < CurTile.Position.x)
                _spriteRenderer.flipX = true;
            else
                _spriteRenderer.flipX = false;

            return Result.Running;
        }
    }
}
