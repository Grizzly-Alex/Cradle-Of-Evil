using System;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    #region InputXY
    [field: SerializeField] public float StickDeadzone { get; private set; }
    public float InputForceX { get; private set; }
    public float InputForceY { get; private set; }
    public int NormInputX { get; private set;}
    public int NormInputY { get; private set; }       
    #endregion

    #region InputEvents
    public event Action JumpEvent;
    public event Action SitStandEvent;
    public event Action DashEvent;
    #endregion

    #region InputCooldown
    public bool CanInput { get; private set; }
    private float inputCooldown;
    public float InputCooldown
    {
        get { return inputCooldown; }
        set { inputCooldown = value + Time.time; }
    }  
    #endregion


    public Vector2 MovementValue { get; private set; }
    private Controls controls;


    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    private void Update()
    {
        CanInput = CheckInputCooldown(inputCooldown);  
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

    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.performed)
        { 
            DashEvent?.Invoke();
        }
    }

    private int StickLimiter(float force, int input) => force > StickDeadzone ? input : 0;
    private bool CheckInputCooldown(float finishTime) => Time.time >= finishTime;
}