using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkHandler
{
    private string url = "mokonanico.alwaysdata.net";
    
    public IEnumerator UploadScore(string playerName, int playerScore)
    {
        string logindataJsonString = transformPlayerInfoToJSON(playerName, playerScore);
        UnityWebRequest request = new UnityWebRequest (url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(logindataJsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (request.error != null)
        {
            Debug.Log("Status Code: " + request.responseCode);
            Debug.Log("Error: " + request.error);
        }
    }

    public IEnumerator GetScore(GameHandler gameHandler)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                gameHandler.CreateScoreTable(JsonHelper.getJsonArray<Score>(webRequest.downloadHandler.text));
            } else if (webRequest.error != null)
            {
                Debug.Log("Status Code: " + webRequest.responseCode);
                Debug.Log("Error: " + webRequest.error);
            }
        }

    }

    private string transformPlayerInfoToJSON(string playerName, int playerScore)
    {
        return "{ \"name\":\"" + playerName + "\", \"score\": " + playerScore.ToString() + " }";
    }
}
