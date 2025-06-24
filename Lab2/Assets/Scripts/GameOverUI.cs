using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // Thêm dòng này ở đầu file
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TextMeshProUGUI recordText;
    [Header("Star UI")]
    [SerializeField] private Image star1;
    [SerializeField] private Image star2;
    [SerializeField] private Image star3;
    [SerializeField] private Sprite starBlackSprite;
    [SerializeField] private Sprite starGoldSprite;

    [System.Serializable]
    public class PlayerResult
    {
        public string playerName;
        public int score;
        public float distance;
    }

    void Start()
    {
        int lastScore = Mathf.RoundToInt(PlayerPrefs.GetFloat("LastScore", 0));
        if (scoreText != null)
            scoreText.text = lastScore.ToString();

        if (distanceText != null)
            distanceText.text = PlayerPrefs.GetFloat("LastDistance", 0).ToString("F1") + "m";

        if (nameInputField != null)
        {
            string playerName = PlayerPrefs.GetString("PlayerName", "");
            nameInputField.text = playerName;
            nameInputField.interactable = true;
        }
        UpdateStars(lastScore);
        DisplayHighestScore();
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
    private void DisplayHighestScore()
    {
        string mapName = PlayerPrefs.GetString("LastMapName", "UnknownMap");
        string fileName = $"result_{mapName}.json";
        string path = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerResultList resultList = JsonUtility.FromJson<PlayerResultList>(json);
            if (resultList != null && resultList.results.Count > 0)
            {
                // Khởi tạo điểm cao nhất là điểm đầu tiên
                PlayerResult highestScoreResult = resultList.results[0];

                // Duyệt qua các kết quả để tìm điểm cao nhất
                foreach (PlayerResult result in resultList.results)
                {
                    if (result.score > highestScoreResult.score)
                    {
                        highestScoreResult = result;
                    }
                }

                // Hiển thị điểm cao nhất lên phần recordText
                if (recordText != null)
                {
                    recordText.text = highestScoreResult.score.ToString(); // Chỉ hiển thị số điểm
                }
            }
        }
        else
        {
            if (recordText != null)
            {
                recordText.text = "0"; // Nếu không có dữ liệu, hiển thị 0
            }
        }
    }

    private void UpdateStars(int score)
    {
        int starCount = 0;
        if (score >= 50) starCount = 1;
        if (score >= 120) starCount = 2;
        if (score >= 200) starCount = 3;

        // Đặt sprite cho từng sao
        star1.sprite = starCount >= 1 ? starGoldSprite : starBlackSprite;
        star2.sprite = starCount >= 2 ? starGoldSprite : starBlackSprite;
        star3.sprite = starCount >= 3 ? starGoldSprite : starBlackSprite;
    }
}

[System.Serializable]
public class PlayerResultList
{
    public List<GameOverUI.PlayerResult> results = new List<GameOverUI.PlayerResult>(); // Updated type to match GameOverUI.PlayerResult  
}
