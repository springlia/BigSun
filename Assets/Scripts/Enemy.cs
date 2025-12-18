using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 2.5f;
    SpriteRenderer spr;

    void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!spr.flipX)
        {
            this.transform.position += Vector3.left * Time.deltaTime * speed;
        }
        else
        {
            this.transform.position += Vector3.right * Time.deltaTime * speed;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy Move Limit")) //이동 범위 제한
        {
            if (!spr.flipX)
            {
                spr.flipX = true;
            }
            else
            {
                spr.flipX = false;
            }
        }
            
    }
}
