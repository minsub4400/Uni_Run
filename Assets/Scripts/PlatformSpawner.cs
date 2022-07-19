using UnityEngine;

// 발판을 생성하고 주기적으로 재배치하는 스크립트
public class PlatformSpawner : MonoBehaviour {
    public GameObject platformPrefab; // 생성할 발판의 원본 프리팹
    public int count = 3; // 생성할 발판의 개수

    // 발판 배치 시간 간격
    public float timeBetSpawnMin = 1.25f; // 다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMax = 2.25f; // 다음 배치까지의 시간 간격 최댓값
    private float timeBetSpawn; // 다음 배치까지의 시간 간격

    // 발판 배치 위치
    public float yMin = -3.5f; // 배치할 위치의 최소 y값
    public float yMax = 1.5f; // 배치할 위치의 최대 y값
    private float xPos = 20f; // 배치할 위치의 x 값

    private GameObject[] platforms; // 미리 생성한 발판들
    private int currentIndex = 0; // 사용할 현재 순번의 발판

    private Vector2 poolPosition = new Vector2(0, -25); // 초반에 생성된 발판들을 화면 밖에 숨겨둘 위치
    private float lastSpawnTime; // 마지막으로 배치한 발판으로 부터 지난 시간


    void Start() 
    {
        // 변수들을 초기화하고 사용할 발판들을 미리 생성
        platforms = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);
        }
        // 0으로 초기에 초기화를 시켜두면 지연시간 없이 발판을 생성할 수 있음
        timeBetSpawn = lastSpawnTime = 0;
    }

    void Update() 
    {
        // 순서를 돌아가며 주기적으로 발판을 배치

        // 게임오버면 아무런 처리 하지 않음
        if (GameManager.instance.isGameover)
        {
            return;
        }
        
        // 마지막 배치 시점에서 timeBetSpawn 이상 시간이 흘렀다면
        if (Time.time >= lastSpawnTime + timeBetSpawn) // Time.time은 게임이 시작되고 나서 몇초가 지났는지 저장되어있는 변수입니다.
        { // 즉, 현재 시점이 생성주기 + 배치가 되고 난 후 지난 시간과 같아지거나 커지면 
            // 마지막 배치 시점을 현재 시점으로 업데이트 하고
            lastSpawnTime = Time.time;
            // 배치 간격 저장
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            // 배치 위치 저장
            float yPos = Random.Range(yMin, yMax);

            // Platform 컴포넌트 재시작을 위한 해당 인덱스의 발판을 false후, true
            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true);

            // 발판 배치
            platforms[currentIndex].transform.position = new Vector2(xPos, yPos);

            // 다음 순번 발판 지정
            currentIndex++;


            // 순번이 한번 돌았다면 0으로 리셋하여 반복
            if (currentIndex >= count)
            {
                currentIndex = 0;
            }
        }

    }
}