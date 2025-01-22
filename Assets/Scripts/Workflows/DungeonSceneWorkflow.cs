using System.Collections;
using Controller;
using Map;
using Unit.Enemy;
using Unit.Player;
using UnityEngine;

namespace Workflows
{
    public class DungeonSceneWorkflow : MonoBehaviour
    {
        private Dungeon _dungeon;
        private PlayerController _playerController;
        private Bat _bat;

        
        private void Start()
        {
            Init();
            
            StartCoroutine(C_Workflow());
        }

        private void Init()
        {
            _dungeon = new Dungeon();
            
            PlayerController playerControllerPrefab = Resources.Load<PlayerController>("Prefabs/Character/Knight");
            _playerController = Instantiate(playerControllerPrefab);
            _playerController.Workflow = this;

            _bat = new Bat();
        }
        
        private IEnumerator C_Workflow()
        {
            _dungeon.ActivateDungeon(7);
            
            _playerController.transform.position = _dungeon.StartRoom.GetTile(Room.X_LENGTH/2, Room.Y_LENGTH/2).Position;
            _playerController.Knight.CurRoom = _dungeon.StartRoom;
            _playerController.Knight.PosX = Room.X_LENGTH/2;
            _playerController.Knight.PosY = Room.Y_LENGTH/2;
            
            _bat.InitPosition(_dungeon.StartRoom.GetTile(Room.X_LENGTH/2 + 3, Room.Y_LENGTH/2 + 3));
            yield return null;
        }

        public void NextTurn()
        {
            _bat.Act();
        }
    }
}
