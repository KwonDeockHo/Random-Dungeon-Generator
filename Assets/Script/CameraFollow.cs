using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    //[Header("Player Camera")]
    //public GameObject player;        //Public variable to store a reference to the player game object
    //private Vector3 offset;            //Private variable to store the offset distance between the player and camera

    [Header("Room Camera")]
    public Room currRoom;
    public float moveSpeedWhenRoomChange;
    public int cameraHeight = 10;
    public float backSide = 1.0f;
    public float cameraAngle = 1.0f;

    public float offsetX;
    public float offsetY;
    public float offsetZ;
    public float DelayTime;

    public void Update()
    {
        UpdatePosition();
    }
    public void UpdatePosition()
    {
        if(currRoom == null)
        {
            return;
        }

        Vector3 playerPosition = Player.self.transform.position;

        //Vector3 targetPos = new Vector3(playerPosition.x, cameraHeight, playerPosition.z - backSide);

        //transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeedWhenRoomChange);
        //transform.rotation = Quaternion.Euler(cameraAngle, transform.rotation.y, transform.rotation.z);

        Vector3 FixedPos = new Vector3(playerPosition.x + offsetX, playerPosition.y + offsetY, playerPosition.z + offsetZ);
        transform.rotation = Quaternion.Euler(cameraAngle, transform.rotation.y, transform.rotation.z);
        transform.position = Vector3.Lerp(transform.position, FixedPos, Time.deltaTime * DelayTime);
        
    }

    public Vector3 GetCameraTargetPosition()
    {
        if(currRoom == null)
        {
            return Vector3.zero;
        }

        Vector3 targetPos = currRoom.GetRoomCenter();
        //targetPos.y = transform.position.y;
        
        return targetPos;
    }
    //public bool IsSwitchingScene()
    //{
    //    return transform.position.Equals(GetCameraTargetPosition()) == false;
    //}

}
