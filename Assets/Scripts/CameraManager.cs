using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [HideInInspector]public Player player;

    void Update()
    {
        if(player != null)
        {
            Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);
        }
    }
}
