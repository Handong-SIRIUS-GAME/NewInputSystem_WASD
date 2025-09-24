using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;   // 좌우 이동 속도
    [SerializeField] private float jumpForce = 12f;  // 점프 힘

    private Rigidbody2D rb;
    private bool isGrounded = false;
    Animator animator;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;   // 바닥 체크용 위치
    [SerializeField] private float checkRadius = 0.2f; // 바닥 체크 반경
    [SerializeField] private LayerMask groundLayer;   // 바닥 레이어

    // Animator 파라미터 해시
    private int isMoveHash, isJumpHash, isFallHash;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        isMoveHash = Animator.StringToHash("isMove");
        isJumpHash = Animator.StringToHash("isJump");
        isFallHash = Animator.StringToHash("isFall");
    }

    public void Move(float direction)
    {
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        // 좌우 반전
        if (direction != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
        }

        animator.SetBool(isMoveHash, Mathf.Abs(direction) > 0.01f);
    }

    public void Jump()
    {
        Debug.Log("점프 시도");
        if (!isGrounded)
        {
            Debug.Log("아직 떨어지지 않음..");
            return;
        }
        
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            Debug.Log("점프!");

            animator.SetBool(isJumpHash, true);
            animator.SetBool(isFallHash, false);
        }
    }

    private void Update()
    {
        if (isGrounded)
        {
            animator.SetBool(isJumpHash, false);
            animator.SetBool(isFallHash, false);
        }
        else
        {
            float vy = rb.linearVelocity.y;
            animator.SetBool(isJumpHash, vy >  0.05f);   // 상승 중
            animator.SetBool(isFallHash, vy < -0.05f);   // 하강 중
        }
    }

    private void FixedUpdate()
    {
        // 바닥 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
