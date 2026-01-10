using System;
using System.Collections.Generic;
using Unit;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu(menuName = "Map/Tutorial Dungeon Layout", fileName = "TutorialDungeonLayout")]
    public class TutorialDungeonLayout : ScriptableObject
    {
        public List<RoomCoord> rooms = new List<RoomCoord>();
        public List<UnitSpawn> unitSpawns = new List<UnitSpawn>();
        public Coord playerSpawn = Coord.Zero;
    }

    [Serializable]
    public struct RoomCoord
    {
        public int x;
        public int y;

        public Coord ToCoord() => new Coord(x, y);
    }

    [Serializable]
    public struct UnitSpawn
    {
        public int x;
        public int y;
        public UnitSpawnType unitType;

        public Coord ToCoord() => new Coord(x, y);
    }
}
