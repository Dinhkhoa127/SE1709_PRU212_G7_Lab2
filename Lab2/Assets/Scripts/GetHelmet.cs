using UnityEngine;

public class GetHelmet : MonoBehaviour
{
    [SerializeField] private GameObject helmet;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(helmet.activeSelf)
        {
            Debug.Log("Helmet is already active — collection ignored.");
            return;
        }

        if (collision.CompareTag("Player"))
        {
            // Equip the helmet to the player
            Debug.Log("Helmet has been collected!");
            helmet.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
