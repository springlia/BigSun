using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public static GameManager instance; //ΩÃ±€≈Ê

    [SerializeField] TextMeshProUGUI scoreText;
    public GameObject gameOverText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(ConnectUICoru());
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        gameOverText = null;
        scoreText = null;

        StartCoroutine(ConnectUICoru());
    }


    IEnumerator ConnectUICoru()
    {
        yield return null;

        //UI ø¨∞·
        gameOverText = GameObject.Find("GameOver Text");
        scoreText = GameObject.Find("Score Text")?.GetComponent<TextMeshProUGUI>();

        yield return null;
        gameOverText.SetActive(false);
    }


    private void Update()
    {
        if (scoreText != null)
            scoreText.text = score.ToString(); //¡°ºˆ≈ÿΩ∫∆Æ
    }



}
