using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public event Action<Vector2> OnMovementInput = delegate { };
    public event Action<Vector2> OnCameraInput = delegate { };
    public event Action OnJumpInput = delegate { };

    public void HandleMovementInput(InputAction.CallbackContext ctx)
    {
        OnMovementInput.Invoke(ctx.ReadValue<Vector2>());
    }

    public void HandleCameraInput(InputAction.CallbackContext ctx)
    {
        OnCameraInput.Invoke(ctx.ReadValue<Vector2>());
    }

    public void HandleJumpInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            OnJumpInput.Invoke();
    }
}