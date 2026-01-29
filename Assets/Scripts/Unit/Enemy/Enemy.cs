using System;
using System.Collections.Generic;
using DG.Tweening;
using Effect;
using Map;
using Unit.Enemy.BT;
using Unit.Player;
using UnityEngine;
using Utility;
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
        public Enemy(Dungeon dungeon, UnitManager unitManager, EffectPool effectPool) : base(dungeon, unitManager, effectPool)
        {
            BlackBoard = new BlackBoard();
            
        }
        
        
        public int Hp { get; set; }
        public int DetectRange { get; set; }
        public int MoveSpeed { get; set; }


        private readonly Vector2[] _direction = new Vector2[4] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
        private const float MOVE_TIME = 0.2f;
        protected BehaviourTree BehaviourTree;
        protected BlackBoard BlackBoard;


        /// <summary>
        /// 턴이 종료될 때 마다 실행될 함수
        /// </summary>
        public void Act()
        {
            BehaviourTree.Invoke();
        }

        public void Move(Coord nextMoveCoord)
        {
            BlackBoard.IsReadyToMove = false;
            
            Tile nextMoveTile = Dungeon.GetTile(nextMoveCoord.X, nextMoveCoord.Y);
            // 플레이어가 있는 위치인 경우
            if (nextMoveTile.Player != null)
            {
                // 1데미지
                nextMoveTile.Player.TakeDamage(1);

                // 플레이어에게 박치기 후 다시 돌아오기
                Transform.DOMove(new Vector3((nextMoveTile.Coord.X + Transform.position.x) / 2,
                    (nextMoveTile.Coord.Y + Transform.position.y) / 2,
                    Transform.position.z), MOVE_TIME / 2).SetLoops(2, LoopType.Yoyo);
            }
            // 갈 수 없으면 (플레이어가 있는지는 확인 X)
            else if (!TileRules.CanEnemyMoveTo(nextMoveTile))
            {
                //제자리 점프
                Transform.DOMoveY(Transform.position.y + 0.5f, MOVE_TIME / 2).SetLoops(2, LoopType.Yoyo);
            }
            // 갈 수 있는 경우
            else
            {
                // 실제 타일 이동 X
                Tile tile = Dungeon.GetTile(Position.X, Position.Y);
                tile.Unit = null;
                Dungeon.SetTile(Position.X, Position.Y, tile);
                
                Position = nextMoveTile.Coord;
                
                tile = Dungeon.GetTile(Position.X, Position.Y);
                tile.Unit = this;
                Dungeon.SetTile(Position.X, Position.Y, tile);
            
                // 실제 타일 이동
                Transform.DOMoveX(CurTile.Coord.X, MOVE_TIME).SetEase(Ease.InOutCubic);
                Transform.DOMoveY(Transform.position.y + 0.5f, MOVE_TIME / 2).OnComplete(() => 
                    Transform.DOMoveY(CurTile.Coord.Y, MOVE_TIME / 2));
            }

            ClearNextMoveTile();
        }

        public void Attack(Coord[] nextAttackCoords)
        {
            BlackBoard.IsReadyToAttack = false;

            foreach (var attackedTile in nextAttackCoords)
            {
                Tile tile = Dungeon.GetTile(attackedTile.X, attackedTile.Y);
                if (tile.Player != null)
                {
                    tile.Player.TakeDamage(1);
                }
            }
            
            ClearNextAttackTile();
        }

        public void TakeDamage(int damage)
        {
            Hp -= damage;
            EffectPool.GetEffect(EffectType.Hit).Play(Transform.position);
            
            if (Hp <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            ClearNextMoveTile();
            ClearNextAttackTile();
            
            //현재 좌표 기반으로 타일 정보 초기화
            Tile tile = Dungeon.GetTile(Position.X, Position.Y); // 현재 적이 위치한 타일 가져오기

            tile.Unit = null;               // 유닛 정보 제거

            Dungeon.SetTile(Position.X, Position.Y, tile); // 초기화된 타일 정보 반영
            
            UnitManager.RemoveEnemy(this); // 적 관리 리스트에서 제거
            // 적 객체 소멸 등 다른 작업
            GameObject.Destroy(GameObject);
        }

        public void SetNextMoveTile(Coord nextMoveCoord)
        {
            BlackBoard.NextMoveCoord = nextMoveCoord;
            
            //이동할 타일은 초록색
            Dungeon.TileObjects[nextMoveCoord].GetComponent<SpriteRenderer>().color = Color.green;
            
            //이동할 방향을 바라봄
            if(nextMoveCoord.X < CurTile.Coord.X)
                FlipX = true;
            else
                FlipX = false;
        }

        public void SetNextAttackTile(Coord[] nextAttackCoords)
        {
            foreach (var attackTile in nextAttackCoords)
            {
                if(Dungeon.TileObjects.ContainsKey(attackTile))
                    Dungeon.TileObjects[attackTile].GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        private void ClearNextMoveTile()
        {
            //이동한 타일에 대한 색 초기화
            if(Dungeon.TileObjects.ContainsKey(BlackBoard.NextMoveCoord))
                Dungeon.TileObjects[BlackBoard.NextMoveCoord].GetComponent<SpriteRenderer>().color = Color.white;
            
            BlackBoard.NextMoveCoord = default;
        }

        private void ClearNextAttackTile()
        {
            if(BlackBoard.NextAttackCoords == null) return;
            
            foreach (var attackTile in BlackBoard.NextAttackCoords)
            {
                Dungeon.TileObjects[attackTile].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}
