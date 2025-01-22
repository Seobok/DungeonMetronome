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
            get => _tileGo ? new Vector2(_tileGo.transform.position.x, _tileGo.transform.position.y) : Vector2.zero;
            set
            {
                if(_tileGo != null)
                {
                    _tileGo.transform.position = value;
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


        private SpriteRenderer _tilePrefab;
        private SpriteRenderer _tileGo;


        public Tile()
        {
            _tilePrefab = Resources.Load<SpriteRenderer>("Prefabs/Map/Tile");
            _tileGo = GameObject.Instantiate(_tilePrefab);
        }
    }
}
