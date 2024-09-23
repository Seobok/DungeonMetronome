using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private Enemy[] monsters;
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
        for(int i=1;i<DungeonManager.instance.roomList.Count;i++)
        {
            //스폰방을 제외한 방 마다 3마리씩
            for(int j = 0; j<3;j ++)
            {
                int rand = Random.Range(0, monsters.Length);
                var enemy = Instantiate(monsters[rand]);

                enemy.curRoom = DungeonManager.instance.roomList[i];

                //위치 조정
                var randX = Random.Range(0, Room.X + 1);
                var randY = Random.Range(0, Room.Y + 1);
                var spawnTile = enemy.curRoom.GetTile(randX, randY);
                while ((spawnTile == null || spawnTile.onTileUnit != null))
                {
                    randX = Random.Range(0, Room.X + 1);
                    randY = Random.Range(0, Room.Y + 1);
                    spawnTile = enemy.curRoom.GetTile(randX, randY);
                }

                enemy.RoomX = randX;
                enemy.RoomY = randY;

                enemy.transform.position = spawnTile.transform.position;
                spawnTile.onTileUnit = enemy;

                monstersList.Add(enemy);
            }
        }
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
