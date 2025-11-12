using System.Collections;
using UnityEngine;

public class SunSpawn : MonoBehaviour
{
    [SerializeField] GameObject Sun;
    float waitTime = 3;
    Vector3 sunSpawnPos = Vector3.zero;

    private void Start()
    {
        StartCoroutine(SpawnSunCor());
    }

    IEnumerator SpawnSunCor()
    {
        while (true)
        {
            sunSpawnPos.x = this.transform.position.x + Random.Range(-5.0f, 5.0f);
            sunSpawnPos.y = this.transform.position.y + 5;
            waitTime = Random.Range(1, 4);
            Instantiate(Sun, sunSpawnPos, Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
