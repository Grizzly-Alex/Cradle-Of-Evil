using System;
using UnityEngine;

namespace Entities
{
    [Serializable]
    public struct PlayerData 
    {
        #region Data
        [field: Header("MOVEMENT")]
        [field: SerializeField] public float StandMoveSpeed { get; private set; }
        [field: SerializeField] public float CrouchMoveSpeed { get; private set; }
        [field: SerializeField] public float InAirMoveSpeed { get; private set; }

        [field: Header("JUMPING")]
        [field: SerializeField] public float FirstJumpForce { get; private set; }
        [field: SerializeField] public float SecondJumpForce { get; private set; }
        [field: SerializeField] public float WallJumpTime { get; private set; }
        public readonly int AmountOfJump => 2;

        [field: Header("GRAB LEDGE")]
        [field: SerializeField] public float GrabLedgeCooldown { get; private set; }

        [field: Header("LANDING")]
        [field: SerializeField] public float LandingThreshold { get; private set; }

        [field: Header("DASHING")]
        [field: SerializeField] public float LevitationDashSpeed { get; private set; }
        [field: SerializeField] public float LevitationDashTime { get; private set; }
        [field: SerializeField] public float GroundDashSpeed { get; private set; }
        [field: SerializeField] public float GroundDashTime { get; private set; }
        [field: SerializeField] public float GroundDashCooldown { get; private set; }

        [field: Header("COLIDER SIZE")]
        [field: SerializeField] public float StandColiderHeight { get; private set; }
        [field: SerializeField] public float CrouchColiderHeight { get; private set; }
        #endregion
    }
}
