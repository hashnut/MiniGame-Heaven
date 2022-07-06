using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public bool isGameover = false; 
    public Text scoreText; 
    public Text highScoreText;
    public GameObject gameoverUI;

    private int score = 0;
    private int highScore = 0;

    void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("GameManager instance cannot not exceed 1");
            Destroy(gameObject);
        }

        highScore = PlayerPrefs.GetInt("highScore", highScore);
    }

    void Update() 
    {
        if (isGameover && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (score >= highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);
        }
        highScoreText.text = "HighScore : " + highScore;
    }


    // 점수를 증가시키는 메서드
    public void AddScore(int newScore) 
    {
        score += newScore;
        scoreText.text = "Score : " + score;


    }

    // 플레이어 캐릭터가 사망시 게임 오버를 실행하는 메서드
    public void OnPlayerDead() 
    {
        isGameover = true;
        gameoverUI.SetActive(true);
    }
}