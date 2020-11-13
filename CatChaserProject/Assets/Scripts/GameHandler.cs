using System.Collections;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    
    private static readonly HttpClient client = new HttpClient();
    
    public bool gameIsOn = true;
    public GameObject catPrefab;
    public GameObject fish;
    private int score;
    public Text scoreText;
    public GameObject playButton;
    public GameObject restartButton;
    public GameObject scorePanel;
    public GameObject submitScoreButton;
    public GameObject scoreInputField;
    

    public float catSpeed = 1f;
    public float spawnTime = 1f;

    public float increaseSpeed = 0.1f;
    public float increaseSpawnRate = 0.05f;

    public float timeIncreaseCatSpeed = 5.0f;
    public float timeIncreaseCatSpawnRate = 5.0f;

    void Start()
    {
        refreshScoreText();
        playButton.GetComponent<Button>().onClick.AddListener(StartGame);
        playButton.SetActive(true);
        
        scorePanel.SetActive(false);
        submitScoreButton.GetComponent<Button>().onClick.AddListener(SubmitScore);
        
        restartButton.GetComponent<Button>().onClick.AddListener(RestartGame);
        restartButton.SetActive(false);
    }

    public void StartGame()
    {
        StartCoroutine(SpawnCatsCoroutine());
        InvokeRepeating("increaseCatSpeed", timeIncreaseCatSpeed, timeIncreaseCatSpeed);
        InvokeRepeating("increaseCatSpawnRate", timeIncreaseCatSpawnRate, timeIncreaseCatSpawnRate);
        playButton.SetActive(false);
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

    private async void SubmitScore()
    {
        string playerName = scoreInputField.GetComponent<InputField>().text;
        string playerScore = score.ToString();
        Debug.Log(playerName + " " + playerScore);
        string jsonString = "{ \"name\":\"" + playerName + "\", \"score\": " + playerScore + " }";
        Debug.Log(jsonString);
        StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        string url = "https://projetinfo.alwaysdata.net/CatChaserAPI/scores";
        HttpResponseMessage response = await client.PostAsync(url, content);
        Debug.Log(response.StatusCode);
        restartButton.SetActive(true);
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
        scoreText.text = "Score: " + score;
    }

    private void increaseCatSpeed()
    {
        catSpeed += increaseSpeed;
    }

    private void increaseCatSpawnRate()
    {
        spawnTime -= increaseSpawnRate;
    }
}
