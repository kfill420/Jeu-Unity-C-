using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{ 
    [SerializeField]
    private GameObject[] rooms;
    Vector3 spawnPoint;

    void Start()
    {
        RoomLoader();
    }

    void RoomLoader()
    {
        //Start by spawning the spawn 
        //Spawn at least 10 rooms
        //Spawn 1, 2, 3, or 4 rooms/hallway depending of the number of doors in the room
        //No more than 2 stairs
        //If room contain stairs then load a room upstais
        //After 10 rooms spawn boss room and rooms with no doors for all the remaining doors
        //End with the boss room



        spawnPoint = new Vector3(0,0,0);

        Instantiate(rooms[0], spawnPoint, new Quaternion());


        

    }
}
