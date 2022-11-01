using System;
using UnityEngine;

[Serializable]
public struct PlayerData
{
    [field: Header("MOVEMENT")]
    [field: SerializeField] public float StandingMoveSpeed { get; private set; }
    [field: SerializeField] public float CrouchingMoveSpeed { get; private set; }

}
