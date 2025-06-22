using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private float scoreValue = 15f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           
            GameManager.Instance.AddScore(scoreValue);

          
            Destroy(gameObject);
        }
    }
}
