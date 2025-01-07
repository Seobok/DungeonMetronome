using UnityEngine;

namespace Map
{
    public class Dungeon : MonoBehaviour
    {
        public Room[,] Rooms { get; set; } = new Room[DUNGEON_X, DUNGEON_Y];


        private const int DUNGEON_X = 5;
        private const int DUNGEON_Y = 5;
        
        
        
    }
}