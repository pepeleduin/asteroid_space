using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highScoreEntryTransformList;

    private void Awake()
    {
        entryContainer = transform.Find("HighScoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        highScoreEntryTransformList = new List<Transform>();

        // Load existing high scores
        string jsonString = PlayerPrefs.GetString("highScoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

        // Add the latest score from the ScoreManager
        if (ScoreManager.Instance != null)
        {
            Debug.Log("Score Manager Instance: " + ScoreManager.Instance.LatestScore);
            AddHighScoreEntry(ScoreManager.Instance.LatestScore, ScoreManager.Instance.PlayerName);
        }
        else {
            AddHighScoreEntry(0, "CPU");
        }
    }

    private void CreateHighScoreEntryTransform(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 30f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }
        entryTransform.Find("PosText").GetComponent<Text>().text = rankString;

        int score = highScoreEntry.score;
        entryTransform.Find("ScoreText").GetComponent<Text>().text = score.ToString();

        string name = highScoreEntry.name;
        entryTransform.Find("NameText").GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
    }

    public void RefreshHighScoreTable()
    {
        // Ensure the list is initialized
        if (highScoreEntryTransformList == null)
        {
            highScoreEntryTransformList = new List<Transform>();
        }

        // Clear existing entries
        foreach (Transform entryTransform in highScoreEntryTransformList)
        {
            Destroy(entryTransform.gameObject);
        }
        highScoreEntryTransformList.Clear();

        // Load and display updated high scores
        string jsonString = PlayerPrefs.GetString("highScoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

        // Check if highScores is null or empty
        if (highScores == null || highScores.highScoreEntryList == null)
        {
            return;
        }

        highScores.highScoreEntryList.Sort((x, y) => y.score.CompareTo(x.score));

        //highScoreEntryTransformList = new List<Transform>();
        foreach (HighScoreEntry highScoreEntry in highScores.highScoreEntryList)
        {
            CreateHighScoreEntryTransform(highScoreEntry, entryContainer, highScoreEntryTransformList);
        }   
    }

    public void AddHighScoreEntry(int score, string name)
    {
        // Load saved HighScores
        string jsonString = PlayerPrefs.GetString("highScoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

        if (highScores == null || highScores.highScoreEntryList == null)
        {
            highScores = new HighScores { highScoreEntryList = new List<HighScoreEntry>() };
        }

        // Add new entry to HighScores
        highScores.highScoreEntryList.Add(new HighScoreEntry { score = score, name = name.ToUpper() });

        // Sort the list by score in descending order
        highScores.highScoreEntryList.Sort((x, y) => y.score.CompareTo(x.score));

        // Keep only the top 10 scores
        if (highScores.highScoreEntryList.Count > 10)
        {
            highScores.highScoreEntryList.RemoveRange(10, highScores.highScoreEntryList.Count - 10);
        }

        // Save Updated HighScores
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("highScoreTable", json);
        PlayerPrefs.Save();

        //Update the table
        RefreshHighScoreTable();
    }

    private class HighScores
    {
        public List<HighScoreEntry> highScoreEntryList;
    }

    //Represents a single High Score Entry
    [System.Serializable]
    private class HighScoreEntry
    {
        public int score;
        public string name;
    }
    
}
