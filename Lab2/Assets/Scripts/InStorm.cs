using UnityEngine;

public class InStorm : MonoBehaviour
{
    [Header("Cài đặt vùng ảnh hưởng")]
    public Transform playerTransform;           // Tham chiếu tới Player
    public Transform particleSystemTransform;   // Tham chiếu tới Particle System
    public float effectRange = 5f;              // Bán kính ảnh hưởng của particle system

    [Header("Tốc độ di chuyển")]
    public float normalSpeed = 12f;             // Tốc độ bình thường
    public float slowDownFactor = 0.3f;         // Hệ số làm chậm (ví dụ: 0.3 = chậm còn 30%)


    [SerializeField] private GameObject ground;

    private float currentSpeed;
    private SurfaceEffector2D surfaceEffector;
    private bool isInStorm = false;

    void Start()
    {
        currentSpeed = normalSpeed;
        surfaceEffector = ground.GetComponent<SurfaceEffector2D>();

        // Ẩn particle system ngay từ đầu
        if (particleSystemTransform != null)
        {
            particleSystemTransform.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        float distanceToParticleSystem = Vector3.Distance(playerTransform.position, particleSystemTransform.position);

        if (distanceToParticleSystem <= effectRange)
        {
            if (!isInStorm)
            {
                isInStorm = true;
                particleSystemTransform.gameObject.SetActive(true); // Bật hiệu ứng
                AudioController.instance.PlaySnowStormSound(); // Phát âm thanh bão tuyết
                Debug.Log("Player is in the snowstorm → Slow down");
            }

            currentSpeed = normalSpeed * slowDownFactor;
        }
        else
        {
            if (isInStorm)
            {
                isInStorm = false;
                Debug.Log("Player is outside the storm → Normal speed");
            }

            currentSpeed = normalSpeed;
        }

        surfaceEffector.speed = currentSpeed;
    }


    void OnDrawGizmosSelected()
    {
        if (particleSystemTransform != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(particleSystemTransform.position, effectRange);
        }
    }


}
