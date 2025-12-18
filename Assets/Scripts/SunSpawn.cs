using System.Collections;
using UnityEngine;

public class SunSpawn : MonoBehaviour
{
    [SerializeField] ObjectPool sunPool;

    [SerializeField] GameObject sun;
    
    Vector3 sunSpawnPos = Vector3.zero;

    //해 생성 시간
    float waitTime;
    const int minWaitTime = 3;
    const int maxWaitTime = 5;

    private void Start()
    {
        StartCoroutine(SpawnSunCor());
    }

    IEnumerator SpawnSunCor()
    {
        while (true)
        {
            sunSpawnPos.x = this.transform.position.x + Random.Range(-10.0f, 10.0f);
            sunSpawnPos.y = this.transform.position.y + 10;

            GameObject sun = sunPool.Get(); //오브젝트풀
            sun.transform.position = sunSpawnPos;
            sun.transform.rotation = Quaternion.identity;

            waitTime = Random.Range(minWaitTime, maxWaitTime); //랜덤 시간 지정
            yield return new WaitForSeconds(waitTime);
        }
    }
}
