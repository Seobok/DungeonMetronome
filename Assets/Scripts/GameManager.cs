using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public enum E_Dir
{
    ED_Up = 0,
    ED_Right = 1,
    ED_Down = 2,
    ED_Left = 3,
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private TrainingBot trainingBot_prefab;
    [SerializeField] private Dagger dagger_prefab;
    [SerializeField] private Spear spear_prefab;
    [SerializeField] private DualDagger dualDagger_prefab;
    [SerializeField] private Bat bat_prefab;
    [SerializeField] private Slime slime_prefab;
    [SerializeField] private Player player_prefab;

    public static GameManager instance;

    [HideInInspector] public List<bool> isPlayerInput;
    [HideInInspector] public List<Player> players;

    [HideInInspector] public int stage = 0;

    private int turnCnt = 0;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this);

        //일단 싱글게임
        isPlayerInput = new() { false };

        players = new List<Player>();
    }


    public void Start()
    {
        StartLobby();
    }

    public void StartLobby()
    {
        stage = 0;

        DungeonManager.instance.GenerateLobby();

        if(players.Count == 0 )
        {
            var playerGo = Instantiate(player_prefab);
            players.Add(playerGo);
            GetComponent<CameraManager>().player = playerGo;
        }

        foreach( Player player in players )
        {
            player.RoomX = Room.X / 2;
            player.RoomY = Room.Y / 2;
            player.curRoom = DungeonManager.instance.lobby;
            player.transform.position = player.GetTile().transform.position;
            player.GetTile().onTilePlayer = player;
        }
    }

    public void Play()
    {
        stage = 1;

        DungeonManager.instance.GenerateRoom();

        //플레이어 생성
        if(players.Count == 0)
        {
            var playerGo = Instantiate(player_prefab);
            players.Add(playerGo);
            GetComponent<CameraManager>().player = playerGo;
        }

        foreach (var player in players)
        {
            player.GetComponent<PlayerController>().InitPlayerPosition();
        }

        if (DungeonManager.instance.lobby != null)
        {
            DungeonManager.instance.DeactiveLobby();
        }

        //테스트용 더미 생성
        GenerateDebugDummy();

    }

    public void GenerateDebugDummy()
    {
        GenerateTrainingBot();
        GenerateDagger();
        GenerateSpear();
        GenerateDualDagger();
        //GenerateBat();
        //GenerateSlime();
        MonsterSpawner.instance.SpawnMonster();
    }

    public void GenerateTrainingBot()
    {
        if (trainingBot_prefab == null)
            return;

        var trainingBot = Instantiate(trainingBot_prefab);

        trainingBot.RoomX = (Room.X / 2) + 1;
        trainingBot.RoomY = Room.Y / 2;
        trainingBot.curRoom = DungeonManager.instance.rooms[DungeonManager.DUNGEON_X / 2, DungeonManager.DUNGEON_Y / 2];
        trainingBot.transform.position = trainingBot.GetTile().transform.position;
        trainingBot.GetTile().onTileUnit = trainingBot;

        trainingBot.transform.SetParent(trainingBot.GetTile().transform);
    }

    public void GenerateDagger()
    {
        if (dagger_prefab == null)
            return;

        var dagger = Instantiate(dagger_prefab);
        dagger.RoomX = 4;
        dagger.RoomY = 4;
        dagger.curRoom = DungeonManager.instance.rooms[DungeonManager.DUNGEON_X / 2, DungeonManager.DUNGEON_Y / 2];
        dagger.transform.position = dagger.GetTile().transform.position;
        dagger.GetTile().onTileUnit = dagger;

        dagger.transform.SetParent(dagger.GetTile().transform);
    }

    public void GenerateSpear()
    {
        if (spear_prefab == null)
            return;

        var spear = Instantiate(spear_prefab);
        spear.RoomX = 6;
        spear.RoomY = 4;
        spear.curRoom = DungeonManager.instance.rooms[DungeonManager.DUNGEON_X / 2, DungeonManager.DUNGEON_Y / 2];
        spear.transform.position= spear.GetTile().transform.position;
        spear.GetTile().onTileUnit = spear;

        spear.transform.SetParent (spear.GetTile().transform);
    }

    public void GenerateDualDagger()
    {
        if (dualDagger_prefab == null)
            return;

        var dualDagger = Instantiate(dualDagger_prefab);
        dualDagger.RoomX = 8;
        dualDagger.RoomY = 4;
        dualDagger.curRoom = DungeonManager.instance.rooms[DungeonManager.DUNGEON_X / 2, DungeonManager.DUNGEON_Y / 2];
        dualDagger.transform.position = dualDagger.GetTile().transform.position;
        dualDagger.GetTile().onTileUnit = dualDagger;

        dualDagger.transform.SetParent(dualDagger.GetTile().transform);
    }

    public void GenerateBat()
    {
        if (bat_prefab == null) return;

        var bat = Instantiate(bat_prefab);
        bat.curRoom = DungeonManager.instance.roomList[1];
        bat.RoomX = (Room.X / 2);
        bat.RoomY = (Room.Y / 2);

        bat.transform.position = bat.GetTile().transform.position;
        bat.GetTile().onTileUnit = bat;

        MonsterSpawner.instance.monstersList.Add(bat);
    }

    public void GenerateSlime()
    {
        if (slime_prefab == null) return;

        var slime = Instantiate(slime_prefab);
        slime.curRoom = DungeonManager.instance.roomList[2];
        slime.RoomX = (Room.X / 2);
        slime.RoomY = (Room.Y / 2);

        slime.transform.position = slime.GetTile().transform.position;
        slime.GetTile().onTileUnit = slime;

        MonsterSpawner.instance.monstersList.Add(slime);
    }

    /// <summary>
    /// 메인 로직이 실행되는 함수
    /// 해당 함수가 끝나면 다시 플레이어 턴
    /// </summary>
    public void ExecuteTurn()
    {
        foreach(var monster in MonsterSpawner.instance.monstersList)
        {
            monster.Act();
        }
        turnCnt++;
    }

    public void ResultQTE(float damageRate, Player causer, List<IDamagable> damagableList)
    {
        causer.Attack(damagableList, damageRate);

        isPlayerInput[0] = false;

        ExecuteTurn();
    }
}
