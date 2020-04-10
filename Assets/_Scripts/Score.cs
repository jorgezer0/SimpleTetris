using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMesh scoreText;
    [SerializeField] private TextMesh levelText;
    [SerializeField] private int score;
    [SerializeField] private int level = 1;
    
    // Start is called before the first frame update
    private void Start()
    {
        levelText.text = level.ToString();
        
        EventBus.OnAddPoints += OnAddPoints;
        EventBus.OnGameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        EventBus.OnAddPoints -= OnAddPoints;
        EventBus.OnGameOver -= OnGameOver;
    }

    private void OnGameOver(object sender, EventArgs e)
    {
        if (score > PlayerPrefs.GetInt("highScore", 0))
            PlayerPrefs.SetInt("highScore", score);
        
        EventBus.RaiseShowGameOverUi(this, score);
    }

    private void OnAddPoints(object sender, EventArgs e)
    {
        var arg = (EventBus.ScoreArgs) e;

        var tempScore =  100 * arg.combo;
        var comboBonus = (arg.combo > 1) ? 25 * arg.combo * (arg.combo-1) : 0;
        
        score += tempScore + comboBonus;
        scoreText.text = score.ToString();

        var tempLevel = Mathf.FloorToInt(score / 1000);
        if (tempLevel >= level)
        {
            EventBus.RaiseLevelUp(this);
            level++;
            levelText.text = level.ToString();
        }
    }

}
