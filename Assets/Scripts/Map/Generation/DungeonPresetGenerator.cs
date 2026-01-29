using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map.Generation
{
    [Serializable]
    public class DungeonPresetGenerationConfig
    {
        public List<RoomPreset> NormalPresets = new();
        public List<RoomPreset> ExitPresets = new();
        public int PresetRoomCount = 6;
        public int MinRoomDistance = 4;
        public int MaxRoomDistance = 6;
        public int MaxPlacementAttempts = 50;
        public int ExtraExitConnections = 1;
    }

    public struct DungeonPresetUnitSpawn
    {
        public Coord Position;
        public RoomPresetUnitType UnitType;
    }

    public class DungeonGenerationResult
    {
        public Coord StartPosition { get; init; }
        public RoomInstance StartRoom { get; init; }
        public RoomInstance ExitRoom { get; init; }
        public List<RoomInstance> Rooms { get; init; } = new();
        public List<DungeonPresetUnitSpawn> UnitSpawns { get; init; } = new();
    }

    public class RoomInstance
    {
        public RoomPreset Preset { get; init; }
        public Coord Center { get; init; }
        public RectInt Bounds { get; init; }
        public List<Coord> DoorWorldPositions { get; init; } = new();
    }

    public class DungeonPresetGenerator
    {
        private readonly Dungeon _dungeon;

        public DungeonPresetGenerator(Dungeon dungeon)
        {
            _dungeon = dungeon;
        }

        public DungeonGenerationResult Generate(DungeonPresetGenerationConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (config.NormalPresets.Count == 0) throw new ArgumentException("Normal presets are required.");
            if (config.ExitPresets.Count == 0) throw new ArgumentException("Exit presets are required.");

            _dungeon.ClearTiles();

            RoomInstance startRoom = CreateStartRoom();
            List<RoomInstance> rooms = new List<RoomInstance> { startRoom };
            List<RoomPreset> selectedNormalPresets = PickRandomPresets(config.NormalPresets, config.PresetRoomCount);

            foreach (RoomPreset preset in selectedNormalPresets)
            {
                RoomInstance room = PlaceRoom(config, rooms, preset);
                if (room != null)
                {
                    rooms.Add(room);
                }
            }

            RoomInstance exitRoom = PlaceRoom(config, rooms, PickRandomPresets(config.ExitPresets, 1).First());
            if (exitRoom != null)
            {
                rooms.Add(exitRoom);
            }

            BuildRooms(rooms);
            List<DungeonPresetUnitSpawn> unitSpawns = ApplyUnits(rooms);

            ConnectRoomsSequentially(rooms);
            if (exitRoom != null)
            {
                ConnectExitRoom(startRoom, exitRoom, rooms, config.ExtraExitConnections);
            }

            return new DungeonGenerationResult
            {
                StartPosition = startRoom.Center,
                StartRoom = startRoom,
                ExitRoom = exitRoom,
                Rooms = rooms,
                UnitSpawns = unitSpawns,
            };
        }

        private RoomInstance CreateStartRoom()
        {
            Vector2Int size = GetRoomTileSize(RoomSizeType.OneByOne);
            Coord center = new Coord(0, 0);
            RectInt bounds = BuildBounds(center, size);

            List<Coord> doorPositions = new List<Coord>
            {
                new Coord(center.X, bounds.yMax - 1),
                new Coord(center.X, bounds.yMin),
                new Coord(bounds.xMin, center.Y),
                new Coord(bounds.xMax - 1, center.Y),
            };

            return new RoomInstance
            {
                Preset = null,
                Center = center,
                Bounds = bounds,
                DoorWorldPositions = doorPositions,
            };
        }

        private List<RoomPreset> PickRandomPresets(List<RoomPreset> presets, int count)
        {
            if (count <= 0) return new List<RoomPreset>();
            List<RoomPreset> pool = new List<RoomPreset>(presets);
            List<RoomPreset> selected = new List<RoomPreset>();
            int total = Mathf.Min(count, pool.Count);

            for (int i = 0; i < total; i++)
            {
                int index = UnityEngine.Random.Range(0, pool.Count);
                selected.Add(pool[index]);
                pool.RemoveAt(index);
            }

            return selected;
        }

        private RoomInstance PlaceRoom(DungeonPresetGenerationConfig config, List<RoomInstance> rooms, RoomPreset preset)
        {
            for (int attempt = 0; attempt < config.MaxPlacementAttempts; attempt++)
            {
                RoomInstance anchor = rooms[UnityEngine.Random.Range(0, rooms.Count)];
                Vector2Int offset = PickRandomOffset(config.MinRoomDistance, config.MaxRoomDistance);
                Coord center = new Coord(anchor.Center.X + offset.x * Room.X_LENGTH, anchor.Center.Y + offset.y * Room.Y_LENGTH);
                RectInt bounds = BuildBounds(center, GetRoomTileSize(preset.Size));

                if (!IsWithinDungeonBounds(bounds)) continue;
                if (rooms.Any(room => room.Bounds.Overlaps(bounds))) continue;

                return CreateRoomInstance(preset, center, bounds);
            }

            Debug.LogWarning($"Room preset placement failed: {preset.name}");
            return null;
        }

        private RoomInstance CreateRoomInstance(RoomPreset preset, Coord center, RectInt bounds)
        {
            List<Coord> doorWorldPositions = new List<Coord>();
            if (preset != null)
            {
                foreach (RoomPresetDoor door in preset.Doors)
                {
                    doorWorldPositions.Add(new Coord(center.X + door.LocalPosition.x, center.Y + door.LocalPosition.y));
                }
            }

            return new RoomInstance
            {
                Preset = preset,
                Center = center,
                Bounds = bounds,
                DoorWorldPositions = doorWorldPositions,
            };
        }

        private Vector2Int PickRandomOffset(int minDistance, int maxDistance)
        {
            int distance = UnityEngine.Random.Range(minDistance, maxDistance + 1);
            int dx = UnityEngine.Random.Range(-distance, distance + 1);
            int dy = distance - Mathf.Abs(dx);
            dy *= UnityEngine.Random.value < 0.5f ? -1 : 1;

            if (dx == 0 && dy == 0)
            {
                dx = distance;
            }

            return new Vector2Int(dx, dy);
        }

        private void BuildRooms(List<RoomInstance> rooms)
        {
            foreach (RoomInstance room in rooms)
            {
                HashSet<Coord> doors = new HashSet<Coord>(room.DoorWorldPositions);
                for (int x = room.Bounds.xMin; x < room.Bounds.xMax; x++)
                {
                    for (int y = room.Bounds.yMin; y < room.Bounds.yMax; y++)
                    {
                        Coord coord = new Coord(x, y);
                        bool isEdge = x == room.Bounds.xMin || x == room.Bounds.xMax - 1 || y == room.Bounds.yMin || y == room.Bounds.yMax - 1;
                        bool isDoor = doors.Contains(coord);

                        Tile tile = new Tile
                        {
                            Coord = coord,
                            Status = isEdge && !isDoor ? StatusFlag.Blocked : StatusFlag.Empty,
                        };

                        _dungeon.SetOrRegisterTile(tile);
                    }
                }
            }
        }

        private List<DungeonPresetUnitSpawn> ApplyUnits(List<RoomInstance> rooms)
        {
            List<DungeonPresetUnitSpawn> spawns = new List<DungeonPresetUnitSpawn>();

            foreach (RoomInstance room in rooms)
            {
                if (room.Preset == null) continue;

                foreach (RoomPresetUnitPlacement placement in room.Preset.Units)
                {
                    Coord coord = new Coord(room.Center.X + placement.LocalPosition.x, room.Center.Y + placement.LocalPosition.y);
                    if (!_dungeon.HasTile(coord)) continue;

                    switch (placement.UnitType)
                    {
                        case RoomPresetUnitType.Bat:
                        case RoomPresetUnitType.Slime:
                            spawns.Add(new DungeonPresetUnitSpawn
                            {
                                Position = coord,
                                UnitType = placement.UnitType,
                            });
                            break;
                        case RoomPresetUnitType.Blocker:
                        case RoomPresetUnitType.Item:
                            if (_dungeon.TryGetTile(coord.X, coord.Y, out Tile tile))
                            {
                                tile.Status = StatusFlag.Blocked;
                                _dungeon.SetOrRegisterTile(tile);
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return spawns;
        }

        private void ConnectRoomsSequentially(List<RoomInstance> rooms)
        {
            for (int i = 1; i < rooms.Count; i++)
            {
                RoomInstance from = rooms[i - 1];
                RoomInstance to = rooms[i];
                TryConnectRooms(from, to, rooms);
            }
        }

        private void ConnectExitRoom(RoomInstance startRoom, RoomInstance exitRoom, List<RoomInstance> rooms, int extraConnections)
        {
            RoomInstance farthest = rooms
                .Where(room => room != exitRoom)
                .OrderByDescending(room => ManhattanDistance(startRoom.Center, room.Center))
                .FirstOrDefault();

            if (farthest != null)
            {
                TryConnectRooms(farthest, exitRoom, rooms);
            }

            List<RoomInstance> nearestRooms = rooms
                .Where(room => room != exitRoom)
                .OrderBy(room => ManhattanDistance(exitRoom.Center, room.Center))
                .Take(Mathf.Max(0, extraConnections))
                .ToList();

            foreach (RoomInstance room in nearestRooms)
            {
                TryConnectRooms(room, exitRoom, rooms);
            }
        }

        private void TryConnectRooms(RoomInstance from, RoomInstance to, List<RoomInstance> rooms)
        {
            List<Coord> fromDoors = EnsureDoors(from);
            List<Coord> toDoors = EnsureDoors(to);

            (Coord start, Coord end) = FindClosestDoorPair(fromDoors, toDoors);
            if (TryCreateCorridor(start, end, rooms))
            {
                return;
            }

            foreach (Coord fromDoor in fromDoors)
            {
                foreach (Coord toDoor in toDoors)
                {
                    if (TryCreateCorridor(fromDoor, toDoor, rooms))
                    {
                        return;
                    }
                }
            }

            Debug.LogWarning($"Failed to connect rooms at {from.Center.X},{from.Center.Y} and {to.Center.X},{to.Center.Y}");
        }

        private List<Coord> EnsureDoors(RoomInstance room)
        {
            if (room.DoorWorldPositions.Count > 0)
            {
                return room.DoorWorldPositions;
            }

            List<Coord> defaultDoors = new List<Coord>
            {
                new Coord(room.Center.X, room.Bounds.yMax - 1),
                new Coord(room.Center.X, room.Bounds.yMin),
                new Coord(room.Bounds.xMin, room.Center.Y),
                new Coord(room.Bounds.xMax - 1, room.Center.Y),
            };

            return defaultDoors;
        }

        private (Coord start, Coord end) FindClosestDoorPair(List<Coord> fromDoors, List<Coord> toDoors)
        {
            Coord bestStart = fromDoors[0];
            Coord bestEnd = toDoors[0];
            int bestDistance = int.MaxValue;

            foreach (Coord fromDoor in fromDoors)
            {
                foreach (Coord toDoor in toDoors)
                {
                    int distance = ManhattanDistance(fromDoor, toDoor);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestStart = fromDoor;
                        bestEnd = toDoor;
                    }
                }
            }

            return (bestStart, bestEnd);
        }

        private bool TryCreateCorridor(Coord startDoor, Coord endDoor, List<RoomInstance> rooms)
        {
            if (!TryGetOutsideDoor(startDoor, rooms, out Coord startOutside) ||
                !TryGetOutsideDoor(endDoor, rooms, out Coord endOutside))
            {
                return false;
            }

            if (!IsWithinDungeonBounds(startOutside) || !IsWithinDungeonBounds(endOutside))
            {
                return false;
            }

            HashSet<Coord> allowed = new HashSet<Coord> { startDoor, endDoor };
            if (TryBuildSingleBendPath(startOutside, endOutside, rooms, allowed, out List<Coord> path))
            {
                CarveCorridor(startDoor, startOutside, path, endOutside, endDoor);
                return true;
            }

            if (TryBuildTwoBendPath(startOutside, endOutside, rooms, allowed, out path))
            {
                CarveCorridor(startDoor, startOutside, path, endOutside, endDoor);
                return true;
            }

            return false;
        }

        private bool TryGetOutsideDoor(Coord door, List<RoomInstance> rooms, out Coord outside)
        {
            foreach (RoomInstance room in rooms)
            {
                if (!room.Bounds.Contains(new Vector2Int(door.X, door.Y))) continue;

                if (door.Y == room.Bounds.yMax - 1)
                {
                    outside = new Coord(door.X, door.Y + 1);
                    return true;
                }

                if (door.Y == room.Bounds.yMin)
                {
                    outside = new Coord(door.X, door.Y - 1);
                    return true;
                }

                if (door.X == room.Bounds.xMin)
                {
                    outside = new Coord(door.X - 1, door.Y);
                    return true;
                }

                if (door.X == room.Bounds.xMax - 1)
                {
                    outside = new Coord(door.X + 1, door.Y);
                    return true;
                }

                Debug.LogWarning($"Door at {door.X},{door.Y} is not on room edge.");
                break;
            }

            outside = default;
            return false;
        }

        private bool TryBuildSingleBendPath(Coord start, Coord end, List<RoomInstance> rooms, HashSet<Coord> allowed, out List<Coord> path)
        {
            Coord cornerA = new Coord(end.X, start.Y);
            if (IsPathClear(start, cornerA, rooms, allowed) && IsPathClear(cornerA, end, rooms, allowed))
            {
                path = BuildPath(start, cornerA, end);
                return true;
            }

            Coord cornerB = new Coord(start.X, end.Y);
            if (IsPathClear(start, cornerB, rooms, allowed) && IsPathClear(cornerB, end, rooms, allowed))
            {
                path = BuildPath(start, cornerB, end);
                return true;
            }

            path = null;
            return false;
        }

        private bool TryBuildTwoBendPath(Coord start, Coord end, List<RoomInstance> rooms, HashSet<Coord> allowed, out List<Coord> path)
        {
            int detourRange = 6;
            List<int> offsets = Enumerable.Range(-detourRange, detourRange * 2 + 1)
                .Where(value => value != 0)
                .OrderBy(_ => UnityEngine.Random.value)
                .ToList();

            foreach (int offset in offsets)
            {
                Coord mid1 = new Coord(start.X + offset, start.Y);
                Coord mid2 = new Coord(start.X + offset, end.Y);

                if (IsPathClear(start, mid1, rooms, allowed) &&
                    IsPathClear(mid1, mid2, rooms, allowed) &&
                    IsPathClear(mid2, end, rooms, allowed))
                {
                    path = BuildPath(start, mid1, mid2, end);
                    return true;
                }
            }

            foreach (int offset in offsets)
            {
                Coord mid1 = new Coord(start.X, start.Y + offset);
                Coord mid2 = new Coord(end.X, start.Y + offset);

                if (IsPathClear(start, mid1, rooms, allowed) &&
                    IsPathClear(mid1, mid2, rooms, allowed) &&
                    IsPathClear(mid2, end, rooms, allowed))
                {
                    path = BuildPath(start, mid1, mid2, end);
                    return true;
                }
            }

            path = null;
            return false;
        }

        private List<Coord> BuildPath(params Coord[] points)
        {
            List<Coord> path = new List<Coord>();

            for (int i = 0; i < points.Length - 1; i++)
            {
                foreach (Coord point in EnumerateLine(points[i], points[i + 1]))
                {
                    if (path.Count == 0 || path[^1] != point)
                    {
                        path.Add(point);
                    }
                }
            }

            return path;
        }

        private IEnumerable<Coord> EnumerateLine(Coord start, Coord end)
        {
            if (start.X == end.X)
            {
                int step = start.Y <= end.Y ? 1 : -1;
                for (int y = start.Y; y != end.Y + step; y += step)
                {
                    yield return new Coord(start.X, y);
                }
                yield break;
            }

            if (start.Y == end.Y)
            {
                int step = start.X <= end.X ? 1 : -1;
                for (int x = start.X; x != end.X + step; x += step)
                {
                    yield return new Coord(x, start.Y);
                }
                yield break;
            }

            throw new ArgumentException("Only axis-aligned lines are supported.");
        }

        private bool IsPathClear(Coord start, Coord end, List<RoomInstance> rooms, HashSet<Coord> allowed)
        {
            foreach (Coord point in EnumerateLine(start, end))
            {
                if (allowed.Contains(point)) continue;

                foreach (RoomInstance room in rooms)
                {
                    if (room.Bounds.Contains(new Vector2Int(point.X, point.Y)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void CarveCorridor(Coord startDoor, Coord startOutside, List<Coord> path, Coord endOutside, Coord endDoor)
        {
            List<Coord> corridor = new List<Coord> { startDoor, startOutside };
            corridor.AddRange(path);
            corridor.Add(endOutside);
            corridor.Add(endDoor);

            foreach (Coord coord in corridor.Distinct())
            {
                if (!IsWithinDungeonBounds(coord)) continue;

                Tile tile = new Tile
                {
                    Coord = coord,
                    Status = StatusFlag.Empty,
                };
                _dungeon.SetOrRegisterTile(tile);
            }
        }

        private RectInt BuildBounds(Coord center, Vector2Int size)
        {
            int minX = center.X - size.x / 2;
            int minY = center.Y - size.y / 2;
            return new RectInt(minX, minY, size.x, size.y);
        }

        private Vector2Int GetRoomTileSize(RoomSizeType sizeType)
        {
            return sizeType switch
            {
                RoomSizeType.OneByOne => new Vector2Int(Room.X_LENGTH, Room.Y_LENGTH),
                RoomSizeType.TwoByOne => new Vector2Int(Room.X_LENGTH * 2, Room.Y_LENGTH),
                RoomSizeType.OneByTwo => new Vector2Int(Room.X_LENGTH, Room.Y_LENGTH * 2),
                RoomSizeType.TwoByTwo => new Vector2Int(Room.X_LENGTH * 2, Room.Y_LENGTH * 2),
                _ => throw new ArgumentOutOfRangeException(nameof(sizeType), sizeType, null),
            };
        }

        private int ManhattanDistance(Coord a, Coord b)
        {
            return Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y);
        }

        private bool IsWithinDungeonBounds(RectInt bounds)
        {
            int minX = -(Dungeon.DUNGEON_X * Room.X_LENGTH / 2);
            int maxX = Dungeon.DUNGEON_X * Room.X_LENGTH / 2;
            int minY = -(Dungeon.DUNGEON_Y * Room.Y_LENGTH / 2);
            int maxY = Dungeon.DUNGEON_Y * Room.Y_LENGTH / 2;

            return bounds.xMin >= minX &&
                   bounds.yMin >= minY &&
                   bounds.xMax <= maxX &&
                   bounds.yMax <= maxY;
        }

        private bool IsWithinDungeonBounds(Coord coord)
        {
            int minX = -(Dungeon.DUNGEON_X * Room.X_LENGTH / 2);
            int maxX = Dungeon.DUNGEON_X * Room.X_LENGTH / 2;
            int minY = -(Dungeon.DUNGEON_Y * Room.Y_LENGTH / 2);
            int maxY = Dungeon.DUNGEON_Y * Room.Y_LENGTH / 2;

            return coord.X >= minX && coord.X < maxX && coord.Y >= minY && coord.Y < maxY;
        }
    }
}
