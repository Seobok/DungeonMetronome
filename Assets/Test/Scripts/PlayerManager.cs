using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    float playerSpeed = 5.0f;

    private void Update()
    {
        PlayerInputSystem();
    }

    private void PlayerInputSystem()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * playerSpeed * Time.deltaTime);
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.right * playerSpeed * Time.deltaTime);
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * playerSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down * playerSpeed * Time.deltaTime);
        }
    }
}
