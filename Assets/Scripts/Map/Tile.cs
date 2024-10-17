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
    private Unit _onTileUnit = null;
    public Unit onTileUnit
    {
        get { return _onTileUnit; }
        set 
        {
            _onTileUnit = value;

            //FogOfWar 관련 처리
            if(_onTileUnit != null)
            {
                if(!isVisible)
                {
                    if (_onTileUnit.GetComponent<Block>() != null) return;

                    _onTileUnit.GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    _onTileUnit.GetComponent<SpriteRenderer>().enabled = true;

                    
                }
            }
        }
    }
    public Player onTilePlayer = null;

    [Header("AStar")]
    public Tile astarParent;
    public int fCost { get { return gCost + hCost; } }
    public int gCost;
    public int hCost;

    [Header("FogOfWar")]
    [SerializeField] private SpriteRenderer fogSprite;
    private bool _isVisible = false;
    private bool _isVisit = false;
    public bool isVisible
    {
        get { return _isVisible; }
        set 
        {
            _isVisible = value;
            if (_isVisible)
            {
                _isVisit = true;
                fogSprite.color = new Color(0, 0, 0, 0);

                if (_onTileUnit != null)
                {
                    var unitSprite = _onTileUnit.GetComponent<SpriteRenderer>();
                    if(unitSprite != null)
                    {
                        unitSprite.enabled = true;
                    }
                }
            }
            else
            {
                if(_isVisit)
                {
                    fogSprite.color = new Color(0, 0, 0, 0.5f);
                }

                if (_onTileUnit != null)
                {
                    if (_onTileUnit.GetComponent<Block>() != null) return;

                    var unitSprite = _onTileUnit.GetComponent<SpriteRenderer>();
                    if (unitSprite != null)
                    {
                        unitSprite.enabled = false;
                    }
                }
            }
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = sprite;
    }
}
