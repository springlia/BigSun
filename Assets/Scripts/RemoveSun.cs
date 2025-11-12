using System.Collections;
using UnityEngine;

public class RemoveSun : MonoBehaviour
{

    SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(Remove());
    }


    IEnumerator Remove()
    {
        yield return new WaitForSeconds(5);
        Color c = sr.color;
        for (int i =0; i < 5; i++)
        {
            c.a = 0.3f;
            sr.color = c;
            yield return new WaitForSeconds(0.2f);
            c.a = 1;
            sr.color = c;
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(this.gameObject);
    }
}
