using System;
using UnityEngine;

[Serializable]
public struct PlayerData
{
    [field: Header("MOVEMENT")]
    [field: SerializeField] public float StandingMoveSpeed { get; private set; }
    [field: SerializeField] public float CrouchingMoveSpeed { get; private set; }

    [field: Header("COLIDER SIZE")]
    [field: SerializeField] public float StandingColiderHeight { get; private set; }
    [field: SerializeField] public float CrouchingColiderHeight { get; private set; }

}
