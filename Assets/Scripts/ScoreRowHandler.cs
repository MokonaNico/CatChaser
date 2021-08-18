using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRowHandler : MonoBehaviour
{
    public GameObject name;
    public GameObject score;

    public void SetName(string n)
    {
        name.GetComponent<Text>().text = n;
    }

    public void SetScore(string s)
    {
        score.GetComponent<Text>().text = s;
    }
}
