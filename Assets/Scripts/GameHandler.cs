using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    
    public bool gameIsOn = true;
    public GameObject catPrefab;
    public GameObject fish;
    private int score;
    public GameObject scoreText;
    public GameObject playButton;
    public GameObject scoreButton;
    public GameObject backButton;
    public GameObject scorePanel;
    public GameObject submitScoreButton;
    public GameObject scoreInputField;

    public GameObject canvas;

    public GameObject scoreRowPrefab;

    public float catSpeed = 2f;
    public float spawnTime = 1f;
    public float difficultyIncreaseTime = 0.1f;
    private float x = 0;

    void Start()
    {
        refreshScoreText();
        playButton.GetComponent<Button>().onClick.AddListener(StartGame);
        playButton.SetActive(true);
        
        scoreButton.GetComponent<Button>().onClick.AddListener(ShowScore);
        scoreButton.SetActive(true);
        
        scorePanel.SetActive(false);
        submitScoreButton.GetComponent<Button>().onClick.AddListener(SubmitScore);
        
        backButton.GetComponent<Button>().onClick.AddListener(RestartGame);
        backButton.SetActive(false);
    }

    public void StartGame()
    {
        StartCoroutine(SpawnCatsCoroutine());
        InvokeRepeating("increaseDifficulty", 0, difficultyIncreaseTime);
        playButton.SetActive(false);
        scoreButton.SetActive(false);
        scoreText.SetActive(true);

    }

    public void ShowScore()
    {
        playButton.SetActive(false);
        backButton.SetActive(true);
        scoreButton.SetActive(false);
        
        NetworkHandler networkHandler = new NetworkHandler();
        StartCoroutine(networkHandler.GetScore(this));
    }

    public void CreateScoreTable(Score[] scores)
    {
        int i = Screen.height - 50;
        foreach (Score score in scores)
        {
            GameObject instantiate = Instantiate(scoreRowPrefab, new Vector3(Screen.width/2,i,0), Quaternion.identity);
            instantiate.transform.SetParent(canvas.transform);
            ScoreRowHandler srh = instantiate.GetComponent<ScoreRowHandler>();
            srh.SetName(score.name);
            srh.SetScore(score.score.ToString());
            i -= 50;
        }

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Defeat()
    {
        gameIsOn = false;
        scorePanel.SetActive(true);
        
    }

    private void SubmitScore()
    {
        NetworkHandler networkHandler = new NetworkHandler();
        string playerName = scoreInputField.GetComponent<InputField>().text;
        StartCoroutine(networkHandler.UploadScore(playerName,score));
        backButton.SetActive(true);
        scorePanel.SetActive(false);
    }
    
    private Vector3 GetRandomVect()
    {
        if (Camera.main == null) return Vector3.zero;
        Vector3 stageDimension = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        int delta = Random.Range(2,8);
        Vector2 topRight = new Vector2(stageDimension.x + delta, stageDimension.y + delta);
        Vector2 topLeft = new Vector2(-stageDimension.x - delta, stageDimension.y + delta);
        Vector2 bottomRight = new Vector2(stageDimension.x + delta, -stageDimension.y - delta);
        Vector2 bottomLeft = new Vector2(-stageDimension.x - delta, -stageDimension.y - delta);

        Vector3 spawnPoint = Vector3.zero;
        int zoneDecision = Random.Range(0, 4);
        
        switch (zoneDecision)
        {
            case 0:
                spawnPoint = new Vector3(topLeft.x, Random.Range(topLeft.y, bottomLeft.y),0);
                break;
            case 1:
                spawnPoint = new Vector3(Random.Range(topLeft.x, topRight.x), topLeft.y,0);
                break;
            case 2:
                spawnPoint = new Vector3(topRight.x, Random.Range(topRight.y, bottomRight.y),0);
                break;
            case 3:
                spawnPoint = new Vector3(Random.Range(bottomLeft.x, bottomRight.x), bottomLeft.y,0);
                break;
        }
        return spawnPoint;
    }

    private void GenerateCat()
    {
        GameObject catGameObject = Instantiate(catPrefab, GetRandomVect(),Quaternion.identity);
        Cat cat = catGameObject.GetComponent<Cat>();
        cat.fish = fish;
        cat.speed = catSpeed;
        cat.gameHandler = this;
    }

    private IEnumerator SpawnCatsCoroutine()
    {
        while (gameIsOn)
        {
            GenerateCat();
            yield return new WaitForSeconds(spawnTime);         
        }
    }

    public void increaseScore(int addScore)
    {
        score += addScore;
        refreshScoreText();

    }

    private void refreshScoreText()
    {
        scoreText.GetComponent<Text>().text = "Score: " + score;
    }

    private void increaseDifficulty()
    {
        x += difficultyIncreaseTime;
        catSpeed = (1 / (1 + Mathf.Exp(-0.03f * x + 2.5f))) * 2.5f + 2;
        spawnTime = (1 / (1 + Mathf.Exp(-(-0.03f) * x - 3.3f))) * 0.7f + 0.3f;
    }
}
