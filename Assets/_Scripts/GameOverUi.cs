using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUi : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    
    [SerializeField] private Text scoreText;
    [SerializeField] private Text bestScoreText;

    [SerializeField] private Button backButton;
    
    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        
        backButton.onClick.AddListener(BackToTitle);
        
        ToggleUi(false);

        EventBus.OnShowGameOverUi += OnShowGameOverUi;
    }

    private void OnDestroy()
    {
        EventBus.OnShowGameOverUi -= OnShowGameOverUi;
    }

    private void OnShowGameOverUi(object sender, EventArgs e)
    {
        var arg = (EventBus.ScoreArgs) e;
        UpdateTexts(arg.combo);
        ToggleUi(true);
    }

    private void UpdateTexts(int score)
    {
        scoreText.text = $"Score: {score}";
        bestScoreText.text = $"Best: {PlayerPrefs.GetInt("highScore")}";
    }

    private void ToggleUi(bool value)
    {
        _canvasGroup.alpha = value ? 1 : 0;
        _canvasGroup.blocksRaycasts = value;
    }

    private void BackToTitle()
    {
        SceneManager.LoadScene(0);
    }
}
