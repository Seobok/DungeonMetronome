using System;
using UnityEngine;

namespace Map
{
    public struct Coord : IEquatable<Coord>
    {
        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }
            
        public int X, Y;
            
        public static bool operator==(Coord a, Coord b) =>
            (a.X == b.X) && (a.Y == b.Y);
                
        public static bool operator!=(Coord a, Coord b) =>
            !(a == b);
        
        public static Coord operator+(Coord a, Coord b) =>
            new Coord(a.X + b.X, a.Y + b.Y);
        
        public static Coord Zero => new Coord(0, 0);

        public bool Equals(Coord other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Coord other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}