using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public float timeLimitSecs = 120.0f;
    public Canvas startGameCanvas;
    public RandomPrefabSpawner[] spawners;
    public StartGame startGameObj;
    public AudioClip gameBGM;
    public AudioSource audioSource;
    public float audioFadeSpeed = 0.3f;

    float timeLeftSecs;

    public enum GameState {
        Start,
        Playing,
        GameOver
    }
    GameState _gameState = GameState.Start;
    public GameState gameState {
        get => _gameState;
        set {
            if (value == GameState.Start) {
                foreach (var spawner in spawners) {
                    spawner.shouldSpawn = false;
                }
            } else if (value == GameState.Playing) {
                StartBGM();
                foreach (var spawner in spawners) {
                    spawner.shouldSpawn = true;
                }
            } else if (value == GameState.GameOver) {
                StopBGM();
                foreach (var spawner in spawners) {
                    spawner.shouldSpawn = false;
                }
            }
            _gameState = value;
        }
    }

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
    int score = 0;

    void StartBGM() {
        if (audioSource.isPlaying)
            return;
        // fade in BGM and loop forever
        IEnumerator FadeInBGM() {
            audioSource.clip = gameBGM;
            audioSource.volume = 0.0f;
            audioSource.Play();
            while (audioSource.volume < 1.0f) {
                audioSource.volume += audioFadeSpeed * Time.deltaTime;
                yield return null;
            }
            audioSource.volume = 1.0f;
            audioSource.loop = true;
        }
        StartCoroutine(FadeInBGM());
    }

    void StopBGM() {
        if (!audioSource.isPlaying)
            return;
        // fade out BGM and stop
        IEnumerator FadeOutBGM() {
            while (audioSource.volume > 0.0f) {
                audioSource.volume -= audioFadeSpeed * Time.deltaTime;
                yield return null;
            }
            audioSource.volume = 0.0f;
            audioSource.Stop();
        }
        StartCoroutine(FadeOutBGM());
    }

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Init() {
        gameState = GameState.Start;
        timeLeftSecs = timeLimitSecs;
        Score = 0;
        startGameCanvas.gameObject.SetActive(true);
        LogHelper.instance.SetTimeLeft(timeLeftSecs);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        startGameObj.OnClick += () => {
            gameState = GameState.Playing;
            startGameCanvas.gameObject.SetActive(false);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.Start) {

        } else if (gameState == GameState.Playing) {
            timeLeftSecs -= Time.deltaTime;
            LogHelper.instance.SetTimeLeft(timeLeftSecs);
            if (timeLeftSecs <= 0.0f) {
                GameOver();
            }
        } else if (gameState == GameState.GameOver) {

        }
    }

    void GameOver() {
        var logHelper = LogHelper.instance;
        gameState = GameState.GameOver;

        IEnumerator GameOverCoroutine() {
            logHelper.SetGameOver(Score);
            yield return new WaitForSecondsRealtime(3.0f);
            yield return LoadSceneCoroutine();
            // reset game state
            Init();
        }

        IEnumerator LoadSceneCoroutine() {
            float loadingTime = 3.0f;
            float t = 0.0f;
            // jack, wtf is this? why is this fake loading?
            // come to my office and explain yourself
            while (t < loadingTime) {
                float progress = t / loadingTime;
                logHelper.SetLoadingProgress(progress);
                yield return new WaitForSecondsRealtime(0.1f);
                t += 0.1f;
            }
            logHelper.SetLoadingProgress(1.0f);
        }
        
        StartCoroutine(GameOverCoroutine());
    }
}
