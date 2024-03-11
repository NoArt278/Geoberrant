using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerMovement player;
    [SerializeField] Slider hpBar;
    [SerializeField] Text scoreText;
    float score;

    private void Awake()
    {
        score = 0;
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
}
