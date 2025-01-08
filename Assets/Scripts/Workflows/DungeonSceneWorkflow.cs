using System.Collections;
using Map;
using Unit.Player;
using UnityEngine;

namespace Workflows
{
    public class DungeonSceneWorkflow : MonoBehaviour
    {
        private Knight _knightPrefab;
        
        private Dungeon _dungeon;
        private Knight _knight;

        
        private void Start()
        {
            Init();
            
            StartCoroutine(C_Workflow());
        }

        private void Init()
        {
            GameObject dungeonObject = new GameObject("Dungeon");
            _dungeon = dungeonObject.AddComponent<Dungeon>();

            if (!_knightPrefab)
                _knightPrefab = Resources.Load<Knight>("Prefabs/Character/Knight");
            _knight = Instantiate(_knightPrefab);
        }
        
        private IEnumerator C_Workflow()
        {
            _dungeon.ActivateDungeon(7);
            _knight.transform.position = _dungeon.StartRoom.GetTile(Room.X_LENGTH/2, Room.Y_LENGTH/2).transform.position;
            _knight.CurRoom = _dungeon.StartRoom;
            _knight.PosX = Room.X_LENGTH/2;
            _knight.PosY = Room.Y_LENGTH/2;
            yield return null;
        }
    }
}
