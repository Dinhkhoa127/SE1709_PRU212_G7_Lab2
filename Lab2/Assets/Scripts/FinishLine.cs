using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private int currentLevelIndex = 0;
    [SerializeField] private float reload = 1.5f;
    [SerializeField] ParticleSystem finishEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            finishEffect.Play();
            Invoke("ReloadScene", reload); 
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(currentLevelIndex);
    }
}
