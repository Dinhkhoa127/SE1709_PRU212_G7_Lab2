using UnityEngine;
using TMPro;

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
    
}
