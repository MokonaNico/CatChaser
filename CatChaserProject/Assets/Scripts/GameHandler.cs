using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public bool gameIsOn = true;
    public GameObject catPrefab;
    public GameObject fish;
    public int spawnTime = 2;
    
    
    void Start()
    {
        StartCoroutine(SpawnCatsCoroutine(spawnTime));
    }
    
    public void Defeat()
    {
        gameIsOn = false;
    }

    private Vector3 GetRandomVect()
    {
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
        cat.gameHandler = this;
    }

    private IEnumerator SpawnCatsCoroutine(int waitTime)
    {
        while (gameIsOn)
        {
            GenerateCat();
            yield return new WaitForSeconds(waitTime);         
        }
    }
}
