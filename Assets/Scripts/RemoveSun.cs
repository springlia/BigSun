using System.Collections;
using UnityEngine;

public class RemoveSun : MonoBehaviour
{

    SpriteRenderer sr;
    [SerializeField] float lifeTime = 5f;
    ObjectPool pool;

    public void SetPool(ObjectPool objectPool)
    {
        pool = objectPool;
    }
    void OnEnable()
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        StartCoroutine(Remove());
    }

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(lifeTime);

        Color c = sr.color;

        for (int i = 0; i < 5; i++) //»ç¶óÁö±âÀü¿¡ ±ôºýÀÌ±â
        {
            c.a = 0.3f;
            sr.color = c;
            yield return new WaitForSeconds(0.2f);

            c.a = 1f;
            sr.color = c;
            yield return new WaitForSeconds(0.2f);
        }

        pool?.ReturnToPool(this.gameObject);
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public void ReturnToPool()
    {
        StopAllCoroutines();          // ±ôºýÀÓ ÄÚ·çÆ¾ Á¤Áö
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        pool?.ReturnToPool(gameObject);
    }
}
