using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerMovement player;
    [SerializeField] Slider hpBar;
    [SerializeField] Text scoreText, gameOverScoreText, gameOverHighScoreText;
    [SerializeField] GameObject gameOverHud;
    float score, highScore;

    private void Awake()
    {
        Time.timeScale = 1;
        score = 0;
        highScore = PlayerPrefs.GetFloat("Highscore", -1);
        scoreText.text = "Score : " + score;
    }

    private void Update()
    {
        hpBar.value = player.GetHP();    
    }

    public void AddScore(float points)
    {
        score += points;
        scoreText.text = "Score : " + score;
    }

    public void GameOver()
    {
        gameOverHud.SetActive(true);
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat("Highscore", score);
        }
        gameOverScoreText.text = "Score : " + score;
        gameOverHighScoreText.text = "High score : " + highScore;
        Time.timeScale = 0;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
