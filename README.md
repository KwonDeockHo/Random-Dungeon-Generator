# Random-Dungeon-Generator Algorithm
Demo Games Using Unity. 
[The Binding of Isaac ] - Map Create Algorithm 

<a href="https://blog.naver.com/kon9383" target="_blank">Detail</a>

아이작에서 사용하는 Dungeon을 모티브로 하여 재구성한 소스 코드 입니다.


[Step 1]
 * N x N 형태의 배열 각각의 패턴을 지정하여 재귀 호출을 통해 방들의 정보들을 구성하여 맵의 형태로 생성

[Step 2]
 * 맵의 형태(N x N) 행렬을 통해 Prefabs으로 구성된 오브젝트를 맵의 정보에 따라 생성

[Step 3]
 * 각 방을 기준으로 인접한 방들을 고려하여 방의 상태 업데이트
   - 현재 방을 기준으로 위쪽에 위치한 방이 있을 경우 : 
      ㆍ 위의 방이 나와 같은 부모를 가지고 있는 방이라면 병합(벽 제거, 문 제거)
      ㆍ 위의 방이 나와 다른 부모를 가지고 있는 방이라면 분리(벽 생성, 문 생성)
      ㆍ 위의 방이 방으로 구성되어 있지 않은 빈 방이라면 분리(벽 생성, 문 제거)
    ※ 각 4방향에 맞게 반복하여 (위, 아래, 좌, 우) 모두 적용
 
[Step 4]
 * 위의 스텝에 따라 미니맵의 구성도 유사한 방식을 통해 생성
   - 방의 아래에 위치하여 Render Texture을 이용하여 Mainimap을 생성

 [Step 5]
 * 플레이어 이동에 따라 맵/미니맵을 업데이트
   - 미니맵의 경우 아래의 경우에 따라 색상을 변화
      ㆍ 방문 한 적이 있는 방인가
      ㆍ 아직 방문하지 않은 방인가
      ㆍ 현재 입장한 방인가
      

★ 개발언어/엔진

C#/ unity

★ 개발인원

1명
