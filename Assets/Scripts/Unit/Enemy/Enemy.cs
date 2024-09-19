using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    protected int detactRange = 3;
    protected bool bIsReadyMove = false;
    protected bool bIsReadyAttack = false;

    protected List<Vector2> upDirRange = new List<Vector2>();
    protected List<Vector2> rightDirRange = new List<Vector2>();
    protected List<Vector2> downDirRange = new List<Vector2>();
    protected List<Vector2> leftDirRange = new List<Vector2>();

    protected List<Vector2> detactTiles = new List<Vector2>();
    protected List<Tile> attackTiles = new List<Tile>();
    protected Tile PlayerTile = null;
    protected Tile moveTile = null;
    protected int moveMaxCnt = 1;
    protected int moveCnt = 1;

    private void Start()
    {
        //detectRange를 기반으로 탐지가 가능한 범위를 정하는 코드
        for (int i = -detactRange; i <= detactRange; i++)
        {
            for (int j = -detactRange; j <= detactRange; j++)
            {
                if (Mathf.Abs(i) + Mathf.Abs(j) > detactRange) continue;

                detactTiles.Add(new Vector2(i, j));
            }
        }

        //rightDirRange 설정
        rightDirRange.Add(new Vector2(1, 0));

        //나머지 공격범위 설정
        foreach(var rightDir in  rightDirRange)
        {
            Vector2 upDir = new Vector2(rightDir.y, rightDir.x);
            upDirRange.Add(upDir);
            Vector2 leftDir = new Vector2(-rightDir.x, rightDir.y);
            leftDirRange.Add(leftDir);
            Vector2 downDir = new Vector2(upDir.x, -upDir.y);
            downDirRange.Add(downDir);
        }
    }

    /// <summary>
    /// 기본적인 행동 패턴 알고리즘
    /// </summary>
    public void Act()
    {
        Debug.Log("InAct");
        if(bIsReadyAttack)
        {
            Attack();

            //만약 공격했다면 이동하지 않음
            bIsReadyMove = false;
        }
        if(bIsReadyMove)
        {
            Move();
        }

        if(Detact())
        {
            if(Range())
            {
                bIsReadyAttack = true;
                return;
            }

            MoveToPlayer();
            return;
        }

        MoveToRandom();
    }

    /// <summary>
    /// detectRange에 따라 해당 거리 안에 플레이어가 있는지 확인하는 코드
    /// </summary>
    /// <returns>플레이어가 존재하면 true, 아니면 false를 반환</returns>
    public bool Detact()
    {
        //Detect Tiles을 기반으로 탐지 범위 내의 tile을 찾는 코드
        var tiles = curRoom.GetTiles(GetTile(), detactTiles);
        foreach (var tile in tiles)
        {
            if (tile.bisOnTilePlayer)
            {
                PlayerTile = tile;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// rightDirRange를 기반으로 공격 사거리 내에 플레이어가 존재하는지 확인하는 코드
    /// 만약 플레이어가 발견되면 해당 방향의 공격 범위를 attackTiles에 넣고 공격을 준비함
    /// </summary>
    /// <returns>플레이어가 있으면 true, 아니면 false를 반환</returns>
    public bool Range()
    {
        //오른쪽 범위를 확인하기
        var tiles = curRoom.GetTiles(GetTile(), rightDirRange);
        foreach (var tile in tiles)
        {
            if (tile.bisOnTilePlayer)
            {
                attackTiles = tiles;
                return true;
            }
        }

        tiles = curRoom.GetTiles(GetTile(), upDirRange);
        foreach (var tile in tiles)
        {
            if(tile.bisOnTilePlayer)
            {
                attackTiles = tiles;
                return true;
            }
        }

        tiles = curRoom.GetTiles(GetTile(), downDirRange);
        foreach (var tile in tiles)
        {
            if (tile.bisOnTilePlayer)
            {
                attackTiles = tiles;
                return true;
            }
        }

        tiles = curRoom.GetTiles(GetTile(), leftDirRange);
        foreach (var tile in tiles)
        {
            if (tile.bisOnTilePlayer)
            {
                attackTiles = tiles;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// PlayerTile방향으로 moveCnt만큼 움직인 타일을 moveTile에 저장하는 코드
    /// 해당 코드가 실행되면 bIsReadyMove가 true로 설정되어 다음 Act에서 공격하지 않는다면 무조건 이동이 실행됨
    /// </summary>
    public void MoveToPlayer()
    {
        if (PlayerTile == null)
        {
            //플레이어가 어디있는지 모르면 RandomMove
            MoveToRandom();
            return;
        }

        moveTile = GetTile();

        var path = AStar.FindPath(moveTile, PlayerTile);
        if(path == null)
        {
            //길이 막혀있으면 랜덤이동으로 변경
            MoveToRandom();
            return;
        }

        while(moveCnt > 0 || path.Count != 0)
        {
            moveTile = path.Pop();
            moveCnt--;
        }
        moveCnt = moveMaxCnt;

        bIsReadyMove = true;
        moveTile.GetComponent<SpriteRenderer>().color = Color.green;
    }
    public void MoveToRandom()
    {
        moveTile = GetTile();

        while (moveCnt > 0)
        {
            var dir = Random.Range(0, 4);
            switch(dir)
            {
                case (int)E_Dir.ED_Up:
                    moveTile = moveTile.parentRoom.GetTile(moveTile.x, moveTile.y + 1);
                    break;
                case (int)E_Dir.ED_Right:
                    moveTile = moveTile.parentRoom.GetTile(moveTile.x + 1, moveTile.y);
                    break;
                case (int) E_Dir.ED_Down:
                    moveTile = moveTile.parentRoom.GetTile(moveTile.x, moveTile.y - 1);
                    break;
                case (int)E_Dir.ED_Left:
                    moveTile = moveTile.parentRoom.GetTile(moveTile.x - 1, moveTile.y);
                    break;
            }
            moveCnt--;
        }
        moveCnt = moveMaxCnt;
        bIsReadyMove = true;
        moveTile.GetComponent<SpriteRenderer>().color = Color.green;
    }

    public void Attack()
    {
        Debug.Log("Attack");

        //TODO : ATTACK

        bIsReadyAttack = false;
    }

    public void Move()
    {
        Debug.Log("Move");

        //TODO : MOVE
        GetTile().onTileUnit = null;

        RoomX = moveTile.x;
        RoomY = moveTile.y;
        gameObject.transform.DOMove(moveTile.transform.position, 0.2f).SetEase(Ease.InOutCubic);

        GetTile().onTileUnit = this;

        bIsReadyMove = false;

        moveTile.GetComponent<SpriteRenderer>().color = Color.white;
        moveTile = null;
    }
}
