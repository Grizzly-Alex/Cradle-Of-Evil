using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovementValue { get; private set; }
    public float InputForceX { get; private set; }
    public float InputForceY { get; private set; }
    public int NormInputX { get; private set;}
    public int NormInputY { get; private set; }
    public event Action JumpEvent;
    private Controls controls;
    private const float stickDeadzone = 0.5f;


    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();

        InputForceX = Mathf.Abs(MovementValue.x);
        InputForceY = Mathf.Abs(MovementValue.y);

        if (InputForceX > stickDeadzone)
        {
            NormInputX = (int)(MovementValue * Vector2.right).normalized.x;
        }
        else
        {
            NormInputX = 0;
        }

        if (InputForceY > stickDeadzone)
        {
            NormInputY = (int)(MovementValue * Vector2.up).normalized.y;
        }
        else
        {
            NormInputY = 0;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpEvent?.Invoke();
        }
    }
}