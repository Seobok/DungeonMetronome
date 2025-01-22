using Controller;
using Unit;
using UnityEngine;

namespace Map
{
    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 Position 
        {
            get => _tile ? new Vector2(_tile.transform.position.x, _tile.transform.position.y) : Vector2.zero;
            set
            {
                if(_tile)
                {
                    _tile.transform.position = value;
                }
            }
        }
        public Room Room { get; set; }
        public UnitBase OnTileUnit { get; set; }
        public PlayerController OnTilePlayer { get; set; }
        public int FCost => GCost + HCost;
        public int GCost { get; set; }
        public int HCost { get; set; }
        public Tile AStarParent { get; set; }


        private GameObject _tile;


        public Tile()
        {
            GameObject tilePrefab = Resources.Load<GameObject>("Prefabs/Map/Tile");
            _tile = GameObject.Instantiate(tilePrefab);
        }
    }
}
