using System;
using System.Collections;
using UnityEngine;

namespace Workflows
{
    public class DungeonSceneWorkflow : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(C_Workflow());
        }

        private IEnumerator C_Workflow()
        {
            CreateRoom();
            yield return null;
        }

        private void CreateRoom()
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Map/Room"), Vector3.zero, Quaternion.identity);
        }

        private void SpawnPlayer()
        {
            
        }
    }
}
