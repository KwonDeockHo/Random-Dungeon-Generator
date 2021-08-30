using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class RoomInfo
{
    public string roomID;
    public string roomName;
    public string roomType;

    // 현재 방의 위치
    public Vector3Int currPos;
    // 부모 방의 위치
    public Vector3Int parentPos;
    // 해당 방의 중앙 위치
    public Vector3 CenterPos;
    // 해당 방의 상태 설정(true : 방 셋팅, false : 빈방)
    public bool roomState;
    // 시작 방에서 부터 해당 방까지의 거리
    public int weightCnt;

}
