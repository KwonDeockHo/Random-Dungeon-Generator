using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Room : MonoBehaviour
{
    public int Width;
    public int Height;

    public string roomName;
    public string roomType;
    public string roomId;

    public Vector3Int center_Position;
    public Vector3Int parent_Position;
    public Vector3 mergeCenter_Position;

    public int distance;

    public bool isUpdatedWalls = false;
    public bool isVisitedRoom = false;
    public GameObject prefabsDoor;
    public GameObject prefabsWall;


    public Room(int x, int y, int z)
    {
        center_Position.x = x;
        center_Position.y = y;
        center_Position.z = z;
    }

    public SubRoom childRooms;

    public bool updateRoomStatus = false;


    // Room을 생성 시 초기에 호출(Start)
    public void SetUpdateWalls(bool setup)
    {
        isUpdatedWalls = setup;
    }

    void Start()
    {
        if (RoomController.Instance == null)
        {
            Debug.Log("You pressed play in the wrong scene!");
            return;
        }

        childRooms = GetComponentInChildren<SubRoom>();

        if (childRooms != null)
        {
            childRooms.center_Position       = center_Position;
            childRooms.roomType              = roomType;
            childRooms.Width                 = Width;
            childRooms.Height                = Height;
            childRooms.roomName              = roomName;
            childRooms.parent_Position       = parent_Position;
            childRooms.mergeCenter_Position  = mergeCenter_Position;

        }

        isUpdatedWalls = false;
    }

    public void RemoveUnconnectedWalls()
    {
        if (childRooms != null)
            childRooms.RemoveUnconnectedWalls();
    }

    void Update()
    {
        if (!isUpdatedWalls)
        {
            RemoveUnconnectedWalls();

            isUpdatedWalls = true;
        }
    }


    public Vector3 GetRoomCenter()
    {
        return new Vector3(center_Position.x, 0, center_Position.z);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            RoomController.Instance.OnPlayerEnterRoom(this);
        }
    }

    
}
