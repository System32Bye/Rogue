# Time Rogue
--------------------
### YouTube Link

 https://youtu.be/vFeyINWD1Zg


### 게임 소개
--------------------
1. 타이틀
  * Time Rogue

2. 개요
  * 적들이 존재하는 건물에서 살아남으며 50층까지 올라가 탈출하는 사람을 인터넷 스트리밍하는 컨셉의 게임

3. 플랫폼
  * PC

4. 장르
  * Roguelike

5. 타켓 플레이어
  * 전연령

### 게임 특징
--------------------
게임을 처음 시작하면 100초의 제한시간이 생기고 제한시간이 0이 되면 게임오버가 된다.
각 층마다 현재 몇 층인지를 출력하고 플레이어가 죽으면 몇 층에서 죽었는지 출력된다.
50층에서 한번 더 골 지점으로 가면 Ending 씬이 나오며 게임이 끝나게 된다.
  
1. BoardManager
  * 스테이지를 건물의 층으로 나눠두었고 층은 매번 스테이지가 시작할 때마다 바닥, 외벽, 내벽, 몬스터, 아이템을 랜덤하게 생성한다.  
  * 아이템과 몬스터는 층에 따라 나오는 수의 범위가 다르다.
  
2. Player
  * 아이템과 Collider가 겹치면 제한시간이 증가한다. 몬스터의 공격을 받으면 제한시간이 감소한다.
  
  * 플레이어 캐릭터는 총 3가지의 공격 방식을 가지고 있는데 키보드 상단 숫자키 1, 2, 3을 누르면 장비를 바꾸고 'C'키를 눌러 공격한다.
    - 1번의 맨손으로는 데미지가 약한 대신 맵 내부의 바위를 부술 수 있다.
    - 2번의 총은 맨손보다 데미지가 강며 공격 거리가 길지만 장전되어있는 탄을 전부 사용한다면 재장전을 해야 공격가능하다.
    - 3번의 도끼는 총보다 데미지가 강한 대신 공격 거리가 짧다.

3. Enermy
  * 플레이어와의 거리에 따라 State가 변경하며 State에 따라 행동이 바뀐다.
    - 플레이어를 추격하는 Trace
    - 플레이어를 공격하는 Attack
    - 아무 행동도 하지 않는 Idel
    - 몬스터의 체력이 0이하로 떨어지면 Dead


### 게임 기획서
-------------------
 * Google Drive Link
   - https://drive.google.com/drive/folders/17W4BKdqBwimlz1cV8YQuSwyj9f0F1Kio?usp=sharing
  
