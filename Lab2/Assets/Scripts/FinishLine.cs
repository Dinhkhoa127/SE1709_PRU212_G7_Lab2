using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private int currentLevelIndex = 0;
    [SerializeField] private float reload = 0.75f;
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
