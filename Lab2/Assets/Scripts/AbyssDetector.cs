using UnityEngine;

public class AbyssDetector : MonoBehaviour
{
    [SerializeField] private string gameOverSceneName = "EndGameScreen";
    [SerializeField] private float reload = 1f;
    [SerializeField] private ParticleSystem crashEffect;
    [SerializeField] private GameObject helmet;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.enabled && collision.gameObject.CompareTag("Player"))
        {
            if (helmet != null && helmet.activeSelf)
            {
                {
                    // Disable the helmet if it exists
                    helmet.SetActive(false);
                }
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.bodyType = RigidbodyType2D.Static;
                }

                // Trigger the abyss effect
                Debug.Log("Player has entered the abyss!");
                AudioController.instance.PlayCrashSound();
                // Additional logic for abyss effect can be added here
                Invoke("LoadGameOverScene", reload);
            }
        }
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
