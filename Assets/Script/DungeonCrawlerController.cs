using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCrawlerController : Singleton<DungeonCrawlerController>
{
    public List<Vector3Int> direction4                      = new List<Vector3Int>
    {
        new Vector3Int( 0, 0,  1),       // down
        new Vector3Int( 1, 0,  0),       // right
        new Vector3Int(-1, 0,  0),       // left
        new Vector3Int( 0, 0, -1)        // up
    };
    public List<Vector3Int> direction8                      = new List<Vector3Int>
    {   // Down                           // Up
        new Vector3Int( 0, 0,  1),        new Vector3Int( 0, 0, -1),        
        // Left                           // Right
        new Vector3Int(-1, 0,  0),        new Vector3Int( 1, 0,  0),        
        // UpLeft                         // UpRight
        new Vector3Int(-1, 0, -1),        new Vector3Int( 1, 0, -1),
        // DiwbLeft                       // DownRight
        new Vector3Int(-1, 0,  1),        new Vector3Int( 1, 0,  1)
    };
    public Dictionary<int, List<Vector3Int>> downPatten     = new Dictionary<int, List<Vector3Int>>
    {
        {  0, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(-1, 0, 1),   new Vector3Int(0, 0, 2),   new Vector3Int(-1, 0, 2) } }, // ㅁ
        {  1, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(0, 0, 2),    new Vector3Int(-1, 0, 2)    } }, // ┓
        {  2, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(0, 0, 2),    new Vector3Int(1, 0, 2)     } }, // ┏
        {  3, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(0, 0, 2)                                 } }, // 아래 |
        {  4, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(-1, 0, 1)                                } }, // 아래 |
        {  5, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(1, 0, 1)                                 } }, // 아래 |
        {  6, new List<Vector3Int>      { new Vector3Int(0, 0, 1)                                                            } }, // 아래 |
    };
    public Dictionary<int, List<Vector3Int>> upPatten       = new Dictionary<int, List<Vector3Int>>
    {
        {  0, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(0, 0, -2),  new Vector3Int(-1, 0, -1),  new Vector3Int(-1, 0, -2)} }, // ㅁ
        {  1, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(0, 0, -2),   new Vector3Int(1, 0, -2)    } }, // ┏
        {  2, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(0, 0, -2),   new Vector3Int(-1, 0, -2)   } }, // ┐
        {  3, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(0, 0, -2)                                } }, // 위 |
        {  4, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(1, 0, -1)                                } }, // 위 |
        {  5, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(-1, 0, -1)                               } }, // 위 |
        {  6, new List<Vector3Int>      { new Vector3Int(0, 0, -1)                                                            } }, // 위 |
    };
    public Dictionary<int, List<Vector3Int>> leftPatten     = new Dictionary<int, List<Vector3Int>>
    {
        {  0, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-2, 0, 0),   new Vector3Int(-1, 0, -1),  new Vector3Int(-2, 0, -1) } }, // ㅁ
        {  1, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-2, 0, 0),   new Vector3Int(-2, 0, -1)   } }, // └ 
        {  2, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-2, 0, 0),   new Vector3Int(-2, 0, 1)    } }, // ┌
        {  3, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-2, 0, 0)                                } }, // 왼쪽  --
        {  4, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-1, 0, -1)                               } }, // 왼쪽  --
        {  5, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-1, 0, 1)                                } }, // 왼쪽  --
        {  6, new List<Vector3Int>      { new Vector3Int(-1, 0, 0)                                                           } }, // 왼쪽  --
    };
    public Dictionary<int, List<Vector3Int>> rightPatten    = new Dictionary<int, List<Vector3Int>>
    {
        {  0, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(2, 0, 0),    new Vector3Int(1, 0, 1) ,   new Vector3Int(2, 0, 1) } }, // ㅁ
        {  1, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(2, 0, 0),    new Vector3Int(2, 0, 1)     } }, // ┐
        {  2, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(2, 0, 0),    new Vector3Int(2, 0, -1)    } }, // ┛ 
        {  3, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(2, 0, 0)                                 } }, // 오른쪽  --
        {  4, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(1, 0, 1)                                 } }, // 오른쪽  --
        {  5, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(1, 0, -1)                                } }, // 오른쪽  --
        {  6, new List<Vector3Int>      { new Vector3Int(1, 0, 0)                                                            } },
    };

    public List<RoomInfo> validRoomList = new List<RoomInfo>();

    public int minRoomCnt       = 15;                       // 최소 방 갯수
    public int maxRoomCnt       = 20;                       // 최대 방 갯수
    public int currRoomCnt      = 0;                        // 현재 방 갯수
    public int maxDistance      = 5;                        // 최대 거리 제한

    public int validRoomCount   = 0;       

    public Vector3Int startRoomPosition;                        // 시작 포지션
    public Vector3Int bossRoomPosition;                         // 보스 방 포지션

    public RoomInfo[,] posArr = new RoomInfo[10, 10];       // 방 좌표에 대한 2차원 배열

    public void CreatedRoom()
    {
        // 배열 ReSize
        posArr = (RoomInfo[,])ResizeArray(posArr, new int[] { (maxDistance * 2), (maxDistance * 2) });

        RealaseRoom();  // 초기화

        int x = Random.Range(0, maxDistance) + (int)(maxDistance / 2);  // 최대 크기 X 좌표
        int z = Random.Range(0, maxDistance) + (int)(maxDistance / 2);  // 최대 크기 Y 좌표

        startRoomPosition = new Vector3Int(x, 0, z);                        // 시작 좌표

        posArr[startRoomPosition.z, startRoomPosition.x]            = AddSingleRoom(new RoomInfo(), startRoomPosition, "Single");
        posArr[startRoomPosition.z, startRoomPosition.x].distance   = 0;
        currRoomCnt++;

        while (true)
        {
            if (!(minRoomCnt <= currRoomCnt && currRoomCnt <= maxRoomCnt))
            {
                FindRoomDistance(startRoomPosition, startRoomPosition);

                // 
                AddRoomLIst();

                int randRoomIdx = Random.Range(0, validRoomList.Count - 1);

                Vector3Int position = new Vector3Int(validRoomList[randRoomIdx].center_Position.x, 0, validRoomList[randRoomIdx].center_Position.z);
                MakeRoomArray(position);
            }
            else
                break;
        }
        FindRoomDistance(startRoomPosition, startRoomPosition);

        AddRoomLIst();
        SortRoomList(validRoomList);

        // 특수방 BOSS 방 생성
        AddBossRoom();

        SetupPosition();
    }
    


    public RoomInfo AddSingleRoom(RoomInfo room, Vector3Int pos, string name)
    {
        RoomInfo single             = room;
        single.roomID               = name + "(" + pos.x + ", " + pos.y + ", " + pos.z + ")";
        single.roomName             = name;
        single.center_Position      = pos;
        single.parent_Position      = pos;
        single.roomType             = "Single";
        single.isValidRoom = true;

        return single;
    }

    // 시작 방에서 해당 방까지의 거리 계산
    public void FindRoomDistance(Vector3Int currentPos, Vector3Int prePos)
    {
        // currentPos = 현재 위치
        // prePos     = 이전 위치
        if (!PossibleArr(currentPos))
            return;

        int _distance = posArr[currentPos.z, currentPos.x].distance;

        for (int i = 0; i < direction4.Count; i++)
        {
            Vector3Int adjustPosition = currentPos + direction4[i];

            if (PossibleArr(adjustPosition) && adjustPosition != prePos)
            {
                // 새로운 위치가 활성화가 되었을 경우
                if (posArr[adjustPosition.z, adjustPosition.x].isValidRoom)
                {
                    // 새로운 위치가 탐색했던 곳일 경우
                    if (posArr[adjustPosition.z, adjustPosition.x].distance != -1)
                    {
                        if ((_distance + 1) <= posArr[adjustPosition.z, adjustPosition.x].distance)
                        {
                            posArr[adjustPosition.z, adjustPosition.x].distance = _distance + 1;
                            FindRoomDistance(adjustPosition, currentPos);
                        }
                    }// 새로운 위치가 탐색하지 않은 곳일 경우
                    else
                    {
                        posArr[adjustPosition.z, adjustPosition.x].distance = _distance + 1;
                        FindRoomDistance(adjustPosition, currentPos);
                    }
                }
            }
        }
    }

    public void SortRoomList(List<RoomInfo> root)
    {
        root.Sort(delegate (RoomInfo A, RoomInfo B)
        {
            if (A.distance > B.distance)
                return 1;
            else if (A.distance < B.distance)
                return -1;
            else
                return 0;
        });
    }

    public bool PossibleArr(Vector3Int pos)
    {
        if ((0 <= (pos).x && (pos).x < (maxDistance * 2))
            && (0 <= (pos).z && (pos).z < (maxDistance * 2)))
        {
            return true;
        }
        else
            return false;
    }

    public void AddBossRoom()
    {
        SortRoomList(validRoomList);

        bool selectBossRoomStatus = false;

        for (int idx = validRoomList.Count - 1; 0 < idx; idx--)
        {
            if (!selectBossRoomStatus)
            {
                int setLIstCnt = idx;
                Vector3Int pos = validRoomList[setLIstCnt].center_Position;

                for (int i = 0; i < direction4.Count; i++)
                {
                    selectBossRoomStatus = false;
                    Vector3Int bossRoomPos = posArr[pos.z, pos.x].center_Position + direction4[i];

                    if (PossibleArr(bossRoomPos))
                    {
                        if ((AroundRoomCount(bossRoomPos) < 2)
                            && !posArr[bossRoomPos.z, bossRoomPos.x].isValidRoom)
                        {
                            posArr[bossRoomPos.z, bossRoomPos.x].roomName               = "Boss";
                            posArr[bossRoomPos.z, bossRoomPos.x].isValidRoom = true;
                            posArr[bossRoomPos.z, bossRoomPos.x].center_Position        = bossRoomPos;
                            posArr[bossRoomPos.z, bossRoomPos.x].parent_Position        = bossRoomPos;
                            posArr[bossRoomPos.z, bossRoomPos.x].mergeCenter_Position   = bossRoomPos;
                            posArr[bossRoomPos.z, bossRoomPos.x].distance               = posArr[pos.z, pos.x].distance + 1;
                            posArr[bossRoomPos.z, bossRoomPos.x].roomType               = "Single";

                            bossRoomPosition = bossRoomPos;
                            selectBossRoomStatus = true;

                            break;
                        }
                    }
                }
            }
        }
    }

    // 방의 배열을 초기화
    public void RealaseRoomPos()
    {
        for (int i = 0; i < (maxDistance * 2); i++)
        {
            for (int j = 0; j < (maxDistance * 2); j++)
            {
                posArr[j, i] = new RoomInfo();
                posArr[j, i].isValidRoom = false;
                posArr[j, i].distance = -1;
            }
        }
    }
    // 모든 변수를 초기화
    public void RealaseRoom()
    {
        RealaseRoomPos();
        validRoomList.Clear();

        currRoomCnt = 0;
    }

    // 배열의 방들을 RoomController의 List로 변환
    public void SetupPosition()
    {
        List<RoomInfo> roomsList = new List<RoomInfo>();

        for (int i = 0; i < (maxDistance * 2); i++)
        {
            for (int j = 0; j < (maxDistance * 2); j++)
            {
                if (posArr[j, i].isValidRoom)
                {
                    Vector3Int tmpArrayPosition = new Vector3Int(i, 0, j);

                    posArr[j, i]                        = SingleRoom(posArr[j, i], posArr[j, i].roomName);
                    posArr[j, i].center_Position        = tmpArrayPosition                  - startRoomPosition;
                    posArr[j, i].parent_Position        = posArr[j, i].parent_Position      - startRoomPosition;
                    posArr[j, i].mergeCenter_Position   = posArr[j, i].mergeCenter_Position - startRoomPosition;

                    roomsList.Add(posArr[j, i]);
                }
            }
        }
        validRoomCount = validRoomList.Count;

        for (int i = 0; i < roomsList.Count; i++)
            RoomController.Instance.LoadRoom(roomsList[i]);

    }
    public void AddRoomLIst()
    {
        validRoomList.Clear();

        for (int i = 0; i < (maxDistance * 2); i++)
        {
            for (int j = 0; j < (maxDistance * 2); j++)
            {
                if (posArr[j, i].isValidRoom)
                {
                    validRoomList.Add(posArr[j, i]);
                }
            }
        }
    }

    public RoomInfo SingleRoom(RoomInfo pos, string name)
    {
        RoomInfo single = pos;
        single.roomID = name + "(" + pos.center_Position.x + ", " + pos.center_Position.y + ", " + pos.center_Position.z + ")";
        single.roomName = name;
        single.center_Position = pos.center_Position;
        single.mergeCenter_Position = pos.mergeCenter_Position;
        single.roomType = pos.roomType;
        single.distance = pos.distance;

        return single;
    }

    public bool PossiblePatten(Vector3Int pos, List<Vector3Int> move)
    {
        bool possible = true;
        for (int i = 0; i < move.Count; i++)
        {
            Vector3Int next = pos + move[i];

            if (PossibleArr(next))
            {
                if (posArr[next.z, next.x].isValidRoom)
                    return false;
            }
            else
                return false;

        }

        return possible;
    }

    public int AroundRoomCount(Vector3Int pos)
    {
        int count = 0;

        // LEFT
        if ((0 <= (pos.x - 1) && (pos.x - 1) < (maxDistance * 2)))
        {
            if (posArr[pos.z, pos.x - 1].isValidRoom)
            {
                count += 1;
            }
        }

        // RIGHT
        if ((0 <= (pos.x + 1) && (pos.x + 1) < (maxDistance * 2)))
        {
            if (posArr[pos.z, pos.x + 1].isValidRoom)
            {
                count += 1;
            }
        }

        // TOP
        if ((0 <= (pos.z - 1) && (pos.z - 1) < (maxDistance * 2)))
        {
            if (posArr[pos.z - 1, pos.x].isValidRoom)
            {
                count += 1;
            }
        }
        // DOWN
        if ((0 <= (pos.z + 1) && (pos.z + 1) < (maxDistance * 2)))
        {
            if (posArr[pos.z + 1, pos.x].isValidRoom)
            {
                count += 1;
            }
        }

        return count;
    }



    // Room 위치 및 방의 크키 지정
    public void MakeRoomArray(Vector3Int start)
    {
        if (start.x >= (maxDistance * 2) || start.z >= (maxDistance * 2))
            return;

        if ((minRoomCnt <= currRoomCnt && currRoomCnt <= maxRoomCnt))
            return;

        // 사각형 방, 기억자, 니은자, -, |
        double[] persent = { 0.01, 0.03, 0.03, 0.1, 0.1, 0.1, 0.8 };

        // 각 방향 패턴을 List화
        List<Dictionary<int, List<Vector3Int>>> collectPatten
            = new List<Dictionary<int, List<Vector3Int>>> { downPatten, rightPatten, leftPatten, upPatten };


        for (int direction = 0; direction < direction4.Count; direction++)
        {
            bool directionsRand = (Random.value > 0.5f);

            if (directionsRand)
            {

                int selectPatten = (int)Choose(persent);

                if (!PossiblePatten(start, collectPatten[direction][selectPatten]))
                    return;

                if (!RoomCountCheck())
                {
                    Vector3Int lastMove = start;
                    Vector3 currCenterPos = Vector3.zero;
                    int maxCount = collectPatten[direction][selectPatten].Count;
                    for (int i = 0; i < maxCount; i++)
                    {
                        Vector3Int startPosition        = collectPatten[direction][selectPatten][0];
                        Vector3Int otherPosition        = collectPatten[direction][selectPatten][maxCount - 1];

                        currCenterPos = new Vector3((float)(startPosition.x + otherPosition.x) / 2, 0, (float)(startPosition.z + otherPosition.z) / 2);
                        Vector3Int move = start + collectPatten[direction][selectPatten][i];

                        posArr[move.z, move.x].isValidRoom              =  true;
                        posArr[move.z, move.x].roomName                 = "Single";
                        posArr[move.z, move.x].center_Position          = start + collectPatten[direction][selectPatten][i];
                        posArr[move.z, move.x].distance                 = -1;

                        lastMove = move;

                        // 미니맵 아이콘을 띄우기 위한 중앙 지점값 삽입
                        switch (collectPatten[direction][selectPatten].Count)
                        {
                            case 2:
                                posArr[move.z, move.x].roomType                 = "Double";
                                posArr[move.z, move.x].parent_Position          = start + collectPatten[direction][selectPatten][0];
                                posArr[move.z, move.x].mergeCenter_Position     = start + currCenterPos;
                                break;
                            case 3:
                                posArr[move.z, move.x].roomType                 = "Triple";
                                posArr[move.z, move.x].parent_Position          = start + collectPatten[direction][selectPatten][1];
                                posArr[move.z, move.x].mergeCenter_Position     = start + collectPatten[direction][selectPatten][1];

                                break;
                            case 4:
                                posArr[move.z, move.x].roomType                 = "Quad";
                                posArr[move.z, move.x].parent_Position          = start + collectPatten[direction][selectPatten][0];
                                posArr[move.z, move.x].mergeCenter_Position     = start + currCenterPos;
                                break;
                            default:
                                posArr[move.z, move.x].roomType                 = "Single";
                                posArr[move.z, move.x].parent_Position          = start + collectPatten[direction][selectPatten][0];
                                posArr[move.z, move.x].mergeCenter_Position     = start + currCenterPos;
                                break;
                        }
                    }
                    // 방의 갯수 증가
                    currRoomCnt++;
                    MakeRoomArray(lastMove);
                }
            }
        }
    }
    public bool RoomCountCheck()
    {
        return ((minRoomCnt <= currRoomCnt && currRoomCnt <= maxRoomCnt));
    }

    // 확률을 계산하여 패턴을 구성
    public double Choose(double[] probs)
    {
        double total = 0;

        foreach (double elem in probs)
            total += elem;

        double randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
                return i;
            else
                randomPoint -= probs[i];
        }
        return probs.Length - 1;
    }

    // 방의 갯수가 최소, 최대크기에 적합한지 체크

    private static System.Array ResizeArray(System.Array arr, int[] newSizes)
    {
        if (newSizes.Length != arr.Rank)
            return null;

        var temp = System.Array.CreateInstance(arr.GetType().GetElementType(), newSizes);
        int length = arr.Length <= temp.Length ? arr.Length : temp.Length;
        System.Array.ConstrainedCopy(arr, 0, temp, 0, length);
        return temp;
    }
}
