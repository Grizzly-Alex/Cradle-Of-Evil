using UnityEngine;

public class PlayerStateMachine: StateMachine
{
    [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }
    [field: SerializeField] public BoxCollider2D BoxCollider { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public InputReader InputReader { get; private set; }


    private void Start()
    {
        InitState(new PlayerStandingState(this));
    }
}
