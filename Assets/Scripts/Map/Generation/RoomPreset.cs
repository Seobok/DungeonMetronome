using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map.Generation
{
    public enum RoomPresetRole
    {
        Normal,
        Exit
    }

    public enum RoomSizeType
    {
        OneByOne,
        TwoByOne,
        OneByTwo,
        TwoByTwo
    }

    public enum RoomPresetUnitType
    {
        Bat,
        Slime,
        Blocker,
        Item
    }

    [Serializable]
    public struct RoomPresetDoor
    {
        public Vector2Int LocalPosition;
    }

    [Serializable]
    public struct RoomPresetUnitPlacement
    {
        public Vector2Int LocalPosition;
        public RoomPresetUnitType UnitType;
    }

    [CreateAssetMenu(menuName = "Dungeon/Room Preset", fileName = "RoomPreset")]
    public class RoomPreset : ScriptableObject
    {
        public RoomSizeType Size = RoomSizeType.OneByOne;
        public RoomPresetRole Role = RoomPresetRole.Normal;
        public List<RoomPresetDoor> Doors = new();
        public List<RoomPresetUnitPlacement> Units = new();
    }
}
