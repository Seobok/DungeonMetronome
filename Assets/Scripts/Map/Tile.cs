using System;
using UnityEngine;

namespace Map
{
    public class Tile : MonoBehaviour
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Room Room { get; set; }
    }
}
