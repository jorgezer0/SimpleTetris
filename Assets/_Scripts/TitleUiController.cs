using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUiController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Text highScore;
    
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        
        UpdateHighScore();
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void UpdateHighScore()
    {
        highScore.text = $"Best Score: {PlayerPrefs.GetInt("highScore", 0)}";
    }
}
