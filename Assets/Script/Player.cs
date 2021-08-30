using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public static Player self;
    public float speed = 5.0f;

    public float horizontal;
    public float vertical;

    public bool moveBool = true;
    void Start()
    {
        if (self == null)
            self = this;
        
    }

    public void PlayerMove()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        transform.position += new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
    }

    void Update()
    {
        if (moveBool)
            PlayerMove();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Door")
        {
            FadeInOut.Instance.Fade(true, 1.35f);

            GameObject nextRoom = collision.gameObject.transform.parent.GetComponent<Door>().nextRoom;
            Door nextDoor = collision.gameObject.transform.parent.GetComponent<Door>().SideDoor;

            // 진행 방향을 알면 문제해결
            Vector3 currPos = new Vector3(nextDoor.transform.position.x, 0.5f, nextDoor.transform.position.z) + (nextDoor.transform.localRotation * (Vector3.forward * 3));

            Player.Instance.transform.position = currPos;

            for (int i = 0; i < RoomController.Instance.loadedRooms.Count; i++)
            {
                if (nextRoom.GetComponent<Room>().parentPos == RoomController.Instance.loadedRooms[i].parentPos)
                {
                    RoomController.Instance.loadedRooms[i].rooms.gameObject.SetActive(true);
                }
                else
                {
                    RoomController.Instance.loadedRooms[i].rooms.gameObject.SetActive(false);
                }
            }

            FadeInOut.Instance.Fade(false, 0.15f);
        }
    }
}
