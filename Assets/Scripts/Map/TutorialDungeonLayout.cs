using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu(menuName = "Map/Tutorial Dungeon Layout", fileName = "TutorialDungeonLayout")]
    public class TutorialDungeonLayout : ScriptableObject
    {
        public List<RoomCoord> rooms = new List<RoomCoord>();
        public List<TileOverride> blockedTiles = new List<TileOverride>();
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
    public struct TileOverride
    {
        public int x;
        public int y;
        public StatusFlag status;

        public Coord ToCoord() => new Coord(x, y);
    }

    public enum UnitSpawnType
    {
        Bat,
        Slime
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
