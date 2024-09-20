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
    [SerializeField] private Bat bat_prefab;
    [SerializeField] private Player player_prefab;

    public static GameManager instance;

    [HideInInspector] public bool[] isPlayerInput;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this);

        //일단 싱글게임
        isPlayerInput = new bool[1];

        for(int i=0;i<isPlayerInput.Length;i++)
        {
            isPlayerInput[i] = false;
        }
    }


    public void Start()
    {
        //DEBUG 맵 생성
        DungeonManager.instance.GenerateRoom();

        //플레이어 생성
        var playerGo = Instantiate(player_prefab);
        playerGo.GetComponent<PlayerController>().InitPlayerPosition();
        GetComponent<CameraManager>().player = playerGo;

        //테스트용 더미 생성
        GenerateDebugDummy();
    }

    public void GenerateDebugDummy()
    {
        GenerateTrainingBot();
        GenerateDagger();
        GenerateSpear();
        GenerateBat();
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
    }

    public void GenerateDagger()
    {
        if (dagger_prefab == null)
            return;

        var dagger = Instantiate(dagger_prefab);
        dagger.RoomX = (Room.X / 2) - 1;
        dagger.RoomY = (Room.Y / 2);
        dagger.curRoom = DungeonManager.instance.rooms[DungeonManager.DUNGEON_X / 2, DungeonManager.DUNGEON_Y / 2];
        dagger.transform.position = dagger.GetTile().transform.position;
        dagger.GetTile().onTileUnit = dagger;
    }

    public void GenerateSpear()
    {
        if (spear_prefab == null)
            return;

        var spear = Instantiate(spear_prefab);
        spear.RoomX = (Room.X / 2);
        spear.RoomY = (Room.Y / 2) - 1;
        spear.curRoom = DungeonManager.instance.rooms[DungeonManager.DUNGEON_X / 2, DungeonManager.DUNGEON_Y / 2];
        spear.transform.position= spear.GetTile().transform.position;
        spear.GetTile().onTileUnit = spear;
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
    }

    public void ResultQTE(bool success, Player causer, List<IDamagable> damagableList)
    {
        if(success)
        {
            causer.Attack(damagableList);
        }

        isPlayerInput[0] = false;

        ExecuteTurn();
    }
}
