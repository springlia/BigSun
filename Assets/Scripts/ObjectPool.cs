using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject sun; //해 원본
    [SerializeField] int poolSize = 10;

    private List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(sun);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    //오브젝트 가져오기
    public GameObject Get()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);

                // Sun이면 pool 참조 연결
                RemoveSun sunScript = obj.GetComponent<RemoveSun>();
                if (sunScript != null)
                    sunScript.SetPool(this);

                return obj;
            }
        }

        //부족하면 생성
        GameObject newObj = Instantiate(sun);
        newObj.SetActive(true);
        pool.Add(newObj);

        RemoveSun newSunScript = newObj.GetComponent<RemoveSun>();
        if (newSunScript != null)
            newSunScript.SetPool(this);

        return newObj;
    }


    //풀 반환
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
