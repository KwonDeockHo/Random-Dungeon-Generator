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
    public Vector3Int currPos;
    public Vector3Int parentPos;
    public Vector3 CenterPos;
    public string wallType;

    public Room parentRoom;
    public bool updatedRooms = false;
    public bool roomPathBool = false;
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
        if (!updatedRooms)
        {
            RemoveUnconnectedWalls();

            updatedRooms = true;
        }

        //if (updatedRooms && !roomPathBool)
        //    setRoomPath(wallType);

    }
    public void updateRoomSetup()
    {
        if (!roomType.Equals("Single"))
        {
            parentRoom = RoomController.Instance.FindRoom(parentPos.x, parentPos.y, parentPos.z );

            GameObject subRoom = this.gameObject;
            subRoom.transform.SetParent(parentRoom.transform);
            subRoom.transform.parent.GetComponent<Room>().setUpdateWalls(false);


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
            if (parentPos == RoomController.Instance.loadedRooms[i].parentPos)
            {
                RoomController.Instance.loadedRooms[i].visitedRoom = true;
                RoomController.Instance.loadedRooms[i].rooms.minimapRoom.VisitiedRoom(true, true);
                RoomController.Instance.loadedRooms[i].rooms.minimapRoom.VisitiedCurrRoom(true);
            }
            else
            {
                RoomController.Instance.loadedRooms[i].rooms.minimapRoom.VisitiedCurrRoom(false);
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
            if (room.parentPos == RoomController.Instance.loadedRooms[i].parentPos)
                   RoomController.Instance.loadedRooms[i].rooms.minimapRoom.VisitiedRoom(true, false);
        }
    }
    public void RemoveUnconnectedWalls()
    {
        Vector3 tmpCenterPos = transform.parent.gameObject.GetComponent<Room>().parentPos;
        string wallStr = "";

        foreach (Wall wall in walls)
        {
            switch (wall.wallType) {
                case Wall.WallType.left:
                    if (GetLeft() != null)
                    {
                        Room leftRoom = GetLeft();

                        if (leftRoom.parentPos == tmpCenterPos)
                        {
                            leftDoor.gameObject.SetActive(false);
                            leftWall.gameObject.SetActive(false);
                            
                            minimapRoom.leftWall.gameObject.SetActive(false);
                            minimapRoom.leftWall.isSetUp = false;

                        }
                        else
                        {
                            wallStr += "L";
                            if (!leftDoor.isUpdate)
                            {
                                GameObject roomDoor = Instantiate(leftRoom.Door, leftDoor.transform);
                                roomDoor.gameObject.transform.SetParent(leftDoor.gameObject.transform);
                                leftDoor.setNextRoom(leftRoom.gameObject);
                                leftDoor.SideDoor = leftRoom.rooms.rightDoor;

                                leftDoor.isUpdate = true;
                            }
                        }
                    }
                    else
                    {
                        if (!leftWall.isUpdate)
                        {
                            GameObject newWall = transform.parent.GetComponent<Room>().Wall.gameObject;
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

                        if (topRoom.parentPos == tmpCenterPos)
                        {
                            topDoor.gameObject.SetActive(false);
                            topWall.gameObject.SetActive(false);
                            minimapRoom.topWall.gameObject.SetActive(false);
                            minimapRoom.topWall.isSetUp = false;

                        }
                        else
                        {
                            wallStr += "T";
                            if (!topDoor.isUpdate)
                            {
                                GameObject roomDoor = Instantiate(topRoom.Door, topDoor.transform);
                                roomDoor.gameObject.transform.SetParent(topDoor.gameObject.transform);
                                topDoor.setNextRoom(topRoom.gameObject);
                                topDoor.SideDoor = topRoom.rooms.bottomDoor;

                                topDoor.isUpdate = true;
                            }
                        }
                    }
                    else
                    {
                        if (!topWall.isUpdate)
                        {
                            GameObject newWall = transform.parent.GetComponent<Room>().Wall.gameObject;
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
                        if (rightRoom.parentPos == tmpCenterPos)
                        {
                            rightDoor.gameObject.SetActive(false);
                            rightWall.gameObject.SetActive(false);

                            minimapRoom.rightWall.gameObject.SetActive(false);
                            minimapRoom.rightWall.isSetUp = false;

                        }
                        else
                        {
                            wallStr += "R";
                            if (!rightDoor.isUpdate)
                            {
                                GameObject roomDoor = Instantiate(rightRoom.Door, rightDoor.transform);
                                roomDoor.gameObject.transform.SetParent(rightDoor.gameObject.transform);

                                rightDoor.setNextRoom(rightRoom.gameObject);
                                rightDoor.SideDoor = rightRoom.rooms.leftDoor;

                                rightDoor.isUpdate = true;
                            }
                        }
                    }
                    else
                    {
                        if (!rightWall.isUpdate)
                        {
                            GameObject newWall = transform.parent.GetComponent<Room>().Wall.gameObject;
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

                        if (bottomRoom.parentPos == tmpCenterPos)
                        {
                            // 방이 뚫려 있다.
                            bottomDoor.gameObject.SetActive(false);
                            bottomWall.gameObject.SetActive(false);

                            minimapRoom.bottomWall.gameObject.SetActive(false);
                            minimapRoom.bottomWall.isSetUp = false;

                        }
                        else
                        {
                            wallStr += "B";
                            if (!bottomDoor.isUpdate)
                            {
                                GameObject roomDoor = Instantiate(bottomRoom.Door, bottomDoor.transform);
                                roomDoor.gameObject.transform.SetParent(bottomDoor.gameObject.transform);

                                bottomDoor.setNextRoom(bottomRoom.gameObject);
                                bottomDoor.SideDoor = bottomRoom.rooms.topDoor;

                                bottomDoor.isUpdate = true;
                            }
                        }
                    }
                    else {

                        if (!bottomWall.isUpdate)
                        {
                            GameObject newWall = transform.parent.GetComponent<Room>().Wall.gameObject;
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
        if (RoomController.Instance.DoesRoomExist(currPos.x + 1, currPos.y, currPos.z))
        {
            return RoomController.Instance.FindRoom(currPos.x + 1, currPos.y, currPos.z);
        }
        return null;
    }
    public Room GetLeft()
    {
        if (RoomController.Instance.DoesRoomExist(currPos.x - 1, currPos.y, currPos.z))
        {
            return RoomController.Instance.FindRoom(currPos.x - 1, currPos.y, currPos.z);
        }
        return null;
    }
    public Room GetTop()
    {
        if (RoomController.Instance.DoesRoomExist(currPos.x, currPos.y, currPos.z + 1))
        {
            return RoomController.Instance.FindRoom(currPos.x, currPos.y, currPos.z + 1);
        }
        return null;
    }
    public Room GetBottom()
    {
        if (RoomController.Instance.DoesRoomExist(currPos.x, currPos.y, currPos.z - 1))
        {
            return RoomController.Instance.FindRoom(currPos.x, currPos.y, currPos.z - 1);
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
