using UnityEngine;

public class Player : MonoBehaviour
{
    Vector3 dir = Vector3.zero;
    [SerializeField] float speed;
    [SerializeField] float jumpPower;
    bool canJump = true;

    public SpriteRenderer spr;
    public Rigidbody2D rb;

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

    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        dir.x = Input.GetAxis("Horizontal"); //이동
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
        else if (collision.gameObject.CompareTag("Sun"))
        {
            Destroy(collision.gameObject);
            Scale++;
            
        }
    }

    void ChangeSacle()
    {
        float newSize = 1 + (_scale - 1) * 0.1f;
        transform.localScale = Vector3.one * newSize;
    }
}
