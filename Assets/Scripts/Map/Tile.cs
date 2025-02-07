using System;

namespace Map
{
    [Flags]
    public enum StatusFlag : uint
    {
        None = 0,
        Empty = 1<<0,
        Blocked = 1<<1,
        Unit = 1<<2,
    }
    
    public struct Tile : IEquatable<Tile>
    {
        public Coord Coord;
        public StatusFlag Status;

        
        public static bool operator==(Tile a, Tile b) =>
            (a.Coord == b.Coord) && (a.Coord == b.Coord);
        public static bool operator!=(Tile a, Tile b) =>
            !(a == b);
        
        
        public bool Equals(Tile other)
        {
            return Coord.Equals(other.Coord) && Status == other.Status;
        }

        public override bool Equals(object obj)
        {
            return obj is Tile other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Coord, (int)Status);
        }
    }
}
