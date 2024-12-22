using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetsReader : MonoBehaviour
{
    private const string SHEET_URL_TRIVIA = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRs1ccJqJo2x7veftoNM2CR2NzcmTwF5fik56T1vkdgWpT5cpKpUvVe5org7IXayNmTUHiOmJxxE7s6/pub?gid=0&single=true&output=csv";
    private const string SHEET_URL_PROMPTS = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRs1ccJqJo2x7veftoNM2CR2NzcmTwF5fik56T1vkdgWpT5cpKpUvVe5org7IXayNmTUHiOmJxxE7s6/pub?gid=89175097&single=true&output=csv";
    private const string SHEET_URL_TASKS = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRs1ccJqJo2x7veftoNM2CR2NzcmTwF5fik56T1vkdgWpT5cpKpUvVe5org7IXayNmTUHiOmJxxE7s6/pub?gid=321474469&single=true&output=csv";
    
    private string triviaData;
    private string promptsData;
    private string tasksData;

    void Start()
    {
        StartCoroutine(GetSheetData(triviaData, SHEET_URL_TRIVIA));
        StartCoroutine(GetSheetData(promptsData, SHEET_URL_PROMPTS));
        StartCoroutine(GetSheetData(tasksData, SHEET_URL_TASKS));
    }

    IEnumerator GetSheetData(string dataUrl, string SHEET_URL)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(SHEET_URL))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                dataUrl = www.downloadHandler.text;
            }
            else
            {
                Debug.LogError("Failed to fetch Google Sheet: " + www.error);
            }
        }

        Debug.Log("=======================");
        ParseCSV(dataUrl);
    }

    void ParseCSV(string csvData)
    {
        string[] rows = csvData.Split('\n');
        foreach (string row in rows)
        {
            string[] cells = row.Split(',');
            Debug.Log(string.Join(" | ", cells));
        }
    }
}