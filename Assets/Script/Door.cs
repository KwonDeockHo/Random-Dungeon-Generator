using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        left, right, top, bottom
    }
    public GameObject nextRoom;
    public Door SideDoor;
    public DoorType doorType;
    public Transform doorPos;
    public bool isUpdate = false;



    public void setNextRoom(GameObject _nextRoom)
    {
        nextRoom = _nextRoom;
    }
}
