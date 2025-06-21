using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // Thêm dòng này ở đầu file

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TMP_InputField nameInputField;

    [System.Serializable]
    public class PlayerResult
    {
        public string playerName;
        public int score;
        public float distance;
    }

    void Start()
    {
        if (scoreText != null)
            scoreText.text = Mathf.RoundToInt(PlayerPrefs.GetFloat("LastScore", 0)).ToString();

        if (distanceText != null)
            distanceText.text = PlayerPrefs.GetFloat("LastDistance", 0).ToString("F1") + "m";

        if (nameInputField != null)
        {
            string playerName = PlayerPrefs.GetString("PlayerName", "");
            nameInputField.text = playerName;
            nameInputField.interactable = true;
        }
    }

    public void OnNameInputEndEdit()
    {
        Debug.Log("OnNameInputEndEdit được gọi!"); // Log khi nhấn Enter hoặc kết thúc nhập
        Debug.Log($"OnNameInputEndEdit: {nameInputField.text}");

        if (nameInputField != null && !string.IsNullOrWhiteSpace(nameInputField.text))
        {
            PlayerPrefs.SetString("PlayerName", nameInputField.text);
            PlayerPrefs.Save();

            SaveResultToJson(nameInputField.text);
        }
    }

    private void SaveResultToJson(string playerName)
    {
        PlayerResult result = new PlayerResult
        {
            playerName = playerName,
            score = Mathf.RoundToInt(PlayerPrefs.GetFloat("LastScore", 0)),
            distance = PlayerPrefs.GetFloat("LastDistance", 0)
        };

        // Lấy tên map vừa chơi từ PlayerPrefs
        string mapName = PlayerPrefs.GetString("LastMapName", "UnknownMap");
        string fileName = $"result_{mapName}.json";
        string path = Path.Combine(Application.persistentDataPath, fileName);

        PlayerResultList resultList = new PlayerResultList();
        if (File.Exists(path))
        {
            string oldJson = File.ReadAllText(path);
            resultList = JsonUtility.FromJson<PlayerResultList>(oldJson) ?? new PlayerResultList();
        }
        resultList.results.Add(result);

        string json = JsonUtility.ToJson(resultList, true);
        File.WriteAllText(path, json);

        Debug.Log($"✅ Đã lưu kết quả cho {playerName} vào file: {path}");
    }
}

[System.Serializable]
public class PlayerResultList
{
    public List<GameOverUI.PlayerResult> results = new List<GameOverUI.PlayerResult>(); // Updated type to match GameOverUI.PlayerResult  
}
