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

    public List<AudioClip> sounds;

    private AudioSource _audioSource;

    void Start()
    {
        startPosition = transform.position;
        endPosition = fish.transform.position;
        _audioSource = this.GetComponent<AudioSource>();
    }


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
            System.Random rand = new System.Random();  
            int index = rand.Next(sounds.Count);
            float val = (float) (rand.NextDouble() * (1.0 - 0.9) + 0.9);
            _audioSource.clip = sounds[index];
            _audioSource.pitch = val;
            _audioSource.Play();
        }
        
    }
}
