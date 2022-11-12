using System;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    [field: SerializeField] public float StickDeadzone { get; private set; }
    [field: SerializeField] public float inputCooldown { get; private set; }
    public Vector2 MovementValue { get; private set; }
    public float InputForceX { get; private set; }
    public float InputForceY { get; private set; }
    public int NormInputX { get; private set;}
    public int NormInputY { get; private set; }
    public event Action JumpEvent;
    public event Action SitStandEvent;
    private Controls controls;


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

        NormInputX = StickLimiter(InputForceX, (int)(MovementValue * Vector2.right).normalized.x);
        NormInputY = StickLimiter(InputForceY, (int)(MovementValue * Vector2.up).normalized.y); 
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpEvent?.Invoke();
        }
    }

    public void OnSitStand(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SitStandEvent?.Invoke();
        }
    }

    private int StickLimiter(float force, int input) => force > StickDeadzone ? input : 0;
}