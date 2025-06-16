
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float torqueAmount = 1f;
    private Rigidbody2D rb;

    // Thêm một biến kiểm tra nếu nhân vật đang chạm đất
    private bool isGrounded;

    // Ground check params
    [SerializeField] private Transform groundCheck; // khi điểm này chạm mặt đất tức là thg player đang trên mặt đất
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer; // thg này sẽ quyết định thg nào trong thế giới game đóng vai trò là mặt đất
    [SerializeField] private float jumpForce = 15f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Tạo groundCheck object nếu chưa có
        if (groundCheck == null)
        {
            GameObject groundCheckObj = new GameObject("GroundCheck");
            groundCheckObj.transform.SetParent(transform);
            groundCheckObj.transform.localPosition = new Vector3(0, -0.5f, 0); // Dưới chân player
            groundCheck = groundCheckObj.transform;
        }
    }

    void Update()
    {
        // Kiểm tra nếu nhân vật đang ở trên mặt đất
        isGrounded = CheckIfGrounded();

        // Chỉ thực hiện roll khi không phải trên mặt đất
        if (!isGrounded)
        {
            Roll();
        }
        else
        {
            HandleJump();
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed and jumping! isGrounded: " + isGrounded);
            // Giữ nguyên tốc độ trượt ngang (velocity.x), chỉ thay đổi velocity.y để nhảy
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
    private bool CheckIfGrounded()
    {
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (grounded)
        {
            Debug.Log("Player is grounded!");
        }
        else
        {
            Debug.Log("Player is NOT grounded!");
        }

        return grounded;
    }

    // Để visualize vùng check ground trong Scene view
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
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
