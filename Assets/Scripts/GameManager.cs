using System.Collections;
using System.Collections.Generic;
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

    public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this);
    }


    public void Start()
    {
        //DEBUG ¸Ê »ý¼º
        DungeonManager.instance.GenerateRoom();
        //GenerateTrainingBot();
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
}
