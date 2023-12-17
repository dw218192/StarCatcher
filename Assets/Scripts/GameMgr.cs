using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public int MaxWaveCnt = 3;
    public float timeLimitSecs = 120.0f;
    public RandomPrefabSpawner[] spawners;
    public AudioClip gameBGM;
    public AudioSource audioSource;
    public float audioFadeSpeed = 0.3f;

    float timeLeftSecs;

    public enum GameState {
        Start,
        Playing,
        Intermission,
        GameOver
    }
    GameState _gameState = GameState.Start;

    public event System.Action<GameState, GameState> OnGameStateChange;

    public GameState gameState {
        get => _gameState;
        set {
            if (value != _gameState) {
                OnGameStateChange?.Invoke(_gameState, value);
            }

            if (value == GameState.Start) {
                StopBGM();
            } else if (value == GameState.Playing) {
                StartBGM();
            } else if (value == GameState.Intermission) {
                StopBGM();
                Intermission();
            } else if (value == GameState.GameOver) {
                StopBGM();
                GameOver();
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
            GameCanvas.instance.SetScore(score);
        }
    }
    int score = 0;
    public int Credit
    {
        get
        {
            return credit;
        }
        set
        {
            credit = value;
            GameCanvas.instance.SetCredit(credit);
        }
    }
    int credit = 0;
    public float TimeLeftSecs
    {
        get => timeLeftSecs;
        set
        {
            timeLeftSecs = value;
            GameCanvas.instance.SetTimeLeft(timeLeftSecs);
        }
    }

    public int WaveCnt { get; set; } = 0;

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

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.Start;
        timeLeftSecs = timeLimitSecs;
        Score = 0;
        GameCanvas.instance.SetTimeLeft(timeLeftSecs);
        GameCanvas.instance.SetTitle("Before the game starts...");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.Playing) {
            timeLeftSecs -= Time.deltaTime;
            GameCanvas.instance.SetTimeLeft(timeLeftSecs);
            if (timeLeftSecs <= 0.0f) {
                ++WaveCnt;
                if (WaveCnt <= MaxWaveCnt)
                {
                    gameState = GameState.Intermission;
                }
                else
                {
                    gameState = GameState.GameOver;
                }
            }
        }
    }
    bool Intermission_1 = false;
    void Intermission()
    {
        TimeLeftSecs = timeLimitSecs;
        Score = 0;

        IEnumerator _1()
        {
            Intermission_1 = true;
            float loadingTime = 3.0f;
            float t = 0.0f;
            while (t < loadingTime)
            {
                float progress = t / loadingTime;
                GameCanvas.instance.SetTitle($"Next Round Loading...{Mathf.FloorToInt(progress * 100)}%");
                yield return new WaitForSecondsRealtime(0.1f);
                t += 0.1f;
            }
            GameCanvas.instance.SetTitle($"Hit the Button When Ready!\nWave: {WaveCnt}/{MaxWaveCnt}\n" +
                $"Score: {Score}\n" +
                $"You may spend your credit in the shop now => ");
            Intermission_1 = false;
        }
        StartCoroutine(_1());
    }

    bool GameOver_0 = false;
    void GameOver() {
        var highScore = PlayerPrefs.GetInt("Score");
        highScore = Mathf.Max(highScore, Score);
        PlayerPrefs.SetInt("Score", highScore);

        GameCanvas.instance.SetTitle($"Congrats! Game Over!\nScore{Score}\nHigh Score{highScore}");
        TimeLeftSecs = timeLimitSecs;
        Score = 0;
        Credit = 0;

        IEnumerator _0() {
            GameOver_0 = true;
            yield return new WaitForSecondsRealtime(10.0f);
            gameState = GameState.Start;
            GameOver_0 = false;
        }
        StartCoroutine(_0());
    }

    public void StartGame()
    {
        if (Intermission_1 || GameOver_0)
            return;

        if (gameState != GameState.Playing) {
            gameState = GameState.Playing;
        }
    }
}
