using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        monsters = new List<MonsterData> ();
    }

    [Header("Room")]
    public bool[,] rooms;

    [Header("Goal")]
    public int goalRoomX;
    public int goalRoomY;
    public int goalX;
    public int goalY;

    public enum MonsterType
    {
        EMT_Bat = 0,
        EMT_Slime = 1,
        EMT_StoneGolem = 1000,
    }

    [Serializable]
    public class MonsterData
    {
        public MonsterData(int x, int y, MonsterType monsterType, Room room)
        {
            this.x = x;
            this.y = y;
            this.monsterType = monsterType;
            roomX = room.roomX;
            roomY = room.roomY;
        }
        public int x;
        public int y;
        public int roomX;
        public int roomY;
        public MonsterType monsterType;
    }
    [Header("Monster")]
    public List<MonsterData> monsters;

    [Header("Player")]
    public int totalScore;
    public Weapon equipedWeapon;
    public int maxHP;
    public int curHP;
    public int stage;

    [Header("Flag")]
    public bool isSaved = false;
}
