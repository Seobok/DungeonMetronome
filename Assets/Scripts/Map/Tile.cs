using Controller;
using Unit;
using UnityEngine;

namespace Map
{
    public class Tile : MonoBehaviour
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Room Room { get; set; }
        public UnitBase OnTileUnit { get; set; }
        public PlayerController OnTilePlayer { get; set; }
        public int FCost => GCost + HCost;
        public int GCost { get; set; }
        public int HCost { get; set; }
        public Tile AStarParent { get; set; }
    }
}
