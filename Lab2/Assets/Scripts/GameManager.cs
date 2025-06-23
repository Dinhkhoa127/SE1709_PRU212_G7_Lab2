using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private Transform player;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI distanceText;

    // Private variables
    private Vector3 startPosition;
    private float currentDistance = 0f;
    private float currentScore = 0f;
    private bool gameStarted = false;

    // Singleton pattern
    public static GameManager Instance { get; private set; }

    [Header("Combo Settings")]
    [SerializeField] private TextMeshProUGUI comboText;
    private int comboCount = 0;

    void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Đăng ký callback cho khi scene mới được load
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Hủy đăng ký callback khi object bị destroy
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset game state khi chuyển scene
        ResetGameState();
        
        // Delay để đảm bảo scene đã load hoàn toàn
        StartCoroutine(InitializeAfterSceneLoad(scene));
    }

    private System.Collections.IEnumerator InitializeAfterSceneLoad(Scene scene)
    {
        // Chờ 1 frame để scene load hoàn toàn
        yield return null;
        
        // Tự động tìm lại UI references và player khi scene mới được load
        FindUIReferences();
        FindPlayerReference();
        
        // Chỉ khởi tạo game khi đang ở game scene
        if (IsGameScene(scene.name))
        {
            StartGame();
        }
    }

    private void ResetGameState()
    {
        // Reset tất cả game variables
        currentDistance = 0f;
        currentScore = 0f;
        gameStarted = false;
        comboCount = 0;
        
        // Hide combo UI chỉ trong game scenes
        string currentScene = SceneManager.GetActiveScene().name;
        if (IsGameScene(currentScene))
        {
            HideComboUI();
        }
        
        Debug.Log($"Game state reset for scene: {currentScene}");
    }

    private void FindUIReferences()
    {
        // Reset references trước khi tìm
        scoreText = null;
        distanceText = null;
        comboText = null;

        // Chỉ tìm UI trong game scenes
        string currentScene = SceneManager.GetActiveScene().name;
        if (!IsGameScene(currentScene))
        {
            Debug.Log($"Skipping UI search - not a game scene: {currentScene}");
            return;
        }

        Debug.Log("Starting UI search...");

        // Tìm tất cả TextMeshProUGUI components trong scene
        TextMeshProUGUI[] allTexts = FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);
        Debug.Log($"Found {allTexts.Length} TextMeshProUGUI components in scene");
        
        foreach (TextMeshProUGUI textComponent in allTexts)
        {
            // Xác định loại text dựa vào tên object hoặc nội dung text
            string objectName = textComponent.gameObject.name.ToLower();
            string textContent = textComponent.text.ToLower();
            
            Debug.Log($"Checking UI: {textComponent.gameObject.name} - Text: '{textComponent.text}'");
            
            // Tìm Score Text
            if (objectName.Contains("score") || textContent.Contains("score"))
            {
                scoreText = textComponent;
                Debug.Log($" Found Score Text: {textComponent.gameObject.name}");
            }
            // Tìm Distance Text
            else if (objectName.Contains("distance") || textContent.Contains("distance"))
            {
                distanceText = textComponent;
                Debug.Log($" Found Distance Text: {textComponent.gameObject.name}");
            }
            // Tìm Combo Text - Kiểm tra tên chính xác "Combo" hoặc text chứa "combo"
            else if (objectName == "combo" || objectName.Contains("combo") || textContent.Contains("combo") || objectName.Contains("multiplier"))
            {
                comboText = textComponent;
                Debug.Log($" Found Combo Text: {textComponent.gameObject.name}");
            }
        }

        Debug.Log($"UI References found - Score: {scoreText != null}, Distance: {distanceText != null}, Combo: {comboText != null}");
        
        // Nếu vẫn không tìm được, thử tìm theo parent hierarchy
        if (scoreText == null || distanceText == null || comboText == null)
        {
            FindUIByHierarchy();
        }

        // Test ComboText sau khi tìm kiếm
        TestComboText();
    }

    private void FindUIByHierarchy()
    {
        Debug.Log("Searching UI by hierarchy...");
        
        // Tìm Canvas chính
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        Debug.Log($"Found {canvases.Length} canvases");
        
        foreach (Canvas canvas in canvases)
        {
            Debug.Log($"Searching in canvas: {canvas.gameObject.name}");
            
            // Tìm trong tất cả children của Canvas
            TextMeshProUGUI[] textsInCanvas = canvas.GetComponentsInChildren<TextMeshProUGUI>(true); // Include inactive objects
            
            foreach (TextMeshProUGUI textComp in textsInCanvas)
            {
                string objName = textComp.gameObject.name.ToLower();
                string textContent = textComp.text.ToLower();
                
                if (scoreText == null && (objName.Contains("score") || textContent.Contains("score")))
                {
                    scoreText = textComp;
                    Debug.Log($" Found Score Text in hierarchy: {textComp.gameObject.name}");
                }
                if (distanceText == null && (objName.Contains("distance") || textContent.Contains("distance")))
                {
                    distanceText = textComp;
                    Debug.Log($" Found Distance Text in hierarchy: {textComp.gameObject.name}");
                }
                if (comboText == null && (objName == "combo" || objName.Contains("combo") || objName.Contains("multiplier") || textContent.Contains("combo")))
                {
                    comboText = textComp;
                    Debug.Log($" Found Combo Text in hierarchy: {textComp.gameObject.name}");
                }
            }
        }
        
        Debug.Log($"UI References after hierarchy search - Score: {scoreText != null}, Distance: {distanceText != null}, Combo: {comboText != null}");
        
        // Nếu vẫn không tìm được ComboText, thử tìm theo tên cụ thể
        if (comboText == null)
        {
            FindComboTextSpecifically();
        }
    }

    private void FindComboTextSpecifically()
    {
        // Tìm object tên chính xác "Combo"
        GameObject comboObj = GameObject.Find("Combo");
        if (comboObj != null)
        {
            TextMeshProUGUI comboComponent = comboObj.GetComponent<TextMeshProUGUI>();
            if (comboComponent != null)
            {
                comboText = comboComponent;
                Debug.Log($" Found Combo Text specifically: {comboObj.name}");
                return;
            }
        }

        // Tìm tất cả TextMeshProUGUI và kiểm tra parent/child hierarchy
        TextMeshProUGUI[] allTexts = FindObjectsByType<TextMeshProUGUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (TextMeshProUGUI text in allTexts)
        {
            // Kiểm tra nếu object hoặc parent có tên "Combo"
            Transform current = text.transform;
            while (current != null)
            {
                if (current.name.ToLower() == "combo")
                {
                    comboText = text;
                    Debug.Log($" Found Combo Text by parent hierarchy: {text.gameObject.name} (parent: {current.name})");
                    return;
                }
                current = current.parent;
            }
        }
        
        Debug.LogWarning("Combo Text still not found after specific search!");
    }

    private void TestComboText()
    {
        // Chỉ test trong game scenes
        string currentScene = SceneManager.GetActiveScene().name;
        if (!IsGameScene(currentScene))
        {
            return;
        }

        if (comboText != null)
        {
            Debug.Log($" Combo Text is ready: {comboText.gameObject.name}");
            Debug.Log($" Current text: '{comboText.text}'");
            Debug.Log($" GameObject active: {comboText.gameObject.activeInHierarchy}");
            Debug.Log($" Component enabled: {comboText.enabled}");
        }
        else
        {
            Debug.LogError(" Combo Text is NULL! Cannot show combo UI.");
        }
    }

    private void FindPlayerReference()
    {
        // Chỉ tìm player trong game scenes
        if (!IsGameScene(SceneManager.GetActiveScene().name))
        {
            player = null;
            return;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            Debug.Log($" Player reference found: {player.name}");
        }
        else
        {
            // Thử tìm theo tên
            GameObject playerByName = GameObject.Find("Player");
            if (playerByName != null)
            {
                player = playerByName.transform;
                Debug.Log($" Player found by name: {player.name}");
                Debug.LogWarning("Player object should be tagged with 'Player' tag for better performance!");
            }
            else
            {
                Debug.LogWarning("Player not found! Please ensure player exists and is tagged with 'Player' tag.");
            }
        }
    }

    private bool IsGameScene(string sceneName)
    {
        return sceneName == "GameScreen1" || sceneName == "GameScreen2" || sceneName == "GameScreen3";
    }

    void Start()
    {
        // Chỉ khởi tạo nếu đang ở game scene
        string currentScene = SceneManager.GetActiveScene().name;
        if (IsGameScene(currentScene))
        {
            // Tự động tìm player nếu chưa assign
            if (player == null)
            {
                FindPlayerReference();
            }
            
            // Tìm UI references
            FindUIReferences();
            
            StartGame();
        }
    }

    void Update()
    {
        if (gameStarted && player != null)
        {
            CalculateDistance();
            UpdateUI();
        }
    }

    public void StartGame()
    {
        if (player != null)
        {
            startPosition = player.position;
            currentDistance = 0f;
            currentScore = 0f;
            gameStarted = true;

            Debug.Log("Game Started! Start position: " + startPosition);
        }
        else
        {
            Debug.LogWarning("Player not found! Please assign player in GameManager or tag player with 'Player' tag.");
        }
    }

    private void CalculateDistance()
    {
        // Tính khoảng cách theo trục X (trượt ngang) - phù hợp với map trượt tuyết
        float distanceTraveled = Mathf.Abs(startPosition.x - player.position.x);

        // Cập nhật khoảng cách tối đa đã đi được
        if (distanceTraveled > currentDistance)
        {
            currentDistance = distanceTraveled;
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null && scoreText.gameObject != null)
        {
            scoreText.text = "Score: " + Mathf.RoundToInt(currentScore).ToString();
        }

        if (distanceText != null && distanceText.gameObject != null)
        {
            distanceText.text = "Distance: " + currentDistance.ToString("F1") + "m";
        }
    }
    // Public methods để các script khác có thể gọi
    public float GetCurrentScore()
    {
        return currentScore;
    }

    public float GetCurrentDistance()
    {
        return currentDistance;
    }

    // Phương thức để thêm điểm (dùng khi nhặt sao hoặc item)
    public void AddScore(float points)
    {
        currentScore += points;
        Debug.Log("Added " + points + " points. Current score: " + currentScore);
    }

    // Reset điểm về 0
    public void ResetScore()
    {
        currentScore = 0f;
    }

    // Gọi khi Player flip thành công
    public void AddFlipScore()
    {
        // Chỉ hoạt động trong game scenes
        if (!IsGameScene(SceneManager.GetActiveScene().name))
        {
            return;
        }

        comboCount++;

        int scoreToAdd = 10; // Điểm cơ bản

        // Từ lần thứ 2 trở đi → tính hệ số combo
        if (comboCount > 1)
        {
            scoreToAdd = 10 * comboCount;
            ShowComboUI(comboCount);
        }
        else
        {
            HideComboUI(); // Không hiển thị combo x1
        }

        currentScore += scoreToAdd;
        Debug.Log($"Flip combo x{comboCount}! +{scoreToAdd} points. Total: {currentScore}");
    }

    // Gọi khi chạm đất
    public void ResetCombo()
    {
        comboCount = 0;
        
        // Chỉ ẩn UI trong game scenes
        if (IsGameScene(SceneManager.GetActiveScene().name))
        {
            HideComboUI();
        }
    }
    private void ShowComboUI(int combo)
    {
        if (comboText != null && comboText.gameObject != null)
        {
            comboText.text = $"Combo x{combo}!";
            comboText.transform.localScale = Vector3.zero; // Scale trước khi bật
            comboText.gameObject.SetActive(true);

            comboText.transform.DOScale(1.5f, 0.3f)
                                .SetEase(Ease.OutBack)
                                .OnComplete(() => {
                                    if (comboText != null && comboText.gameObject != null)
                                        comboText.transform.DOScale(1f, 0.1f);
                                });

            Debug.Log("Combo UI show with tween!");
        }
    }

    private void HideComboUI()
    {
        if (comboText != null && comboText.gameObject != null)
        {
            comboText.gameObject.SetActive(false);
        }
    }

    // Trước khi load scene GameOver
    public void SaveGameOverData()
    {
        Debug.Log("SaveGameOverData called! Score: " + GetCurrentScore() + ", Distance: " + GetCurrentDistance());
        PlayerPrefs.SetFloat("LastScore", GetCurrentScore());
        PlayerPrefs.SetFloat("LastDistance", GetCurrentDistance());
        PlayerPrefs.Save();
    }
}

