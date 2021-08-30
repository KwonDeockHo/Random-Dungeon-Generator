using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Room : MonoBehaviour
{
    public int Width;
    public int Height;

    public Vector3Int currPos;
    public Vector3Int parentPos;
    public Vector3 CenterPos;

    public string roomName;
    public string roomType;
    public string roomId;

    public int weight;

    public bool updatedWalls = false;
    public bool visitedRoom = false;
    public GameObject Door;
    public GameObject Wall;


    public Room(int x, int y, int z)
    {
        currPos.x = x;
        currPos.y = y;
        currPos.z = z;
    }

    public SubRoom rooms;

    public bool updateRooms = false;


    // Room을 생성 시 초기에 호출(Start)
    public void setUpdateWalls(bool setup)
    {
        updatedWalls = setup;
    }

    void Start()
    {
        if (RoomController.Instance == null)
        {
            Debug.Log("You pressed play in the wrong scene!");
            return;
        }

        rooms = GetComponentInChildren<SubRoom>();

        if (rooms != null)
        {
            rooms.currPos   = currPos;
            rooms.roomType  = roomType;
            rooms.Width     = Width;
            rooms.Height    = Height;
            rooms.roomName  = roomName;
            rooms.parentPos = parentPos;
            rooms.CenterPos = CenterPos;

        }

        updatedWalls = false;
    }

    public void RemoveUnconnectedWalls()
    {
        if (rooms != null)
            rooms.RemoveUnconnectedWalls();
    }

    void Update()
    {
        if (!updatedWalls)
        {
            RemoveUnconnectedWalls();

            updatedWalls = true;
        }
    }


    public Vector3 GetRoomCenter()
    {
        return new Vector3(currPos.x, 0, currPos.z);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            RoomController.Instance.OnPlayerEnterRoom(this);
        }
    }

    
}
