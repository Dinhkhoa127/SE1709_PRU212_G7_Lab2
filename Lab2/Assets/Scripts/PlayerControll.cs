
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float torqueAmount = 1f;
    private Rigidbody2D rb;

    // Thêm một biến kiểm tra nếu nhân vật đang chạm đất
    private bool isGrounded;

    // Raycast check params
    [SerializeField] private float groundCheckDistance = 0.1f;  // Khoảng cách kiểm tra dưới chân
    [SerializeField] private LayerMask groundLayer;  // Mặt đất nằm trong Layer này (bạn cần thiết lập trong Unity)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Kiểm tra nếu nhân vật đang ở trên mặt đất
        CheckIfGrounded();

        // Chỉ thực hiện roll khi không phải trên mặt đất
        if (!isGrounded)
        {
            Roll();
        }
    }


    private void CheckIfGrounded()
    {
        // Raycast từ dưới chân nhân vật để kiểm tra mặt đất
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        if (hit.collider != null)
        {
            // Nếu raycast va chạm với mặt đất, thì nhân vật đang trên mặt đất
            isGrounded = true;
        }
        else
        {
            // Nếu raycast không va chạm, tức là nhân vật không trên mặt đất
            isGrounded = false;
        }
    }

    private void Roll()
    {
        float currentAngularVelocity = rb.angularVelocity;

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(torqueAmount);
        }
        else if (Input.GetKey(KeyCode.D))
        {  
            rb.AddTorque(-torqueAmount);
        }
    }

}
