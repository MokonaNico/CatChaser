using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkHandler
{
    private string url = "https://projetinfo.alwaysdata.net/CatChaserAPI/scores";
    
    
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

    private string transformPlayerInfoToJSON(string playerName, int playerScore)
    {
        string jsonString = "{ \"name\":\"" + playerName + "\", \"score\": " + playerScore.ToString() + " }";
        return jsonString;
    }
}
