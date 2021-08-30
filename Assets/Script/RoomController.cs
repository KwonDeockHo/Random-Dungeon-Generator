using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomController : Singleton<RoomController>
{
    public string currentWorldName = "Basement";

    public RoomInfo currentLoadRoomData;
    public Room currRoom;

    public List<Room> loadedRooms = new List<Room>();
    public GameObject roomPrefabs;
    public bool isLoadingRoom = false;
    public bool spawnedBossRoom = false;
    public bool spawnedMergeRoom = false;
    public bool updatedRooms = false;
    public bool createRoom = false;
    public Material DefaultBackground;
    public Material VisitedBack;
    public Material currMaterial;

    public void newCreatedRoom()
    {
        isLoadingRoom = false;
        spawnedBossRoom = false;
        spawnedMergeRoom = false;
        updatedRooms = false;
        createRoom = false;

        for(int i=0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
        loadedRooms.Clear();

        Player.Instance.transform.position = new Vector3(0, 0.5f, 0) ;
        DungeonCrawlerController.Instance.CreatedRoom();
        UpdateRoomQueue();


    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom)
        {
            return;
        }

        if (loadedRooms.Count > 0)
        {
            foreach (Room room in loadedRooms)
            {
                room.RemoveUnconnectedWalls();
            }
            isLoadingRoom = true;
        }
    }

    public void LoadRoom(RoomInfo oldRoom)
    {
        if (DoesRoomExist(oldRoom.currPos.x, oldRoom.currPos.y, oldRoom.currPos.z))
        {
            return;
        }
        string roomPreName = oldRoom.roomName;

        GameObject room = Instantiate(RoomPrefabsSet.Instance.roomPrefabs[roomPreName]);
        
        room.transform.position = new Vector3(
                    (oldRoom.currPos.x * room.transform.GetComponent<Room>().Width),
                     oldRoom.currPos.y,
                    (oldRoom.currPos.z * room.transform.GetComponent<Room>().Height)
        );

        room.transform.localScale = new Vector3(
                    (room.transform.GetComponent<Room>().Width/10),
                     1,
                    (room.transform.GetComponent<Room>().Height/10)
        );
        room.transform.GetComponent<Room>().currPos     = oldRoom.currPos;
        room.name = currentWorldName + "-" + oldRoom.roomName + " " + oldRoom.currPos.x + ", " + oldRoom.currPos.z;

        room.transform.GetComponent<Room>().roomName    = oldRoom.roomName;
        room.transform.GetComponent<Room>().roomType    = oldRoom.roomType;
        room.transform.GetComponent<Room>().roomId      = oldRoom.roomID;
        room.transform.GetComponent<Room>().parentPos   = oldRoom.parentPos;
        room.transform.GetComponent<Room>().CenterPos   = oldRoom.CenterPos;
        room.transform.GetComponent<Room>().weight      = oldRoom.weightCnt;

        room.transform.parent = transform;

        loadedRooms.Add(room.GetComponent<Room>());
    }
        
    // 
    public bool DoesRoomExist(int x, int y, int z)
    {
        return loadedRooms.Find(item => item.currPos.x == x && item.currPos.y == y && item.currPos.z == z) != null;
    }

    //    
    public Room FindRoom(int x, int y, int z)
    {
        // List.Find : item 변수 조건에 맞는 Room을 찾아 반환
        return loadedRooms.Find(item => item.currPos.x == x && item.currPos.y == y && item.currPos.z == z);
    }

    // 해당 Room에서 Player가 있는 방을 반환
    public void OnPlayerEnterRoom(Room room) {
        CameraFollow.Instance.currRoom = room;
        
        currRoom = room;

        for (int i = 0; i < loadedRooms.Count; i++)
        {
            if (room.parentPos == loadedRooms[i].parentPos)
                loadedRooms[i].rooms.minimapUpdate();
        }
    }

}
