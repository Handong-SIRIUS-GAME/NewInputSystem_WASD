using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private Movement2D movement;
    private float moveX = 0f;

    private void Awake()
    {
        movement = GetComponent<Movement2D>();
    }

    private void Update()
    {
        // 유지 입력을 매 프레임 이동에 반영
        movement.Move(moveX);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        // 취소되면 0으로
        if (ctx.canceled)
        {
            moveX = 0f;
            return;
        }

        Vector2 input = ctx.ReadValue<Vector2>();
        moveX = Mathf.Clamp(input.x, -1f, 1f);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        // performed 시에만 점프
        if (ctx.performed)
        {
            movement.Jump();
            Debug.Log("점프입력");
        }
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            movement.Attack();
        }
    }
}
