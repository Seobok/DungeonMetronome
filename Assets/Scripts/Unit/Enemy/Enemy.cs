using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy 기초 클래스
/// [초기화 목록]
/// detectRange : Enemy로 부터 detectRange칸 만큼 떨어진 플레이어를 탐지할 수 있음
/// moveMaxCnt : 한번 이동할때 몇 칸 이동할수 있는지 나타내는 변수
/// moveCnt : moveMaxCnt와 값이 같도록 초기화
/// attackDamage : Enemy가 공격할 때 입히는 피해량
/// rightDirRange : Enemy가 오른쪽을 보고있다고 가정했을 때의 공격범위
/// </summary>
public class Enemy : Unit
{
    [Header("EnemyProperty")]
    protected int detactRange = 3;
    protected int moveMaxCnt = 1;
    protected int moveCnt = 1;
    protected int attackDamage = 1;

    [Header("ActFlag")]
    protected bool bIsReadyMove = false;
    protected bool bIsReadyAttack = false;

    [Header("AttackRangeTiles")]
    protected List<Vector2> upDirRange = new List<Vector2>();
    protected List<Vector2> rightDirRange = new List<Vector2>();
    protected List<Vector2> downDirRange = new List<Vector2>();
    protected List<Vector2> leftDirRange = new List<Vector2>();

    [Header("DetactRangeTile")]
    protected List<Vector2> detactTiles = new List<Vector2>();

    [Header("RequireToAct")]
    protected List<Tile> attackTiles = new List<Tile>();
    protected Tile PlayerTile = null;
    protected Tile moveTile = null;

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
        //rightDirRange 설정 -> 초기화를 통해 설정
        if(rightDirRange.Count == 0)
        {
            rightDirRange.Add(new Vector2(1, 0));
        }

        //나머지 공격범위 설정
        foreach (var rightDir in rightDirRange)
        {
            upDirRange.Add(new Vector2(rightDir.y, rightDir.x));
            leftDirRange.Add(new Vector2(-rightDir.x, rightDir.y));
            downDirRange.Add(new Vector2(rightDir.y, -rightDir.x));
        }
    }

    /// <summary>
    /// 기본적인 행동 패턴 알고리즘
    /// </summary>
    public void Act()
    {
        if(bIsReadyAttack)
        {
            Attack();

            //만약 공격했다면 이동하지 않음
            bIsReadyMove = false;

            //공격을 한 직후에는 다음 행동 준비를 할 수 없음
            return;
        }
        if(bIsReadyMove)
        {
            Move();

            //임시) 움직인 직후에는 행동 준비를 할 수 없음
            return;
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
            if (tile.OnTilePlayer != null)
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
            if (tile.OnTilePlayer != null)
            {
                attackTiles = tiles;
                ShowAttackTile();
                return true;
            }
        }

        tiles = curRoom.GetTiles(GetTile(), upDirRange);
        foreach (var tile in tiles)
        {
            if(tile.OnTilePlayer != null)
            {
                attackTiles = tiles;
                ShowAttackTile();
                return true;
            }
        }

        tiles = curRoom.GetTiles(GetTile(), downDirRange);
        foreach (var tile in tiles)
        {
            if (tile.OnTilePlayer != null)
            {
                attackTiles = tiles;
                ShowAttackTile();
                return true;
            }
        }

        tiles = curRoom.GetTiles(GetTile(), leftDirRange);
        foreach (var tile in tiles)
        {
            if (tile.OnTilePlayer != null)
            {
                attackTiles = tiles;
                ShowAttackTile();
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

        var path = AStar.FindPath(GetTile(), PlayerTile);
        if(path == null)
        {
            //길이 막혀있으면 랜덤이동으로 변경
            MoveToRandom();
            return;
        }

        moveTile = GetTile();
        while (moveCnt > 0)
        {
            if (path.Count == 0) break;
            moveTile = path.Pop();
            moveCnt--;
        }
        moveCnt = moveMaxCnt;

        bIsReadyMove = true;
        ShowMoveTile();
    }
    public void MoveToRandom()
    {
        moveTile = GetTile();

        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { 1, 0, -1, 0 };

        while (moveCnt > 0)
        {
            var nextTileList = new List<Tile>();
            for(int i=0;i<4;i++)
            {
                var nextTile = moveTile.parentRoom.GetTile(moveTile.x + dx[i], moveTile.y + dy[i]);
                if (nextTile == null)
                    continue;

                if(nextTile.onTileUnit == null)
                    nextTileList.Add(nextTile);
            }
            if(nextTileList.Count > 0)
            {
                moveTile = nextTileList[Random.Range(0, nextTileList.Count)];
                moveCnt--;
            }
        }
        moveCnt = moveMaxCnt;
        bIsReadyMove = true;
        ShowMoveTile();
    }

    public void Attack()
    {
        //일단 그냥 때리는걸로 하고 나중에 방패같은거 생기면 데미지 반감? 이 밸런스 고려했을때 적절할듯
        foreach(var tile in attackTiles)
        {
            if(tile.OnTilePlayer != null)
            {
                tile.OnTilePlayer.Damaged(1, this);
            }
        }

        ReturnAttackTile();
        bIsReadyAttack = false;
    }

    public void Move()
    {
        if(moveTile.OnTilePlayer != null)
        {
            //움직이는 대신 공격하기
            //이때는 AttackDamage에 상관없이 1의 피해를 입힘
            moveTile.OnTilePlayer.Damaged(1, this);

            gameObject.transform.DOMove(new Vector3((moveTile.transform.position.x + gameObject.transform.position.x) / 2, (moveTile.transform.position.y + gameObject.transform.position.y) / 2, transform.position.z), 0.1f).SetLoops(2, LoopType.Yoyo);

            bIsReadyMove = false;

            ReturnMoveTile();
            moveTile = null;

            return;
        }

        if(moveTile.onTileUnit == null)
        {
            GetTile().onTileUnit = null;

            curRoom = moveTile.parentRoom;
            RoomX = moveTile.x;
            RoomY = moveTile.y;

            GetTile().onTileUnit = this;
        }

        gameObject.transform.DOMoveX(GetTile().transform.position.x, 0.2f).SetEase(Ease.InOutCubic);
        gameObject.transform.DOMoveY(gameObject.transform.position.y + 0.5f, 0.1f).OnComplete(() => gameObject.transform.DOMoveY(GetTile().transform.position.y, 0.1f));

        bIsReadyMove = false;

        ReturnMoveTile();
        moveTile = null;
    }

    public void ShowMoveTile()
    {
        if(moveTile != null)
        {
            moveTile.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public void ReturnMoveTile()
    {
        if(moveTile != null)
        {
            moveTile.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void ShowAttackTile()
    {
        foreach(Tile tile in attackTiles)
        {
            tile.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    public void ReturnAttackTile()
    {
        foreach (Tile tile in attackTiles)
        {
            tile.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
