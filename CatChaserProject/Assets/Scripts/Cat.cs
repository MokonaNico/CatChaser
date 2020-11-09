using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    
    public float speed = 10;
    public GameObject fish;
    public GameHandler gameHandler;
    public int value = 10;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private bool clicked = false;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        endPosition = fish.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameHandler.gameIsOn)
            return;
        
        if (!clicked)
        {
            if (transform.position != endPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            }

            else
            {
                Debug.Log("Noob");
                gameHandler.Defeat();
            }
        }
        else{
            transform.position = Vector3.MoveTowards(transform.position, startPosition, (speed + 2) * Time.deltaTime);
            if (transform.position == startPosition)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnMouseDown()
    {
        if (!clicked && gameHandler.gameIsOn)
        {
            clicked = true;
            gameHandler.increaseScore(value);
            Debug.Log("Miaou");
        }
        
    }
}
