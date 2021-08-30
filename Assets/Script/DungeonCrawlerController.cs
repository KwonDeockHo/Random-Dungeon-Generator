using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCrawlerController : Singleton<DungeonCrawlerController>
{
    public List<Vector3Int> direction4 = new List<Vector3Int>
    {
        new Vector3Int( 0, 0,  1),       // down
        new Vector3Int( 1, 0,  0),       // right
        new Vector3Int(-1, 0,  0),       // left
        new Vector3Int( 0, 0, -1)        // up
    };
    public List<Vector3Int> direction8 = new List<Vector3Int>
    {   // Down                           // Up
        new Vector3Int( 0, 0,  1),        new Vector3Int( 0, 0, -1),        
        // Left                           // Right
        new Vector3Int(-1, 0,  0),        new Vector3Int( 1, 0,  0),        
        // UpLeft                         // UpRight
        new Vector3Int(-1, 0, -1),        new Vector3Int( 1, 0, -1),
        // DiwbLeft                       // DownRight
        new Vector3Int(-1, 0,  1),        new Vector3Int( 1, 0,  1)
    };
    public Dictionary<int, List<Vector3Int>> downPatten = new Dictionary<int, List<Vector3Int>>
    {
        {  0, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(-1, 0, 1),   new Vector3Int(0, 0, 2),   new Vector3Int(-1, 0, 2) } }, // ㅁ
        {  1, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(0, 0, 2),    new Vector3Int(-1, 0, 2)    } }, // ┓
        {  2, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(0, 0, 2),    new Vector3Int(1, 0, 2)     } }, // ┏
        {  3, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(0, 0, 2)                                 } }, // 아래 |
        {  4, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(-1, 0, 1)                                } }, // 아래 |
        {  5, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(1, 0, 1)                                 } }, // 아래 |
        {  6, new List<Vector3Int>      { new Vector3Int(0, 0, 1)                                                            } }, // 아래 |
    };
    public Dictionary<int, List<Vector3Int>> upPatten = new Dictionary<int, List<Vector3Int>>
    {
        {  0, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(0, 0, -2),  new Vector3Int(-1, 0, -1),  new Vector3Int(-1, 0, -2)} }, // ㅁ
        {  1, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(0, 0, -2),   new Vector3Int(1, 0, -2)    } }, // ┏
        {  2, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(0, 0, -2),   new Vector3Int(-1, 0, -2)   } }, // ┐
        {  3, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(0, 0, -2)                                } }, // 위 |
        {  4, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(1, 0, -1)                                } }, // 위 |
        {  5, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(-1, 0, -1)                               } }, // 위 |
        {  6, new List<Vector3Int>      { new Vector3Int(0, 0, -1)                                                            } }, // 위 |
    };
    public Dictionary<int, List<Vector3Int>> leftPatten = new Dictionary<int, List<Vector3Int>>
    {
        {  0, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-2, 0, 0),   new Vector3Int(-1, 0, -1),  new Vector3Int(-2, 0, -1) } }, // ㅁ
        {  1, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-2, 0, 0),   new Vector3Int(-2, 0, -1)   } }, // └ 
        {  2, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-2, 0, 0),   new Vector3Int(-2, 0, 1)    } }, // ┌
        {  3, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-2, 0, 0)                                } }, // 왼쪽  --
        {  4, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-1, 0, -1)                               } }, // 왼쪽  --
        {  5, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-1, 0, 1)                                } }, // 왼쪽  --
        {  6, new List<Vector3Int>      { new Vector3Int(-1, 0, 0)                                                           } }, // 왼쪽  --
    };
    public Dictionary<int, List<Vector3Int>> rightPatten = new Dictionary<int, List<Vector3Int>>
    {
        {  0, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(2, 0, 0),    new Vector3Int(1, 0, 1) ,   new Vector3Int(2, 0, 1) } }, // ㅁ
        {  1, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(2, 0, 0),    new Vector3Int(2, 0, 1)     } }, // ┐
        {  2, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(2, 0, 0),    new Vector3Int(2, 0, -1)    } }, // ┛ 
        {  3, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(2, 0, 0)                                 } }, // 오른쪽  --
        {  4, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(1, 0, 1)                                 } }, // 오른쪽  --
        {  5, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(1, 0, -1)                                } }, // 오른쪽  --
        {  6, new List<Vector3Int>      { new Vector3Int(1, 0, 0)                                                            } },
    };

    public List<RoomInfo> mainRoot = new List<RoomInfo>();
    public List<Vector3Int> shortParentRoot = new List<Vector3Int>();

    public int minRoomCnt = 15;
    public int maxRoomCnt = 20;
    public int currRoomCnt = 0;
    public int maxDistance = 5;

    public int mainRoomCount = 0;

    public Vector3Int startPoint;
    public Vector3Int BossPoint;

    public RoomInfo[,] posArr = new RoomInfo[10, 10];

    public void CreatedRoom()
    {
        // 배열 ReSize
        posArr = (RoomInfo[,])ResizeArray(posArr, new int[] { (maxDistance * 2), (maxDistance * 2) });

        RealaseRoom();

        int x = Random.Range(0, maxDistance) + (int)(maxDistance / 2);
        int z = Random.Range(0, maxDistance) + (int)(maxDistance / 2);

        startPoint = new Vector3Int(x, 0, z);

        posArr[startPoint.z, startPoint.x] = singleRoom(new RoomInfo(), startPoint, "Single");
        posArr[startPoint.z, startPoint.x].weightCnt = 0;
        currRoomCnt++;

        while (true)
        {
            if (!(minRoomCnt <= currRoomCnt && currRoomCnt <= maxRoomCnt))
            {
                countRoomWeight(startPoint, startPoint);
                makeMainRoot();
                mainRoomSort(mainRoot);

                int Mainrand = Random.Range(0, mainRoot.Count - 1);

                Vector3Int position = new Vector3Int(mainRoot[Mainrand].currPos.x, 0, mainRoot[Mainrand].currPos.z);
                makeRoomArray(position);
            }
            else
                break;
        }
        countRoomWeight(startPoint, startPoint);
        makeMainRoot();

        // 특수방 BOSS 방 생성
        selectBossRoom();

        setupPosition();
    }
    


    public RoomInfo singleRoom(RoomInfo room, Vector3Int pos, string name)
    {
        RoomInfo single = room;
        single.roomID = name + "(" + pos.x + ", " + pos.y + ", " + pos.z + ")";
        single.roomName = name;
        single.currPos = pos;
        single.parentPos = pos;
        single.roomType = "Single";
        single.roomState = true;

        return single;
    }

    // 시작 방에서 해당 방까지의 거리 계산
    public void countRoomWeight(Vector3Int currentPos, Vector3Int prePos)
    {
        // currentPos = 현재 위치
        // prePos     = 이전 위치
        if (!possibleArr(currentPos))
            return;

        int weight = posArr[currentPos.z, currentPos.x].weightCnt;

        for (int i = 0; i < direction4.Count; i++)
        {
            Vector3Int newPosition = currentPos + direction4[i];

            if (possibleArr(newPosition) && newPosition != prePos)
            {
                // 새로운 위치가 활성화가 되었을 경우
                if (posArr[newPosition.z, newPosition.x].roomState)
                {
                    // 새로운 위치가 탐색했던 곳일 경우
                    if (posArr[newPosition.z, newPosition.x].weightCnt != -1)
                    {
                        if ((weight + 1) <= posArr[newPosition.z, newPosition.x].weightCnt)
                        {
                            posArr[newPosition.z, newPosition.x].weightCnt = weight + 1;
                            countRoomWeight(newPosition, currentPos);
                        }
                    }// 새로운 위치가 탐색하지 않은 곳일 경우
                    else
                    {
                        posArr[newPosition.z, newPosition.x].weightCnt = weight + 1;
                        countRoomWeight(newPosition, currentPos);
                    }
                }
            }
        }
    }

    public void mainRoomSort(List<RoomInfo> root)
    {
        root.Sort(delegate (RoomInfo A, RoomInfo B)
        {
            if (A.weightCnt > B.weightCnt)
                return 1;
            else if (A.weightCnt < B.weightCnt)
                return -1;
            else
                return 0;
        });
    }

    public bool possibleArr(Vector3Int pos)
    {
        if ((0 <= (pos).x && (pos).x < (maxDistance * 2))
            && (0 <= (pos).z && (pos).z < (maxDistance * 2)))
        {
            return true;
        }
        else
            return false;
    }

    public void selectBossRoom()
    {
        mainRoomSort(mainRoot);

        bool selectBossRoom = false;

        for (int idx = mainRoot.Count - 1; 0 < idx; idx--)
        {
            if (!selectBossRoom)
            {
                int setLIstCnt = idx;
                Vector3Int pos = mainRoot[setLIstCnt].currPos;

                for (int i = 0; i < direction4.Count; i++)
                {
                    selectBossRoom = false;
                    Vector3Int bossMove = posArr[pos.z, pos.x].currPos + direction4[i];

                    if (possibleArr(bossMove))
                    {
                        if ((aroundRoomCount(bossMove) < 2)
                            && !posArr[bossMove.z, bossMove.x].roomState)
                        {
                            posArr[bossMove.z, bossMove.x].roomName = "Boss";
                            posArr[bossMove.z, bossMove.x].roomState = true;
                            posArr[bossMove.z, bossMove.x].currPos = bossMove;
                            posArr[bossMove.z, bossMove.x].parentPos = bossMove;
                            posArr[bossMove.z, bossMove.x].CenterPos = bossMove;
                            posArr[bossMove.z, bossMove.x].weightCnt = posArr[pos.z, pos.x].weightCnt + 1;
                            posArr[bossMove.z, bossMove.x].roomType = "Single";

                            BossPoint = bossMove;
                            selectBossRoom = true;

                            break;
                        }
                    }
                }
            }
        }
    }

    // 방의 배열을 초기화
    public void roomPosInit()
    {
        for (int i = 0; i < (maxDistance * 2); i++)
        {
            for (int j = 0; j < (maxDistance * 2); j++)
            {
                posArr[j, i] = new RoomInfo();
                posArr[j, i].roomState = false;
                posArr[j, i].weightCnt = -1;
            }
        }
    }
    // 모든 변수를 초기화
    public void RealaseRoom()
    {
        roomPosInit();
        mainRoot.Clear();
        shortParentRoot.Clear();

        currRoomCnt = 0;
    }

    // 배열의 방들을 RoomController의 List로 변환
    public void setupPosition()
    {
        List<RoomInfo> positions = new List<RoomInfo>();

        for (int i = 0; i < (maxDistance * 2); i++)
        {
            for (int j = 0; j < (maxDistance * 2); j++)
            {
                if (posArr[j, i].roomState)
                {
                    Vector3Int tmp = new Vector3Int(i, 0, j);

                    posArr[j, i] = singleRoom(posArr[j, i], posArr[j, i].roomName);

                    posArr[j, i].currPos = tmp - startPoint;
                    posArr[j, i].parentPos = posArr[j, i].parentPos - startPoint;
                    posArr[j, i].CenterPos = posArr[j, i].CenterPos - startPoint;

                    positions.Add(posArr[j, i]);
                }
            }
        }
        mainRoomCount = mainRoot.Count;
        for (int i = 0; i < positions.Count; i++)
            RoomController.Instance.LoadRoom(positions[i]);

    }
    public void makeMainRoot()
    {
        mainRoot.Clear();

        for (int i = 0; i < (maxDistance * 2); i++)
        {
            for (int j = 0; j < (maxDistance * 2); j++)
            {
                if (posArr[j, i].roomState)
                {
                    mainRoot.Add(posArr[j, i]);
                }
            }
        }
    }

    public RoomInfo singleRoom(RoomInfo pos, string name)
    {
        RoomInfo single = pos;
        single.roomID = name + "(" + pos.currPos.x + ", " + pos.currPos.y + ", " + pos.currPos.z + ")";
        single.roomName = name;
        single.currPos = pos.currPos;
        single.parentPos = pos.parentPos;
        single.roomType = pos.roomType;
        single.weightCnt = pos.weightCnt;

        return single;
    }

    public bool possiblePatten(Vector3Int pos, List<Vector3Int> move)
    {
        bool possible = true;
        for (int i = 0; i < move.Count; i++)
        {
            Vector3Int next = pos + move[i];

            if (possibleArr(next))
            {
                if (posArr[next.z, next.x].roomState)
                    return false;
            }
            else
                return false;

        }

        return possible;
    }

    public int aroundRoomCount(Vector3Int pos)
    {
        int count = 0;

        // LEFT
        if ((0 <= (pos.x - 1) && (pos.x - 1) < (maxDistance * 2)))
        {
            if (posArr[pos.z, pos.x - 1].roomState)
            {
                count += 1;
            }
        }

        // RIGHT
        if ((0 <= (pos.x + 1) && (pos.x + 1) < (maxDistance * 2)))
        {
            if (posArr[pos.z, pos.x + 1].roomState)
            {
                count += 1;
            }
        }

        // TOP
        if ((0 <= (pos.z - 1) && (pos.z - 1) < (maxDistance * 2)))
        {
            if (posArr[pos.z - 1, pos.x].roomState)
            {
                count += 1;
            }
        }
        // DOWN
        if ((0 <= (pos.z + 1) && (pos.z + 1) < (maxDistance * 2)))
        {
            if (posArr[pos.z + 1, pos.x].roomState)
            {
                count += 1;
            }
        }

        return count;
    }



    // Room Position 생성
    public void makeRoomArray(Vector3Int start)
    {
        if (start.x >= (maxDistance * 2) || start.z >= (maxDistance * 2))
            return;

        if ((minRoomCnt <= currRoomCnt && currRoomCnt <= maxRoomCnt))
            return;

        // 사각형 방, 기억자, 니은자, -, |
        double[] persent = { 0.01, 0.03, 0.03, 0.1, 0.1, 0.1, 0.8 };

        // 각 방향 패턴을 List화
        List<Dictionary<int, List<Vector3Int>>> allPatten
            = new List<Dictionary<int, List<Vector3Int>>> { downPatten, rightPatten, leftPatten, upPatten };


        for (int direction = 0; direction < direction4.Count; direction++)
        {
            bool directionsRand = (Random.value > 0.5f);

            if (directionsRand)
            {

                int thisPatten = (int)Choose(persent);

                if (!possiblePatten(start, allPatten[direction][thisPatten]))
                    return;

                if (!roomCountCheck())
                {
                    Vector3Int lastMove = start;
                    Vector3 currCenterPos = Vector3.zero;
                    int maxCount = allPatten[direction][thisPatten].Count;
                    for (int i = 0; i < maxCount; i++)
                    {
                        Vector3Int firstPos = allPatten[direction][thisPatten][0];
                        Vector3Int SecondPos = allPatten[direction][thisPatten][maxCount - 1];
                        currCenterPos = new Vector3((float)(firstPos.x + SecondPos.x) / 2, 0, (float)(firstPos.z + SecondPos.z) / 2);
                        Vector3Int move = start + allPatten[direction][thisPatten][i];

                        posArr[move.z, move.x].roomState = true;
                        posArr[move.z, move.x].roomName = "Single";
                        posArr[move.z, move.x].currPos = start + allPatten[direction][thisPatten][i];
                        posArr[move.z, move.x].weightCnt = -1;
                        lastMove = move;

                        switch (allPatten[direction][thisPatten].Count)
                        {
                            case 2:
                                posArr[move.z, move.x].roomType = "Double";
                                posArr[move.z, move.x].parentPos = start + allPatten[direction][thisPatten][0];
                                posArr[move.z, move.x].CenterPos = start + currCenterPos;
                                break;
                            case 3:
                                posArr[move.z, move.x].roomType = "Triple";
                                posArr[move.z, move.x].parentPos = start + allPatten[direction][thisPatten][1];
                                posArr[move.z, move.x].CenterPos = start + allPatten[direction][thisPatten][1];

                                break;
                            case 4:
                                posArr[move.z, move.x].roomType = "Quad";
                                posArr[move.z, move.x].parentPos = start + allPatten[direction][thisPatten][0];
                                posArr[move.z, move.x].CenterPos = start + currCenterPos;
                                break;
                            default:
                                posArr[move.z, move.x].roomType = "Single";
                                posArr[move.z, move.x].parentPos = start + allPatten[direction][thisPatten][0];
                                posArr[move.z, move.x].CenterPos = start + currCenterPos;
                                break;
                        }
                    }
                    currRoomCnt++;
                    makeRoomArray(lastMove);
                }
            }
        }
    }
    public bool roomCountCheck()
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
