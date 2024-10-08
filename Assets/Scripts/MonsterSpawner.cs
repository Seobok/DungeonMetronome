using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private Enemy[] monsters;
    [SerializeField] private Enemy[] epicMonsters;
    public List<Enemy> monstersList;
    

    public static MonsterSpawner instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void SpawnMonster()
    {
        SaveManager.instance.monsters.Clear();

        //일반 몬스터 소환
        for (int i = 1; i < DungeonManager.instance.roomList.Count; i++)
        {
            //스폰방을 제외한 방 마다 3마리씩
            for (int j = 0; j < 3; j++)
            {
                int rand = Random.Range(0, monsters.Length);
                var enemy = Instantiate(monsters[rand]);

                SetRandomPos(enemy, DungeonManager.instance.roomList[i]);

                //Save
                switch(rand)
                {
                    case 0:
                        //Bat
                        SaveManager.instance.monsters.Add(new SaveManager.MonsterData(enemy.RoomX, enemy.RoomX, SaveManager.MonsterType.EMT_Bat, enemy.curRoom));
                        break;
                    case 1:
                        SaveManager.instance.monsters.Add(new SaveManager.MonsterData(enemy.RoomX, enemy.RoomX, SaveManager.MonsterType.EMT_Slime, enemy.curRoom));
                        //Slime
                        break;

                }
            }
        }

        //에픽몬스터 소환
        var epicMonster = Instantiate(epicMonsters[Random.Range(0, epicMonsters.Length)]);

        SetRandomPos(epicMonster, DungeonManager.instance.roomList[DungeonManager.instance.roomList.Count - 1]);

        SaveManager.instance.monsters.Add(new SaveManager.MonsterData(epicMonster.RoomX, epicMonster.RoomX, SaveManager.MonsterType.EMT_StoneGolem, epicMonster.curRoom));
    }

    public void ReSpawn()
    {
        foreach(var monster in SaveManager.instance.monsters)
        {
            Enemy enemy = null;
            switch(monster.monsterType)
            {
                case SaveManager.MonsterType.EMT_Bat:
                    enemy = Instantiate(monsters[0]);
                    return;
                case SaveManager.MonsterType.EMT_Slime:
                    enemy = Instantiate(monsters[1]);
                    return;
                case SaveManager.MonsterType.EMT_StoneGolem:
                    enemy = Instantiate(epicMonsters[0]);
                    return;
            }
            enemy.RoomX = monster.x;
            enemy.RoomY = monster.y;
            enemy.curRoom = DungeonManager.instance.rooms[monster.roomX, monster.roomY];

            enemy.transform.position = enemy.GetTile().transform.position;
            enemy.GetTile().onTileUnit = enemy;

            enemy.transform.SetParent(enemy.GetTile().transform);

            monstersList.Add(enemy);
        }
    }

    private void SetRandomPos(Enemy enemy, Room room)
    {
        enemy.curRoom = room;

        //위치 조정
        int randX, randY;
        Tile spawnTile;
        do
        {
            randX = Random.Range(0, Room.X + 1);
            randY = Random.Range(0, Room.Y + 1);
            spawnTile = enemy.curRoom.GetTile(randX, randY);
        } while ((spawnTile == null || spawnTile.onTileUnit != null)) ;

        enemy.RoomX = randX;
        enemy.RoomY = randY;

        enemy.transform.position = spawnTile.transform.position;
        spawnTile.onTileUnit = enemy;

        enemy.transform.SetParent(enemy.GetTile().transform);

        monstersList.Add(enemy);
    }

    public void Die(Enemy enemy)
    {
        monstersList.Remove(enemy);
        enemy.GetTile().onTileUnit = null;
        enemy.ReturnAttackTile();
        enemy.ReturnMoveTile();
        Destroy(enemy.gameObject);
    }
}
