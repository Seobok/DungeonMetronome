using Map;
using UnityEngine;

namespace Unit
{
    /// <summary>
    /// 유닛 기반 클래스
    /// 타일 위에 존재하는 모든 객체를 유닛이라고 정의함
    /// </summary>
    public abstract class UnitBase
    {
        //public Room CurRoom { get; set; }
        public UnitManager Manager { get; set; }
        public abstract Coord Position { get; set; }
        public Tile CurTile
        {
            get
            {
                Manager.Dungeon.GetTile(Position.X, Position.Y , out Tile tile);
                return tile;
            }
        }
    }
}
