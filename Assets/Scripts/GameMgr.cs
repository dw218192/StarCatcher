using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
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
        
    }

}
