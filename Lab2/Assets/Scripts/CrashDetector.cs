using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] private int currentLevelIndex = 0;
    [SerializeField] private float reload = 1f;
    [SerializeField] ParticleSystem finishEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground"))
    {
        finishEffect.Play();

        // Tìm Rigidbody2D của player và khóa lại
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        // Nếu có script điều khiển, hãy vô hiệu hóa nó
        // GetComponent<PlayerController>().enabled = false;

        Invoke("ReloadScene", reload);
    }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(currentLevelIndex);
    }
}
