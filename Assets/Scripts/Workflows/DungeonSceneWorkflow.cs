using System;
using System.Collections;
using Controller;
using Map;
using Unit;
using Unit.Enemy;
using Unit.Player;
using UnityEngine;
using VContainer;

namespace Workflows
{
    public class DungeonSceneWorkflow : MonoBehaviour
    {
        private Dungeon _dungeon;
        private UnitManager _unitManager;

        private Bat _bat;


        [Inject]
        public void Construct(Dungeon dungeon, UnitManager unitManager)
        {
            _dungeon = dungeon;
            _unitManager = unitManager;
        }
        
        private void Start()
        {
            _dungeon.ActivateDungeon(7);

            Knight knight = _unitManager.SpawnKnight(0, 0);
            knight.PlayerController.NextTurn += NextTurn;

            _bat = (Bat)_unitManager.SpawnEnemy(typeof(Bat), 3, 3);
        }

        public void NextTurn()
        {
            _bat.Act();
        }
    }
}
