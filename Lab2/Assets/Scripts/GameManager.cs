using UnityEngine;
using TMPro;
using DG.Tweening;

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
    }

    void Start()
    {
        // Tự động tìm player nếu chưa assign
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        // Khởi tạo game
        StartGame();
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
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Mathf.RoundToInt(currentScore).ToString();
        }

        if (distanceText != null)
        {
            distanceText.text = "Distance: " + currentDistance.ToString("F1") + "m";
        }
    }
<<<<<<< HEAD
=======

>>>>>>> e8926b28a41fdb8c9ff695e3fd84c9c459b6bff1
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
        HideComboUI();
    }
    private void ShowComboUI(int combo)
    {
        if (comboText != null)
        {
            comboText.text = $"Combo x{combo}!";
            comboText.transform.localScale = Vector3.zero; // Scale trước khi bật
            comboText.gameObject.SetActive(true);

            comboText.transform.DOScale(1.5f, 0.3f)
                                .SetEase(Ease.OutBack)
                                .OnComplete(() => comboText.transform.DOScale(1f, 0.1f));

            Debug.Log("Combo UI show with tween!");
        }
    }

    private void HideComboUI()
    {
        if (comboText != null)
        {
            comboText.gameObject.SetActive(false);
        }
    }
}
