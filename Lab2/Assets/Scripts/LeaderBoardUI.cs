using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class LeaderBoardUI : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown mapDropdown;
    [SerializeField] private Transform scoreListParent; // Panel chứa các dòng điểm
    [SerializeField] private GameObject scoreItemPrefab; // Prefab 1 dòng điểm (Text hoặc custom)
    
    

    private List<string> mapNames = new List<string> { "1", "2", "3" };

    [System.Serializable]
    public class PlayerResult
    {
        public string playerName;
        public int score;
        public float distance;
    }

    [System.Serializable]
    public class PlayerResultList
    {
        public List<PlayerResult> results = new List<PlayerResult>();
    }

    void Start()
    {
        // Khởi tạo dropdown
        mapDropdown.ClearOptions();
        mapDropdown.AddOptions(mapNames);
        mapDropdown.onValueChanged.AddListener(OnDropdownChanged);

        // Hiển thị leaderboard map đầu tiên
        ShowLeaderBoard(mapNames[0]);

    }

    void OnDropdownChanged(int index)
    {
        ShowLeaderBoard(mapNames[index]);
    }


    //void Start()
    //{
    //    // Gắn sự kiện khi chọn một option từ dropdown
    //    mapDropdown.onValueChanged.AddListener(OnDropdownChanged);

    //    // Hiển thị leaderboard cho option đầu tiên khi khởi động
    //    ShowLeaderBoard(mapDropdown.options[0].text);
    //}

    //void OnDropdownChanged(int index)
    //{
    //    // Hiển thị leaderboard cho map được chọn
    //    ShowLeaderBoard(mapDropdown.options[index].text);
    //}


    void ShowLeaderBoard(string mapName)
    {
        foreach (Transform child in scoreListParent)
            Destroy(child.gameObject);

        string fileName = $"result_GameScreen{mapName}.json";
        string path = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(path))
        {
            Debug.Log($"Không có dữ liệu cho GameScreen{mapName}");
            return;
        }

        string json = File.ReadAllText(path);
        Debug.Log($"Đọc file: {path}\nNội dung: {json}");
        PlayerResultList resultList = JsonUtility.FromJson<PlayerResultList>(json);

        if (resultList == null || resultList.results == null)
        {
            Debug.LogError("Lỗi parse JSON hoặc không có dữ liệu!");
            return;
        }

        resultList.results.Sort((a, b) => b.score.CompareTo(a.score));

        // Hiện tối đa 5 người cao nhất
        int maxShow = Mathf.Min(5, resultList.results.Count);
        for (int i = 0; i < maxShow; i++)
        {
            var result = resultList.results[i];
            GameObject item = Instantiate(scoreItemPrefab, scoreListParent);
            var texts = item.GetComponentsInChildren<TMP_Text>();
            if (texts.Length >= 3)
            {
                texts[0].text = result.playerName;
                texts[1].text = result.score.ToString();
                texts[2].text = result.distance.ToString("0.0") + "m";
            }
        }
    }
}
