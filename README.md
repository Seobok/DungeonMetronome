# DungeonMetronome
2D Dungeon PC Game

## 게임 소개
### 메인
![image](https://github.com/user-attachments/assets/a6b96998-c33a-4040-9c2a-e417a9bf2fb6)
![image](https://github.com/user-attachments/assets/9bb6bbc8-9507-4eff-bd5d-d33f04e2b483)

- 게임 시작 및 설정 탭에서 음량 조절등을 할 수 있고, 도움말을 통해 간단한 게임 설명을 볼 수 있음

### 게임
![image](https://github.com/user-attachments/assets/10102550-d741-4303-87d4-e7bee783ba32)

- 이동시 타일을 한칸씩 움직이며 플레이어의 공격범위 내에 적이있다면 이동대신 공격을 함
- 플레이어의 행동이 끝나면 적이 행동을 진행함
- 마찬가지로 공격범위 내에 플레이어가 있다면 공격을 시도함

![image](https://github.com/user-attachments/assets/ac53c53e-6aa5-450b-b46c-88e5bec3927e)
- 플레이어는 power버튼을 눌러 공격할 때 특수한 기믹을 수행할 수 있고 해당 기믹에 성공하면 더 강하게 공격할 수 있음 

### 데모 버전 플레이영상
https://youtu.be/OVh9YMtHkIw?si=9fi1VLnH7xVIjiDv

## 개발
- 초기 개발에서는 각 오브젝트 (타일, 적 및 플레이어)를 MonoBehaviour 클래스를 상속받아 사용하여 오브젝트에 붙여 사용하였으나 Unity의 생명주기에 관리될 필요가 없었기 때문에 성능적 개선을 위해 이를 제거하였습니다.
- 길찾기 알고리즘은 A*알고리즘을 사용하고 있습니다.
- 적 AI는 BehaviourTree를 이용하여 사용하고 있습니다.
- 현재 DI를 위해 VContainer를 사용하고 있습니다.
- 개발중...
