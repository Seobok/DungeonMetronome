using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    private float playerSpeed = 1.0f;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInputSystem();
    }

    private void PlayerInputSystem()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * playerSpeed);
            spriteRenderer.flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * playerSpeed);
            spriteRenderer.flipX = false;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * playerSpeed);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down * playerSpeed);
        }
    }
}
