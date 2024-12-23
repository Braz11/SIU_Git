using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public enum PhraseType
{
    Prompt,
    Trivia,
    IndivialTask,
}

[System.Serializable]
public class PhrasesData
{
    public PhraseType phraseType;
    public string phraseIdentifier;
    public string numberOfSips;
    public string phrase;
    public string awnser;
}
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
        StartCoroutine(GetSheetData(PhraseType.Trivia, triviaData, SHEET_URL_TRIVIA));
        StartCoroutine(GetSheetData(PhraseType.Prompt, promptsData, SHEET_URL_PROMPTS));
        StartCoroutine(GetSheetData(PhraseType.IndivialTask, tasksData, SHEET_URL_TASKS));
    }

    IEnumerator GetSheetData(PhraseType type, string dataUrl, string SHEET_URL)
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
        ParseCSV(type, dataUrl);
    }

    void ParseCSV(PhraseType type,  string csvData)
    {
        string[] rows = csvData.Split('\n');
        for(int i = 1; i < rows.Length; i++)
        {   
            string[] cells = rows[i].Split(',');
            PhrasesData phrasesData = new PhrasesData();
            phrasesData.phraseType = type;
            phrasesData.phrase = cells[0];
            phrasesData.phraseIdentifier = cells[1];
            phrasesData.awnser = cells[2];
            phrasesData.numberOfSips = cells[3];
            EventsManager.OnPhraseCreated?.Invoke(phrasesData);
        }
    }
}