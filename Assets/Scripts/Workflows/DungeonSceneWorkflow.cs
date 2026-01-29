using Controller;
using Effect;
using Map;
using Map.Generation;
using Unit;
using Unit.Player;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Workflows
{
    public class DungeonSceneWorkflow : MonoBehaviour
    {
        private Dungeon _dungeon;
        private UnitManager _unitManager;
        private EffectPool _effectPool;

        [SerializeField] private bool usePresetGenerator = true;
        [SerializeField] private DungeonPresetGenerationConfig presetConfig = new DungeonPresetGenerationConfig();

        [SerializeField] private bool isTutorial;
        [SerializeField] private TutorialDungeonLayout tutorialLayout;
        [SerializeField] private string tutorialLayoutResourcePath = "Layouts/TutorialDungeonLayout_Tutorial1";


        [Inject]
        public void Construct(Dungeon dungeon, UnitManager unitManager, EffectPool effectPool)
        {
            _effectPool = effectPool;
            _dungeon = dungeon;
            _unitManager = unitManager;
        }
        
        private void Start()
        {
            _effectPool.Init();
            if (usePresetGenerator && presetConfig.NormalPresets.Count > 0 && presetConfig.ExitPresets.Count > 0)
            {
                DungeonPresetGenerator generator = new DungeonPresetGenerator(_dungeon);
                DungeonGenerationResult result = generator.Generate(presetConfig);
                Knight knight = _unitManager.SpawnKnight(result.StartPosition.X, result.StartPosition.Y);
                knight.PlayerController.NextTurn += NextTurn;

                foreach (DungeonPresetUnitSpawn spawn in result.UnitSpawns)
                {
                    if (TryGetSpawnType(spawn.UnitType, out UnitSpawnType spawnType))
                    {
                        _unitManager.SpawnEnemy(spawnType, spawn.Position.X, spawn.Position.Y);
                    }
                }

                return;
            }

            DungeonGenerationMode generationMode = isTutorial
                ? DungeonGenerationMode.TutorialPreset
                : DungeonGenerationMode.RandomExpand;
            _dungeon.ActivateDungeon(7, generationMode);

            TutorialDungeonLayout activeLayout = null;
            if (isTutorial)
            {
                activeLayout = tutorialLayout != null
                    ? tutorialLayout
                    : Resources.Load<TutorialDungeonLayout>(tutorialLayoutResourcePath);

                if (activeLayout == null)
                {
                    Debug.LogError("Tutorial layout asset is missing. Falling back to default spawn.");
                }
                else
                {
                    _dungeon.GenerateTutorialLayout(activeLayout);
                }
            }

            Coord playerSpawn = activeLayout != null ? activeLayout.playerSpawn : Coord.Zero;
            Knight fallbackKnight = _unitManager.SpawnKnight(playerSpawn.X, playerSpawn.Y);
            fallbackKnight.PlayerController.NextTurn += NextTurn;

            if (isTutorial && activeLayout != null)
            {
                foreach (UnitSpawn spawn in activeLayout.unitSpawns)
                {
                    Coord spawnCoord = spawn.ToCoord();
                    _unitManager.SpawnEnemy(spawn.unitType, spawnCoord.X, spawnCoord.Y);
                }
            }
            else
            {
                // 적 15마리 스폰
                for (int i = 0; i < 15; i++)
                {
                    Coord spawnPosition = _dungeon.GetRandomEmptyTileFromRooms(100);
                    
                    if (spawnPosition != Coord.Zero) // 적절한 위치를 찾은 경우
                    {
                        int random = Random.Range(0, 2);
                        switch (random)
                        {
                            case 0:
                                _unitManager.SpawnEnemy(UnitSpawnType.Bat, spawnPosition.X, spawnPosition.Y);
                                break;
                            case 1:
                                _unitManager.SpawnEnemy(UnitSpawnType.Slime, spawnPosition.X, spawnPosition.Y);
                                break;
                        }
                        
                    }
                    else
                    {
                        Debug.LogWarning("더 이상 적을 스폰할 수 있는 빈 공간이 없습니다.");
                        break;
                    }
                }
            }
        }

        public void NextTurn()
        {
            _unitManager.ActOnAllEnemies();
        }

        private bool TryGetSpawnType(RoomPresetUnitType unitType, out UnitSpawnType spawnType)
        {
            switch (unitType)
            {
                case RoomPresetUnitType.Bat:
                    spawnType = UnitSpawnType.Bat;
                    return true;
                case RoomPresetUnitType.Slime:
                    spawnType = UnitSpawnType.Slime;
                    return true;
                default:
                    spawnType = default;
                    return false;
            }
        }

    }
}
