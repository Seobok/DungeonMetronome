using Unit.Enemy;

namespace Map
{
    public static class TileRules
    {
        public static bool HasPlayer(Tile tile) => tile.Player != null;
        public static bool HasUnit(Tile tile) => tile.Unit != null;
        public static bool IsBlocked(Tile tile) => tile.Status.HasFlag(StatusFlag.Blocked);

        public static bool IsBlockedForEnemy(Tile tile)
        {
            return IsBlocked(tile) || HasUnit(tile);
        }

        public static bool CanEnemyMoveTo(Tile tile)
        {
            return !IsBlockedForEnemy(tile) && !HasPlayer(tile);
        }

        public static bool IsBlockedForPlayer(Tile tile)
        {
            return IsBlocked(tile) || tile.Unit is Enemy;
        }

        public static bool IsSpawnable(Tile tile)
        {
            return !IsBlocked(tile) && !HasUnit(tile) && !HasPlayer(tile);
        }

        public static bool TryGetEnemy(Tile tile, out Enemy enemy)
        {
            if (tile.Unit is Enemy found)
            {
                enemy = found;
                return true;
            }

            enemy = null;
            return false;
        }
    }
}
