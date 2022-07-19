using UnityEngine;

// 발판으로서 필요한 동작을 담은 스크립트
public class Platform : MonoBehaviour 
{
    public GameObject[] obstacles; // 장애물 오브젝트들, 여러 장애물들을 한번에 다룰 수 있게 배열로 선언하였다.
    private bool stepped = false; // 플레이어 캐릭터가 밟았었는가 밟으면 true

    // 컴포넌트가 활성화될때 마다 매번 실행되는 유니트 이벤트 메서드
    // 컴포넌트의 체크 박스가 활성, 비활성 되는 것으로 판단할 수 있다.
    private void OnEnable() 
    {
        // 발판을 리셋하는 처리
        stepped = false;
        for (int i = 0; i < obstacles.Length; i++) // 장애물 게임 오브젝트 중 1/3확률로 활성화시킴
        {
            if (Random.Range(0, 3) == 0)
            {
                obstacles[i].SetActive(true);
            }
            else
            {
                obstacles[i].SetActive(false);
            }
        }

    }

    void OnCollisionEnter2D(Collision2D collision) 
    {
        // 플레이어 캐릭터가 자신을 밟았을때 점수를 추가하는 처리
        if (collision.collider.tag == "Player" && !stepped)
        {
            stepped = true;
            GameManager.instance.AddScore(1);
        }
    }
}