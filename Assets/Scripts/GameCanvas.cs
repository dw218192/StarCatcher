using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro

public class GameCanvas : MonoBehaviour
{
    public static GameCanvas instance;
    public GameObject playingUI, startUI;
    public TextMeshPro scoreText, creditText, timeLeftText;
    public TextMeshPro titleText;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        GameMgr.instance.OnGameStateChange += (oldState, newState) =>
        {
            if (newState == GameMgr.GameState.Start)
            {
                startUI.SetActive(true);
                playingUI.SetActive(false);
            }
            else if (newState == GameMgr.GameState.Intermission)
            {

                startUI.SetActive(false);
                playingUI.SetActive(false);
            }
            else if (newState == GameMgr.GameState.Playing)
            {
                SetTitle("Catch as many as you can!!");
                startUI.SetActive(false);
                playingUI.SetActive(true);
            }
            else if (newState == GameMgr.GameState.GameOver)
            {
                startUI.SetActive(false);
                playingUI.SetActive(false);
            }
        };
    }

    public void SetScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }
    public void SetCredit(int credit)
    {
        creditText.text = $"Credit: {credit}";
    }

    public void SetTimeLeft(float timeLeft)
    {
        timeLeft = Mathf.Max(0, timeLeft);
        timeLeftText.text = $"Time Left: {timeLeft:F1}";
    }

    public void SetTitle(string str) {
        titleText.text = str;
    }
}
