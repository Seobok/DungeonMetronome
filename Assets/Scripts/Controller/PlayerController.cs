using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    private float playerSpeed = 1.0f;
    private SpriteRenderer spriteRenderer;
    private Player player;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GetComponent<Player>();
    }

    private void OnMove(InputValue inputValue)
    {
        //임시 코드
        if(player.curRoom == null)
        {
            InitPlayerPosition();
            return;
        }
        var input = inputValue.Get<Vector2>();

        int nextXPos = player.RoomX + (int)input.x;
        int nextYPos = player.RoomY + (int)input.y;
        var nextTile = player.curRoom.GetTile(nextXPos, nextYPos);

        //공격가능한 대상이 있는지 확인
        if(player.weapon != null)
        {
            var rangeTiles = player.weapon.GetRange(nextTile);
            List<IDamagable> damagableList = new List<IDamagable>();
            foreach (var rangeTile in rangeTiles)
            {
                if (rangeTile.onTileUnit != null)
                {
                    var damagableTile = rangeTile.onTileUnit.GetComponent<IDamagable>();
                    if (damagableTile != null)
                    {
                        damagableList.Add(damagableTile);
                    }
                }
            }
            if (damagableList.Count > 0)
            {
                player.Attack(damagableList);
                return;
            }
        }
        
        var floorTile = nextTile.GetComponent<Floor>();
        if (floorTile)
        {
            //Tile에 Object가 있으면 상호작용
            if(floorTile.onTileUnit != null)
            {
                
            }
            else
            {
                //이동
                player.GetTile().onTileUnit = null;

                transform.Translate(input.x, input.y, 0);
                player.RoomX = nextXPos;
                player.RoomY = nextYPos;

                if (input.x > 0)
                {
                    spriteRenderer.flipX = false;
                }
                else if (input.x < 0)
                {
                    spriteRenderer.flipX = true;
                }

                player.GetTile().onTileUnit = player;
            }
        }
        
    }

    /// <summary>
    /// DEBUG 필요한 선 처리 작업
    /// </summary>
    public void InitPlayerPosition()
    {
        //플레이어 위치 조정
        player.RoomX = Room.X / 2;
        player.RoomY = Room.Y / 2;
        player.curRoom = DungeonManager.instance.rooms[DungeonManager.DUNGEON_X / 2, DungeonManager.DUNGEON_Y / 2];
        transform.position = player.GetTile().transform.position;
        player.GetTile().onTileUnit = player;

        //훈련용 봇 위치 조정
        GameManager.instance.GenerateTrainingBot();
    }
}
