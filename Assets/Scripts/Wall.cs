using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class Wall : Tile
{
    [SerializeField] private Sprite backSprite;

    public void SetWallSprite()
    {
        var tile = parentRoom.GetTile(x, y - 1);

        if (tile)
        {
            var wallObject = tile.GetComponent<Wall>();
            if (wallObject != null)
            {
                spriteRenderer.sprite = backSprite;
            }
            else
            {
                spriteRenderer.sprite = sprite;
            }
        }
        else
        {
            spriteRenderer.sprite = sprite;
        }
    }


}
