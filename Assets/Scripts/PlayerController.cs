using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour {
   public AudioClip deathClip; // 사망시 재생할 오디오 클립
   public float jumpForce = 700f; // 점프 힘

   private int jumpCount = 0; // 누적 점프 횟수
   private bool isGrounded = false; // 바닥에 닿았는지 나타냄
   private bool isDead = false; // 사망 상태

   private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
   private Animator animator; // 사용할 애니메이터 컴포넌트
   private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트

   private void Start() 
    {
        // 초기화
        // 사용할 컴포넌트를 가져와 초기화 시킨다.
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

   private void Update() 
    {
       // 사용자 입력을 감지하고 점프하는 처리
       if (isDead) // 죽었을 경우 아무런 처리를 하지 않음
        {
            return;
        }
       
       if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 3)
        {
            jumpCount++;
            //playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            playerAudio.Play();
        }
       else if (Input.GetKeyUp(KeyCode.Space) && playerRigidbody.velocity.y > 0)
        {
            // 이걸 하면 키를 얼마만큼 누르냐에 따라 점프 높이가 달라짐
            playerRigidbody.velocity = playerRigidbody.velocity * 0.5f;
        }

        animator.SetBool("Grounded", isGrounded);
    }

   private void Die() 
    {
        // 사망 처리
        animator.SetTrigger("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play();
        playerRigidbody.velocity = Vector2.zero;
        isDead = true;
        GameManager.instance.OnPlayerDead();
    }

   private void OnTriggerEnter2D(Collider2D other) 
    {
       // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
       if (other.tag == "Dead" && !isDead)
        {
            Die();
        }
    }

   private void OnCollisionEnter2D(Collision2D collision) 
    {
        // 바닥에 닿았음을 감지하는 처리
        if (collision.contacts[0].normal.y > 0.7f) // 충돌 표면이 위쪽(0 > 1)이며, 경사가 너무 급하지 않은지 검사
        {
            jumpCount = 0;
            isGrounded = true;
        }
    }

   private void OnCollisionExit2D(Collision2D collision) 
    {
        // 바닥에서 벗어났음을 감지하는 처리
        isGrounded = false;
    }

}