using System;
using System.Collections;
using Map;
using UnityEngine;

namespace Workflows
{
    public class DungeonSceneWorkflow : MonoBehaviour
    {
        private Dungeon _dungeon;

        private void Start()
        {
            Init();
            
            StartCoroutine(C_Workflow());
        }

        private void Init()
        {
            GameObject dungeonObject = new GameObject("Dungeon");
            _dungeon = dungeonObject.AddComponent<Dungeon>();
        }
        
        private IEnumerator C_Workflow()
        {
            _dungeon.GenerateDungeon(7);
            yield return null;
        }
    }
}
