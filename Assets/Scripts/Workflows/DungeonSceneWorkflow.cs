using System.Collections;
using Controller;
using Map;
using Unit;
using Unit.Enemy;
using Unit.Player;
using UnityEngine;

namespace Workflows
{
    public class DungeonSceneWorkflow : MonoBehaviour
    {
        private Dungeon _dungeon;
        private UnitManager _unitManager;

        private Bat _bat;

        
        private void Start()
        {
            Init();
            
            _dungeon.ActivateDungeon(7);

            Knight knight = _unitManager.SpawnKnight(0, 0);
            PlayerController playerController = _unitManager.KnightGameObject.GetComponent<PlayerController>();
            playerController.Workflow = this;
            playerController.Knight = knight;

            _bat = (Bat)_unitManager.SpawnEnemy(typeof(Bat), 3, 3);
        }

        private void Init()
        {
            _dungeon = new Dungeon();
            _unitManager = new UnitManager(_dungeon);
        }

        public void NextTurn()
        {
            _bat.Act();
        }
    }
}
