using System.Collections.Generic;
using Controller;
using Map;
using Utility;

namespace Unit.Enemy
{
    public class Enemy : UnitBase
    {
        public int Hp { get; private set; }
        
        
        private EnemySpec _enemySpec;
        private PlayerController _targetPlayer;


        public void SetEnemySpec(EnemySpec enemySpec)
        {
            _enemySpec = enemySpec;
            Hp = _enemySpec._hp;
        }

        public bool IsDetected()
        {
            List<Tile> detectTiles = PathFind.GetTilesInDistance(CurTile, _enemySpec._detectRange, true);
            foreach (Tile tile in detectTiles)
            {
                if (tile.OnTilePlayer)
                {
                    _targetPlayer = tile.OnTilePlayer;
                    return true;
                }
            }
            return false;
        }
    }
}
