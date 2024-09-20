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

    public void Die(Enemy enemy)
    {
        monstersList.Remove(enemy);
        enemy.GetTile().onTileUnit = null;
        enemy.ReturnAttackTile();
        enemy.ReturnMoveTile();
        Destroy(enemy.gameObject);
    }
}
