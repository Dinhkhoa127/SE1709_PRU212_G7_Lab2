using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Explode : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private ParticleSystem crashEffect;

    void Start()
    {
        // Get the Animator component attached to the object  
        animator = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool("isExplode", true);

            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
                playerRb.bodyType = RigidbodyType2D.Static;
            }
            AudioController.instance.PlayBoomSound();
            Destroy(this.gameObject, 0.7f); // Destroy the object after 0.5 seconds  
            Destroy(collision.gameObject, 0.7f);
            StartCoroutine(PlayCrashEffectWithDelay(0.1f));
            Invoke("ReloadScene", 0.6f);
        }
    }

    private IEnumerator PlayCrashEffectWithDelay(float delay)
    {
        // Wait for the specified delay (0.2 seconds)  
        yield return new WaitForSeconds(delay);

        // Play the crash effect after the delay  
        if (crashEffect != null)
        {
            crashEffect.Play();
            
        }
    }

    //private void ReloadScene()
    //{
    //    SceneManager.LoadScene(0);
    //}
    private void ReloadScene()
    {
        // Gọi giống CrashDetector
        // Use the updated method to find the CrashDetector object  
        var crashDetector = Object.FindFirstObjectByType<CrashDetector>();
        if (crashDetector != null)
        {
            crashDetector.LoadGameOverScene();
        }
        else
        {
            // Fallback: load scene hiện tại
            int currentIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentIndex);
        }
    }
}
