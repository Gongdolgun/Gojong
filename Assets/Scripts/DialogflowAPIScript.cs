using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using JsonData;
using System.IO;
using System.Text;
public class DialogflowAPIScript : MonoBehaviour
{
    public UnityEngine.UI.InputField input;
    public UnityEngine.UI.Text output;
    public string key = "";
    public go_controller gc;
    // Use this for initialization

    public void SetKey()
    {
        //System.Diagnostics.Process.Start("C:/Users/lkiop/AppData/Local/Google/Cloud SDK/google-cloud-sdk/bin/aa.bat");
        // Start the child process.
        //키파일을 텍스트에서 불러와서 쓰려고 테스트한 부분
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        // Redirect the output stream of the child process.
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.RedirectStandardOutput = true;
        
        p.StartInfo.FileName = "e:/aa.bat";
        p.Start();
        // Do not wait for the child process to exit before
        // reading to the end of its redirected stream.
        // p.WaitForExit();
        // Read the output stream first and then wait.
        string output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        StringBuilder sb = new StringBuilder();
        sb.Append(output );
        Debug.Log(sb);
        key = output;
        string[] words = output.Split(' ');
        key = words[5].Trim();
    }
    IEnumerator Start()
    {
        //System.Diagnostics.Process.Start("C:/Users/lkiop/AppData/Local/Google/Cloud SDK/google-cloud-sdk/bin/aa.bat");
        //string path = "e:/test.txt";
        
        //Read the text from directly from the test.txt file
        //string  read = File.ReadAllText(path);
        yield return new WaitForSeconds(3);
        key = "ya29.c.Kp0B6AfB-sjVomJTRJiY5ETLGZ3_FNYBViFyPq4bWPu2kmg1fhnHLb035pwEQsi01aX29I1_JxVlp4HNI8MEuWz6Wqe8jj3mpy4RSmioBLu77G0mj5vHVEBqoDhArLSydX5TcTPxEtG9F_n8GAo9nA-DDlij2uYr8yu0ln7FeSvHBcdF1BtOzKQgwk5Fs-xl7kEVnIwbu2wyFBN9KKoanA";
        //key = read.Trim();
        //key = read ;
        //Debug.Log(key);
        //StartCoroutine(PostRequest("https://dialogflow.clients6.google.com/v2/projects/gojong/agent/sessions/c7f0adcc-79df-76af-b102-6330ae2fa42f:detectIntent", key));
    }

    
    public void SendQuestion()
    {
        string aa = input.text;
        //string key = "ya29.c.Kp0B5wfEcqjzTiHJlWpF0cg9N8znnRiRxD3661832B8pCqISmcGam50Bs9ukLKINJBrMpCLxoHfjuVdd2bJq16H7Cix0LyygwhRRKGdeRVcu-vqewkVd1VyTAPau8ASQfrttRFKH-bpSpdAHkBep7l-a6kQJ2hTE2CIfeac4TrDzWqoecHRcAWBi5PkPRHHjHM4OKXmIktDcUj15PY0utg";
        //주소 확인 필요, dialogflow intent에서 오른쪽 테스트하는 부분에서 확인 가능
        StartCoroutine(PostRequest2("https://dialogflow.clients6.google.com/v2/projects/gojong/agent/sessions/c7f0adcc-79df-76af-b102-6330ae2fa42f:detectIntent", key, aa));
    }
    IEnumerator PostRequest2(string url, string AccessToken, string _aa)
    {
        UnityWebRequest postRequest = new UnityWebRequest(url, "POST");
        RequestBody requestBody = new RequestBody();
        requestBody.queryInput = new QueryInput();
         

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes("{\"queryInput\":{\"text\":{\"text\":\"" + _aa + "\",\"languageCode\":\"ko\"}},\"queryParams\":{\"source\":\"DIALOGFLOW_CONSOLE\",\"timeZone\":\"Asia/Seoul\"}}");
        //Debug.Log(bodyRaw);
        postRequest.SetRequestHeader("Authorization", "Bearer " + AccessToken);
        postRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        postRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        //postRequest.SetRequestHeader("Content-Type", "application/json\; charset=utf-8");

        yield return postRequest.SendWebRequest();

        if (postRequest.isNetworkError || postRequest.isHttpError)
        {
            Debug.Log(postRequest.responseCode);
            Debug.Log(postRequest.error);
        }
        else
        {
            // Show results as text
            Debug.Log("Response: " + postRequest.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] resultbyte = postRequest.downloadHandler.data;
            string result = System.Text.Encoding.UTF8.GetString(resultbyte);
            ResponseBody content = (ResponseBody)JsonUtility.FromJson<ResponseBody>(result);
            Debug.Log(content.queryResult.fulfillmentText);
            gc.PlayAnswer(int.Parse(content.queryResult.fulfillmentText));
            output.text = content.queryResult.fulfillmentText;
        }
    }

    IEnumerator PostRequest(String url, String AccessToken)
    {
        UnityWebRequest postRequest = new UnityWebRequest(url, "POST");
        RequestBody requestBody = new RequestBody();
        requestBody.queryInput = new QueryInput();
        

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes("{\"queryInput\":{\"text\":{\"text\":\"조선의 몇번째 왕인가요\",\"languageCode\":\"ko\"}},\"queryParams\":{\"source\":\"DIALOGFLOW_CONSOLE\",\"timeZone\":\"Asia/Seoul\"}}");
        //Debug.Log(bodyRaw);
        postRequest.SetRequestHeader("Authorization", "Bearer " + AccessToken);
        postRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        postRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        //postRequest.SetRequestHeader("Content-Type", "application/json\; charset=utf-8");

        yield return postRequest.SendWebRequest();

        if (postRequest.isNetworkError || postRequest.isHttpError)
        {
            Debug.Log(postRequest.responseCode);
            Debug.Log(postRequest.error);
        }
        else
        {
            // Show results as text
            Debug.Log("Response: " + postRequest.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] resultbyte = postRequest.downloadHandler.data;
            string result = System.Text.Encoding.UTF8.GetString(resultbyte);
            ResponseBody content = (ResponseBody)JsonUtility.FromJson<ResponseBody>(result);
            Debug.Log(content.queryResult.fulfillmentText);
        }
    }

    IEnumerator GetAgent(String AccessToken)
    {
        UnityWebRequest www = UnityWebRequest.Get("https://dialogflow.clients6.google.com/v2/projects/gojong/agent/sessions/c7f0adcc-79df-76af-b102-6330ae2fa42f:detectIntent");

        www.SetRequestHeader("Authorization", "Bearer " + AccessToken);

        yield return www.SendWebRequest();
        //myHttpWebRequest.PreAuthenticate = true;
        //myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
        //myHttpWebRequest.Accept = "application/json";

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
}
