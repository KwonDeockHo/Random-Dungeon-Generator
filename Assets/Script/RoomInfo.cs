using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class RoomInfo
{
    public string roomID;
    public string roomName;
    public string roomType;

    // 현재 방(개별)의 위치
    public Vector3Int center_Position;
    // 부모 방의 위치
    public Vector3Int parent_Position;
    // 해당 방(통합)의 중앙 위치
    public Vector3 mergeCenter_Position;
    // 해당 방의 상태 설정(true : 방 셋팅, false : 빈방)
    public bool isValidRoom;
    // 시작 방에서 부터 해당 방까지의 거리
    public int distance;

}
