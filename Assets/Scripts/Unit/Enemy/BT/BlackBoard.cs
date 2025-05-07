using System.Collections.Generic;
using Map;
using Unit.Player;

namespace Unit.Enemy.BT
{
    public class BlackBoard
    {
        public Knight TargetPlayer { get; set; }
        public Coord NextMoveCoord { get; set; }
        public Coord[] NextAttackCoords { get; set; }
        public bool IsReadyToMove { get; set; }
        public bool IsReadyToAttack { get; set; }
        public List<Coord[]> Patterns { get; private set; } = new List<Coord[]>(); // 공격 패턴
        
        public BlackBoard()
        {
            Reset();
        }
        
        // 상태를 초기화하는 함수
        public void Reset()
        {
            TargetPlayer = null;
            NextMoveCoord = default;
            NextAttackCoords = null;
            IsReadyToMove = false;
            IsReadyToAttack = false;
        }
    }
}