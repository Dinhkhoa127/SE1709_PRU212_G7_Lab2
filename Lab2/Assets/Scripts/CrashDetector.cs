using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] private int currentLevelIndex = 0;
    [SerializeField] private float reload = 1f;
    [SerializeField] ParticleSystem finishEffect;
    [SerializeField] private GameObject helmet;
    [SerializeField] private string gameOverSceneName = "EndGameScreen";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isObstacle = collision.CompareTag("Obstacles");
        bool isGround = collision.CompareTag("Ground");

        // If helmet active, handle collisions differently
        if (helmet != null && helmet.activeSelf)
        {
            // With obstacle when helmet is active — lose helmet, no death
            if (isObstacle)
            {
                Debug.Log("Crash obstacle, with helmet - no death");
                AudioController.instance.PlayHelmetCrashSound();
                Destroy(collision.gameObject); // Delete obstacle
                helmet.SetActive(false);       // Now helmet is inactive
                return;
            }

            // with ground when helmet is active — no death
            if (isGround)
            {
                Debug.Log("Crash ground, with helmet - no death");
                return;
            }
            // return to disable death logic if helmet is active
            return;
        }

        // if dont have helmet, both obstacle and ground collisions result in death
        if (isObstacle || isGround)
        {
            Debug.Log("No helmet - Death");
            AudioController.instance.PlayCrashSound();
            finishEffect.Play();

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
            }

            //Invoke("ReloadScene", reload);
            Invoke("LoadGameOverScene", reload);
        }
    }



    private void ReloadScene()
    {
        SceneManager.LoadScene(currentLevelIndex);
    }

    public void LoadGameOverScene()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveGameOverData();
        }
        // Lưu lại tên map vừa chơi
        string lastMapName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LastMapName", lastMapName);
        PlayerPrefs.Save();

        UnityEngine.SceneManagement.SceneManager.LoadScene(gameOverSceneName);
    }

}
