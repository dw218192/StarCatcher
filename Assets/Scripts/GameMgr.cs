using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    public float timeLimitSecs = 120.0f;

    public static GameMgr instance = null;
    public int Score {
        get {
            return score;
        }
        set {
            score = value;
            LogHelper.instance.SetScore(score);
        }
    }
    int score;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;   
    }

    // Update is called once per frame
    void Update()
    {
        timeLimitSecs -= Time.deltaTime;
        LogHelper.instance.SetTimeLeft(timeLimitSecs);
        if (timeLimitSecs <= 0.0f) {
            GameOver();
        }
    }

    void GameOver() {
        IEnumerator GameOverCoroutine() {
            LogHelper.instance.SetGameOver(Score);
            yield return new WaitForSeconds(5.0f);
            SceneManager.LoadScene(0);
        }
        StartCoroutine(GameOverCoroutine());
    }
}
