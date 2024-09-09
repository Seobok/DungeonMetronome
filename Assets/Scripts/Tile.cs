using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    [HideInInspector] public int x, y;
    [SerializeField] protected Sprite sprite;
    protected SpriteRenderer spriteRenderer;
    public Room parentRoom;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = sprite;
    }
}
