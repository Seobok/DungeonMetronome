using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    [SerializeField] public int x, y;
    [SerializeField] protected Sprite sprite;
    protected SpriteRenderer spriteRenderer;
    public Room parentRoom;
    public Unit onTileUnit;
    public Player OnTilePlayer = null;

    #region AStar
    public int fCost { get { return gCost + hCost; } }
    public int gCost;
    public int hCost;
    public Tile astarParent;
    #endregion

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = sprite;
    }
}
