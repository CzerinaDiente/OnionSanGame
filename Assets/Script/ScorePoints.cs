using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScorePoints : MonoBehaviour
{
    public static ScorePoints scorePoints;
    
    [Header("UI Text")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalscoreText;
    public TextMeshProUGUI highscoreText;

    int score = 0;
    int highscore = 0;

    private void Awake(){ scorePoints = this; }

    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);

        scoreText.text = score.ToString() + " ";
        //finalscoreText.text = score.ToString();
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();
    }

    public void AddPoint()
    {
        score += 100;
        scoreText.text = score.ToString() + " ";

        if (highscore < score) { PlayerPrefs.SetInt("highscore", score); }
    }

    public void SubtractPoint(int amount)
    {
        score -= amount;

        // Optional: prevent negative score
        if (score < 0)
            score = 0;

        scoreText.text = score.ToString() + " ";
    }

    public void UpdateFinalScore()
    {
        finalscoreText.text = score.ToString();
    }
}