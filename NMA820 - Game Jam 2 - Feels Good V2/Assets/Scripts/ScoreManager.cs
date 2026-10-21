using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI scoreText;
    public int score = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        scoreText.text = "SCORE: " + score.ToString();
    }

    public void AddPoint()
    {
        score += 1;
        scoreText.text = "SCORE: " + score.ToString();
    }
}
