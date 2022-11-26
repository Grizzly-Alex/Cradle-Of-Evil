using System;
using UnityEngine;

[Serializable]
public struct PlayerData
{
    [field: Header("MOVEMENT")]
    [field: SerializeField] public float StandingMoveSpeed { get; private set; }
    [field: SerializeField] public float CrouchingMoveSpeed { get; private set; }

    [field: Header("JUMPING")]
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public int AmountOfJumps{ get; private set; }

    [field: Header("Air")]
    [field: SerializeField] public float InAirMoveSpeed { get; private set; }

    [field: Header("LANDING")]
    [field: SerializeField] public float LandingThreshold { get; private set; }

    [field: Header("DASHING")]
    [field: SerializeField] public float DashingSpeed { get; private set; }
    [field: SerializeField] public float DashingTime { get; private set; }
    [field: SerializeField] public float DashingCooldown { get; private set; }

    [field: Header("COLIDER SIZE")]
    [field: SerializeField] public float StandingColiderHeight { get; private set; }
    [field: SerializeField] public float CrouchingColiderHeight { get; private set; }
}