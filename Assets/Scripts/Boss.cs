using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss : MonoBehaviour
{
    [SerializeField] GameObject badSun;
    [SerializeField] ObjectPool badSunPool; // 나쁜 해 풀

    public static Boss instance2; //싱글톤2

    //해 생성
    const int minWaitTime = 5;
    const int maxWaitTime = 10; //대기시간

    Vector3 sunSpawnPos1 = Vector3.zero;
    Vector3 sunSpawnPos2 = Vector3.zero;

    //보스 모션값
    public float amplitude = 0.05f; 
    public float frequency = 0.5f;

    //보스 손
    [SerializeField] GameObject rightHand;
    [SerializeField] GameObject leftHand;
    Vector3 startPos;
    Vector3 leftStartPos;
    Vector3 rightStartPos;

    float downTime = 5;
    bool isRightHandDown = false;
    bool isLeftHandDown = false;

    //체력 및 UI
    public int hp = 8;
    [SerializeField] RectTransform hpBar;
    [SerializeField] Sprite deathImg;
    [SerializeField] GameObject gameClearText;

    //컴포넌트
    SpriteRenderer sr;

    private void Awake()
    {
        instance2 = this;
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startPos = transform.localPosition;
        leftStartPos = leftHand.transform.localPosition;
        rightStartPos = rightHand.transform.localPosition;
        StartCoroutine(SpawnSunCorRight());
        StartCoroutine(SpawnSunCorLeft()); //해 생성 시작
        StartCoroutine(HandDown()); //손 패턴 시작
    }

    private void Update()
    {
        //보스 모션
        float y = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startPos + new Vector3(0, y, 0);
        HandMove();
    }

    void HandMove() //손 모션 및 패턴
    {
        float x = Mathf.Sin(Time.time * frequency) * amplitude;
        float y = Mathf.Sin(Time.time * frequency) * amplitude / 0.5f;
        if (!isRightHandDown)
        {
            rightHand.transform.localPosition = rightStartPos + new Vector3(y, x, 0);
        }
        else
        {
            rightHand.transform.localPosition = rightStartPos + new Vector3(y, -3.5f, 0);

        }
        if (!isLeftHandDown)
        {
            leftHand.transform.localPosition = leftStartPos + new Vector3(x, y, 0);
        }
        else
        {
            leftHand.transform.localPosition = leftStartPos + new Vector3(x, -3.5f, 0);
        }
 
    }

    public void HandDamage()
    {
        hp--;
        isLeftHandDown = false;
        isRightHandDown = false;
        hpBar.sizeDelta = new Vector2(hp * 100, 80);
        if (hp <= 0)
        {
            Time.timeScale = 0;
            GameClear();
        }
    }

    IEnumerator HandDown() //주기적으로 손 내려옴
    {
        while(true)
        {
            yield return new WaitForSeconds(downTime);
            
            int hand = Random.Range(1, 3);
            if (hand == 1)
            {
                isLeftHandDown = true;
                yield return new WaitForSeconds(3);
                isLeftHandDown = false;
            }
            else
            {
                isRightHandDown = true;
                yield return new WaitForSeconds(3);
                isRightHandDown = false;
            }
        }
    }

    //나쁜 해 생성
    IEnumerator SpawnSunCorRight() 
    {
        while (true)
        {
            sunSpawnPos1.x = rightHand.transform.position.x + Random.Range(-5.0f, 5.0f);
            sunSpawnPos1.y = rightHand.transform.position.y + 0.5f;

            float waitTime = Random.Range(minWaitTime, maxWaitTime);

            GameObject bad = badSunPool.Get();
            bad.transform.position = sunSpawnPos1;
            bad.transform.rotation = Quaternion.identity;

            RemoveSun rs = bad.GetComponent<RemoveSun>();
            if (rs != null)
                rs.SetPool(badSunPool);

            yield return new WaitForSeconds(waitTime);
        }
    }
    IEnumerator SpawnSunCorLeft()
    {
        while (true)
        {
            sunSpawnPos2.x = leftHand.transform.position.x + Random.Range(-5.0f, 5.0f);
            sunSpawnPos2.y = leftHand.transform.position.y + 0.5f;

            float waitTime = Random.Range(minWaitTime, maxWaitTime);

            GameObject bad = badSunPool.Get();
            bad.transform.position = sunSpawnPos2;
            bad.transform.rotation = Quaternion.identity;

            RemoveSun rs = bad.GetComponent<RemoveSun>();
            if (rs != null)
                rs.SetPool(badSunPool);

            yield return new WaitForSeconds(waitTime);
        }
    }

    void GameClear()
    {
        Time.timeScale = 0;
        sr.sprite = deathImg;
        gameClearText.SetActive(true);
    }

}
