using UnityEngine;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public TextMeshProUGUI pauseScoreText;
    public TextMeshProUGUI pauseDistanceText;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        if (GameManager.Instance != null)
        {
            float score = GameManager.Instance.GetCurrentScore();
            float distance = GameManager.Instance.GetCurrentDistance();
            PlayerPrefs.SetFloat("PausedScore", score);
            PlayerPrefs.SetFloat("PausedDistance", distance);
            PlayerPrefs.Save();

            // Cập nhật UI
            if (pauseScoreText != null)
                pauseScoreText.text = Mathf.RoundToInt(score).ToString("D6");
            if (pauseDistanceText != null)
                pauseDistanceText.text = distance.ToString("F1") + "m";
        }
    }
}