using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubRoom : MonoBehaviour
{
    public int Width;
    public int Height;

    public string roomName;
    public string roomType;

    // 각 방의 문을 세팅
    public List<Door> doors;
    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;
    
    public List<Wall> walls;
    public Wall leftWall;
    public Wall rightWall;
    public Wall topWall;
    public Wall bottomWall;

    // 현재 방 위치
    public Vector3Int center_Position;
    public Vector3Int parent_Position;
    public Vector3 mergeCenter_Position;
    public string wallType;

    public Room parentRoom;
    public bool isUpdatedRooms = false;
    public bool isRoomPathBool = false;
    public RoomMinimap minimapRoom;


    // Start is called before the first frame update
    void Start()
    {
        Door[] ds = GetComponentsInChildren<Door>();

        foreach (Door d in ds)
        {
            // Door 리스트에 Door를 삽입(
            doors.Add(d);

            switch (d.doorType)
            {
                case Door.DoorType.right:
                    rightDoor = d;
                    break;
                case Door.DoorType.left:
                    leftDoor = d;
                    break;
                case Door.DoorType.top:
                    topDoor = d;
                    break;
                case Door.DoorType.bottom:
                    bottomDoor = d;
                    break;
            }
        }

        Wall[] ws = GetComponentsInChildren<Wall>();

        foreach (Wall w in ws)
        {
            // Door 리스트에 Door를 삽입(
            walls.Add(w);

            switch (w.wallType)
            {
                case Wall.WallType.left:
                    leftWall = w;
                    break;
                case Wall.WallType.top:
                    topWall = w;
                    break;
                case Wall.WallType.right:
                    rightWall = w;
                    break;
                case Wall.WallType.bottom:
                    bottomWall = w;
                    break;
            }
        }


        updateRoomSetup();
    }

    private void Update()
    {
        RoomUpdate();
    }
    void RoomUpdate()
    {
        if (!isUpdatedRooms)
        {
            RemoveUnconnectedWalls();

            isUpdatedRooms = true;
        }

        //if (updatedRooms && !roomPathBool)
        //    setRoomPath(wallType);

    }
    public void updateRoomSetup()
    {
        if (!roomType.Equals("Single"))
        {
            parentRoom = RoomController.Instance.FindRoom(parent_Position.x, parent_Position.y, parent_Position.z );

            GameObject tmpChildRoom = this.gameObject;
            tmpChildRoom.transform.SetParent(parentRoom.transform);
            tmpChildRoom.transform.parent.GetComponent<Room>().SetUpdateWalls(false);


            GameObject miniRoom = minimapRoom.gameObject;
            miniRoom.transform.SetParent(parentRoom.transform);
        }
    }
    public void minimapUpdate()
    {
        // 0. 현재 맵을 setActive = true
        //visitedRoom = true;

        for (int i = 0; i < RoomController.Instance.loadedRooms.Count; i++)
        {
            if (parent_Position == RoomController.Instance.loadedRooms[i].parent_Position)
            {
                RoomController.Instance.loadedRooms[i].isVisitedRoom = true;
                RoomController.Instance.loadedRooms[i].childRooms.minimapRoom.VisitiedRoom(true, true);
                RoomController.Instance.loadedRooms[i].childRooms.minimapRoom.VisitiedCurrRoom(true);
            }
            else
            {
                RoomController.Instance.loadedRooms[i].childRooms.minimapRoom.VisitiedCurrRoom(false);
            }
        }

        // 2. 해당 인접한 Room에 대해서 visible 
        if (GetRight() != null)
            minimapUpdateSide(GetRight());

        if (GetLeft() != null)
            minimapUpdateSide(GetLeft());


        if (GetTop() != null)
            minimapUpdateSide(GetTop());

        if (GetBottom() != null)
            minimapUpdateSide(GetBottom());
    }
    public void minimapUpdateSide(Room room)
    {
        for (int i = 0; i < RoomController.Instance.loadedRooms.Count; i++)
        {
            if (room.parent_Position == RoomController.Instance.loadedRooms[i].parent_Position)
                   RoomController.Instance.loadedRooms[i].childRooms.minimapRoom.VisitiedRoom(true, false);
        }
    }
    public void RemoveUnconnectedWalls()
    {
        Vector3 tmpCenterPos = transform.parent.gameObject.GetComponent<Room>().parent_Position;
        string wallStr = "";

        foreach (Wall wall in walls)
        {
            switch (wall.wallType) {
                case Wall.WallType.left:
                    if (GetLeft() != null)
                    {
                        Room leftRoom = GetLeft();

                        if (leftRoom.parent_Position == tmpCenterPos)
                        {
                            leftDoor.gameObject.SetActive(false);
                            leftWall.gameObject.SetActive(false);
                            
                            minimapRoom.leftWall.gameObject.SetActive(false);
                            minimapRoom.leftWall.isSetUp = false;

                        }
                        else
                        {
                            wallStr += "Left";
                            if (!leftDoor.isUpdate)
                            {
                                GameObject roomDoor = Instantiate(leftRoom.prefabsDoor, leftDoor.transform);
                                roomDoor.gameObject.transform.SetParent(leftDoor.gameObject.transform);
                                leftDoor.setNextRoom(leftRoom.gameObject);
                                leftDoor.SideDoor = leftRoom.childRooms.rightDoor;

                                leftDoor.isUpdate = true;
                            }
                        }
                    }
                    else
                    {
                        if (!leftWall.isUpdate)
                        {
                            GameObject newWall = transform.parent.GetComponent<Room>().prefabsWall.gameObject;
                            GameObject roomWall = Instantiate(newWall, leftWall.transform);
                            leftWall.isUpdate = true;
                        }

                        leftDoor.gameObject.SetActive(false);
                    }
                    break;

                case Wall.WallType.top:

                    if (GetTop() != null)
                    {
                        Room topRoom = GetTop();

                        if (topRoom.parent_Position == tmpCenterPos)
                        {
                            topDoor.gameObject.SetActive(false);
                            topWall.gameObject.SetActive(false);
                            minimapRoom.topWall.gameObject.SetActive(false);
                            minimapRoom.topWall.isSetUp = false;

                        }
                        else
                        {
                            wallStr += "Top";
                            if (!topDoor.isUpdate)
                            {
                                GameObject roomDoor = Instantiate(topRoom.prefabsDoor, topDoor.transform);
                                roomDoor.gameObject.transform.SetParent(topDoor.gameObject.transform);
                                topDoor.setNextRoom(topRoom.gameObject);
                                topDoor.SideDoor = topRoom.childRooms.bottomDoor;

                                topDoor.isUpdate = true;
                            }
                        }
                    }
                    else
                    {
                        if (!topWall.isUpdate)
                        {
                            GameObject newWall = transform.parent.GetComponent<Room>().prefabsWall.gameObject;
                            GameObject roomWall = Instantiate(newWall, topWall.transform);
                            topWall.isUpdate = true;
                        }

                        topDoor.gameObject.SetActive(false);
                    }
                    break;

                case Wall.WallType.right:
                    if (GetRight() != null)
                    {
                        Room rightRoom = GetRight();
                        if (rightRoom.parent_Position == tmpCenterPos)
                        {
                            rightDoor.gameObject.SetActive(false);
                            rightWall.gameObject.SetActive(false);

                            minimapRoom.rightWall.gameObject.SetActive(false);
                            minimapRoom.rightWall.isSetUp = false;

                        }
                        else
                        {
                            wallStr += "Rright";
                            if (!rightDoor.isUpdate)
                            {
                                GameObject roomDoor = Instantiate(rightRoom.prefabsDoor, rightDoor.transform);
                                roomDoor.gameObject.transform.SetParent(rightDoor.gameObject.transform);

                                rightDoor.setNextRoom(rightRoom.gameObject);
                                rightDoor.SideDoor = rightRoom.childRooms.leftDoor;

                                rightDoor.isUpdate = true;
                            }
                        }
                    }
                    else
                    {
                        if (!rightWall.isUpdate)
                        {
                            GameObject newWall = transform.parent.GetComponent<Room>().prefabsWall.gameObject;
                            GameObject roomWall = Instantiate(newWall, rightWall.transform);
                            rightWall.isUpdate = true;
                        }

                        rightDoor.gameObject.SetActive(false);
                    }
                    break;

                case Wall.WallType.bottom:
                    if (GetBottom() != null)
                    {
                        Room bottomRoom = GetBottom();

                        if (bottomRoom.parent_Position == tmpCenterPos)
                        {
                            // 방이 뚫려 있다.
                            bottomDoor.gameObject.SetActive(false);
                            bottomWall.gameObject.SetActive(false);

                            minimapRoom.bottomWall.gameObject.SetActive(false);
                            minimapRoom.bottomWall.isSetUp = false;

                        }
                        else
                        {
                            wallStr += "Bottom";
                            if (!bottomDoor.isUpdate)
                            {
                                GameObject roomDoor = Instantiate(bottomRoom.prefabsDoor, bottomDoor.transform);
                                roomDoor.gameObject.transform.SetParent(bottomDoor.gameObject.transform);

                                bottomDoor.setNextRoom(bottomRoom.gameObject);
                                bottomDoor.SideDoor = bottomRoom.childRooms.topDoor;

                                bottomDoor.isUpdate = true;
                            }
                        }
                    }
                    else {

                        if (!bottomWall.isUpdate)
                        {
                            GameObject newWall = transform.parent.GetComponent<Room>().prefabsWall.gameObject;
                            GameObject roomWall = Instantiate(newWall, bottomWall.transform);
                            bottomWall.isUpdate = true;
                        }
                        bottomDoor.gameObject.SetActive(false);
                    }
                    break;

            }
        }

        if (wallStr != "")
            wallType = wallStr;
        else
            wallType = "None";
    }

    public Room GetRight()
    {
        if (RoomController.Instance.DoesRoomExist(center_Position.x + 1, center_Position.y, center_Position.z))
        {
            return RoomController.Instance.FindRoom(center_Position.x + 1, center_Position.y, center_Position.z);
        }
        return null;
    }
    public Room GetLeft()
    {
        if (RoomController.Instance.DoesRoomExist(center_Position.x - 1, center_Position.y, center_Position.z))
        {
            return RoomController.Instance.FindRoom(center_Position.x - 1, center_Position.y, center_Position.z);
        }
        return null;
    }
    public Room GetTop()
    {
        if (RoomController.Instance.DoesRoomExist(center_Position.x, center_Position.y, center_Position.z + 1))
        {
            return RoomController.Instance.FindRoom(center_Position.x, center_Position.y, center_Position.z + 1);
        }
        return null;
    }
    public Room GetBottom()
    {
        if (RoomController.Instance.DoesRoomExist(center_Position.x, center_Position.y, center_Position.z - 1))
        {
            return RoomController.Instance.FindRoom(center_Position.x, center_Position.y, center_Position.z - 1);
        }
        return null;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            RoomController.Instance.OnPlayerEnterRoom(this.transform.parent.GetComponent<Room>());
        }
    }
}
