using Effect;
using Map;
using UnityEngine;
using VContainer;

namespace Unit
{
    /// <summary>
    /// 유닛 기반 클래스
    /// 타일 위에 존재하는 모든 객체를 유닛이라고 정의함
    /// </summary>
    public abstract class UnitBase
    {
        protected UnitBase(Dungeon dungeon, UnitManager unitManager, EffectPool effectPool)
        {
            GameObject = new GameObject();
            Renderer = GameObject.AddComponent<SpriteRenderer>();
            Animator = GameObject.AddComponent<Animator>(); // Animator 추가
            Dungeon = dungeon;
            UnitManager = unitManager;
            EffectPool = effectPool;
        }

        public bool FlipX
        {
            get => Renderer.flipX;
            set => Renderer.flipX = value;
        }
        public Coord Position { get; set; }
        public Transform Transform => GameObject.transform;
        public Tile CurTile => Dungeon.GetTile(Position.X, Position.Y);

        public Dungeon Dungeon { get; private set; }
        protected UnitManager UnitManager { get; private set; }
        protected EffectPool EffectPool { get; private set; }
        
        
        protected readonly GameObject GameObject;
        protected readonly SpriteRenderer Renderer;
        protected readonly Animator Animator;
    }
}
