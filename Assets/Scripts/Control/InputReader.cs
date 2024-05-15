using System;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    #region InputXY
    private float inputX;
    private float inputY;
    public int InputHorizontal { get; private set;}
    public int InputVertical { get; private set; }       
    [field: SerializeField] public float StickDeadzone { get; private set; }
    #endregion

    #region InputEvents
    public event Action JumpEvent;
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

        inputX = Mathf.Abs(MovementValue.x);
        inputY = Mathf.Abs(MovementValue.y);

        InputHorizontal = StickLimiter(inputX, (int)(MovementValue * Vector2.right).normalized.x);
        InputVertical = StickLimiter(inputY, (int)(MovementValue * Vector2.up).normalized.y); 
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpEvent?.Invoke();
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