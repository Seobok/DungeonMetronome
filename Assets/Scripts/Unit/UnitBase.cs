using Map;
using UnityEngine;

namespace Unit
{
    /// <summary>
    /// 유닛 기반 클래스
    /// 타일 위에 존재하는 모든 객체를 유닛이라고 정의함
    /// </summary>
    public abstract class UnitBase : MonoBehaviour
    {
        public Room CurRoom { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
    }
}
