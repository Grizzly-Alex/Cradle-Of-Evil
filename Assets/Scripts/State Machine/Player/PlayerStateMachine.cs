using System;
using UnityEngine;

public sealed class PlayerStateMachine: StateMachine
{
    [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }
    [field: SerializeField] public CapsuleCollider2D BodyCollider { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public Core Core { get; private set; }
    [field: SerializeField] public MaterialsData MaterialsData { get; private set; }
    [field: SerializeField] public PlayerData PlayerData { get; private set; }

    private void Start()
    {
        InitState(new PlayerStandingState(this));
    }
}