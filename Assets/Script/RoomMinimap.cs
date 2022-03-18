using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMinimap : MonoBehaviour
{
    //public List<GameObject> minimapRoomWall;
    public GameObject floorMap;
    public bool visitedRoom = false;

    public List<Wall> walls;
    public Wall leftWall;
    public Wall rightWall;
    public Wall topWall;
    public Wall bottomWall;

    public GameObject currRoom;
    public bool visited = false;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.SetActive(false);
        floorMap.GetComponentInChildren<MeshRenderer>().material = RoomController.Instance.DefaultBackground;


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

        minimapWallset(false);
    }

    // Update is called once per frame
    
    public void VisitiedRoom(bool boolean, bool currBool)
    {
        transform.gameObject.SetActive(boolean);
        if (currBool)
        {
            visited = true;
        }

        minimapWallset(true);
    }

    public void VisitiedCurrRoom(bool boolean)
    {

        // 4. 현재 위치 밝게 처리
        if (boolean)
            floorMap.GetComponentInChildren<MeshRenderer>().material = RoomController.Instance.currMaterial;
        else
        {
            if(visited)
                floorMap.GetComponentInChildren<MeshRenderer>().material = RoomController.Instance.VisitedBack;
            else
                floorMap.GetComponentInChildren<MeshRenderer>().material = RoomController.Instance.DefaultBackground;
        }
    }

    public void minimapWallset(bool boolean)
    {
        if (visited || boolean)
            for (int i = 0; i < walls.Count; i++)
            {
                if (walls[i].isSetUp)
                    walls[i].transform.gameObject.SetActive(boolean);
            }
        else
            for (int i = 0; i < walls.Count; i++)
            {
                if(walls[i].isSetUp)
                    walls[i].transform.gameObject.SetActive(boolean);
            }
    }
}
