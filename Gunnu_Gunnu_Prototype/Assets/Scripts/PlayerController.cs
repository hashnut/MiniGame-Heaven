using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour 
{
    public AudioClip jumpClip;
    public AudioClip deathClip; // 사망시 재생할 오디오 클립

    private float initialX = -6f;
    private float initialY = 1f;

    // Player stat variable
    private float jumpSpeed = 2.5f;
    private float singleJumpHeight = 5.0f;
    private float doubleJumpHeight = 2.5f;
    private float leapDist = 1.0f;
    private float doubleJumpTimeLimit = 0.3f;

    private int jumpCount = 0; // 누적 점프 횟수
    private bool isJumping = false;
    private float jumpingTime = 0.0f;

    private bool isGrounded = false; // 바닥에 닿았는지 나타냄
    private bool isDead = false; // 사망 상태

    private float lastX;
    private float lastY;

    private float targetX;
    private float targetY;

    private float timeAfterJump = 0.0f;
    
    private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
    private Animator animator; // 사용할 애니메이터 컴포넌트
    private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트

    private void Start() 
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        transform.position = new Vector2(initialX, initialY);
    }

    private void Update() 
    {
        if (isDead)
        {
            return;
        }

        Jump();
        animator.SetBool("Grounded", isGrounded);
    }

    private void Jump()
    {
        if (jumpCount == 0)
        {
            timeAfterJump = 0.0f;
        }

        if (jumpCount == 1)
        {
            timeAfterJump += Time.deltaTime;
        }

        // move along the parabola
        if (isJumping)
        {
            float jumpHeight = 0f;

            if (jumpCount == 1)
            {
                jumpHeight = singleJumpHeight;
                transform.position = MathParabola.Parabola(new Vector2(lastX, lastY), new Vector2(targetX, targetY), jumpHeight, lastX + jumpingTime, jumpSpeed);
            }
            else if (jumpCount == 2)
            {
                jumpHeight = doubleJumpHeight;
                transform.position = MathParabola.ParabolaDouble(new Vector2(lastX, lastY), new Vector2(targetX, targetY), jumpHeight, lastX + jumpingTime, jumpSpeed);
            }

            jumpingTime += Time.deltaTime;

            if (transform.position.x == targetX)
            {
                isJumping = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        //if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount == 0)
            {
                if (!isGrounded)
                {
                    return;
                }

                jumpCount += 1;
                isJumping = true;

                lastX = transform.position.x;
                lastY = transform.position.y;

                targetX = lastX + leapDist;
                targetY = lastY;

                playerAudio.Play();
            }
            else if (jumpCount == 1)
            {
                if (timeAfterJump < doubleJumpTimeLimit)
                {
                    jumpCount += 1;
                    isJumping = true; // redundant
                   
                    targetX = targetX + leapDist;

                    playerAudio.Play();
                }
            }
        }
    }

    private void Die() 
    {
        animator.SetTrigger("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play();

        playerRigidbody.velocity = Vector2.zero;

        isDead = true;

        GameManager.instance.OnPlayerDead();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Dead" && !isDead)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        isGrounded = true;
        jumpCount = 0;
        isJumping = false;
        jumpingTime = 0.0f;

        lastX = transform.position.x;
        lastY = transform.position.y;
    }

    private void OnCollisionExit2D(Collision2D collision) 
    {
        isGrounded = false;
    }
    
    public float GetLeapDist()
    {
        return leapDist;
    }

    public float GetInitialX()
    {
        return initialX;
    }
}