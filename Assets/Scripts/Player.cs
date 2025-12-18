using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    //오디오
    [SerializeField] AudioClip attack;
    [SerializeField] AudioSource audioSource;

    //이동
    Vector3 dir = Vector3.zero;
    [SerializeField] float speed = 5;
    [SerializeField] float jumpPower = 300;
    bool canJump = true;
    
    //컴포넌트
    public SpriteRenderer spr;
    public Rigidbody2D rb;

    //크기(체력)
    [SerializeField] int _scale = 1;
    
    public int Scale
    {
        get => _scale;
        set
        {
            _scale = Mathf.Clamp(value, 1, 10);
            ChangeSacle();
        }
    }

    public int hp = 3;
    [SerializeField] GameObject[] hpUI;

    //중력
    [SerializeField] float baseGravityRadius = 3f;
    [SerializeField] float baseGravityForce = 2f;


    void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        
    }
    private void Start()
    {
        StartCoroutine(ScaleSmalling());
    }

    void Update()
    {
        PlayerMove();
        ApplyGravityEffect();
    }

    void PlayerMove()
    {
        dir.x = Input.GetAxis("Horizontal");
        dir.y = Input.GetAxis("Vertical");
        this.transform.position += dir * speed * Time.deltaTime;

        if (Input.GetButtonDown("Horizontal")) //캐릭터 좌우반전
        {
            spr.flipX = Input.GetAxisRaw("Horizontal") == 1;
        }

        if (Input.GetKey(KeyCode.Space) && canJump) //점프
        {
            rb.AddForce(Vector3.up * jumpPower);
            canJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) //점프 초기화
        {
            canJump = true;
        }
        else if (collision.gameObject.CompareTag("Sun")) //해 흡수
        {
            Physics2D.IgnoreCollision(collision.collider,GetComponent<Collider2D>());
            RemoveSun sunScript = collision.gameObject.GetComponent<RemoveSun>();
            if (sunScript != null)
            {
                sunScript.ReturnToPool();
            }
            else
            {
                Destroy(collision.gameObject);
            }
            Scale++;
            
        }
        else if (collision.gameObject.CompareTag("Enemy")) //적 충돌
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            if (Scale > 1)
            {
                GameManager.instance.score++;
                Scale--;
                audioSource.clip = attack;
                audioSource.Play();
            }
            else
            {
                Damage();
            }
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("Bad Sun")) //나쁜 해 흡수
        {
            if (Scale > 1)
            {
                Scale--;
                audioSource.clip = attack;
                audioSource.Play();
            }

            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());

            RemoveSun badSunScript = collision.gameObject.GetComponent<RemoveSun>();
            if (badSunScript != null)
            {
                badSunScript.ReturnToPool();
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }

        else if (collision.gameObject.CompareTag("Death Zone")) //추락
        {
            Damage();
        }
        else if (collision.gameObject.CompareTag("Boss Hand")) //보스 손 충돌
        {
            if (Scale > 1)
            {
                Scale--;
                Boss.instance2.HandDamage();
                audioSource.clip = attack;
                audioSource.Play();
            }
            else
            {
                Damage();
            }
        }
        else if (collision.gameObject.CompareTag("Finish")) //도착
        {
            hp = 3;
            hpUI[2].SetActive(true);
            hpUI[1].SetActive(false);
            SceneManager.LoadScene(2);
        }
    }

    void Damage()
    {
        Scale = 1;
        hp--;
        hpUI[hp].SetActive(false);
        this.transform.position = Vector3.up * -4.25f;
        audioSource.clip = attack;
        audioSource.Play();
        if (hp <= 0)
        {
            GameManager.instance.gameOverText.SetActive(true);
            Time.timeScale = 0;
        }
    }

    IEnumerator ScaleSmalling() //20초마다 크기 감소
    {
        while(true)
        {
            yield return new WaitForSeconds(20);
            if (Scale >1)
            {
                Scale--;
            }
        }
    }

    void ChangeSacle() //크기 변경
    {
        float newSize = 1 + (_scale - 1) * 0.1f;
        transform.localScale = Vector3.one * newSize;
    }

    void ApplyGravityEffect() //해(플레이어) 중력
    {
        float radius = baseGravityRadius * (1 + (_scale - 1) * 0.3f);
        float force = baseGravityForce * (1 + (_scale - 1) * 0.5f);

        Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var obj in objs)
        {
            if (obj.attachedRigidbody != null && obj.gameObject != this.gameObject)
            {
                // Ground는 제외
                if (obj.CompareTag("Ground")) continue;

                Vector2 dirToPlayer = (transform.position - obj.transform.position);
                float dist = dirToPlayer.magnitude;

                // 거리 비례로 힘 약화
                float pull = Mathf.Lerp(force, 0, dist / radius);

                obj.attachedRigidbody.AddForce(dirToPlayer.normalized * pull, ForceMode2D.Force);
            }
        }
    }

    //중력 반경 확인
    private void OnDrawGizmosSelected()
    {
        float radius = baseGravityRadius * (1 + (_scale - 1) * 0.3f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
