﻿using System;
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
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public float DoubleJumpForce { get; private set; }
        [field: SerializeField] public float WallJumpTime { get; private set; }

        [field: Header("LANDING")]
        [field: SerializeField] public float LandingThreshold { get; private set; }

        [field: Header("DASHING")]
        [field: SerializeField] public float DashSpeed { get; private set; }
        [field: SerializeField] public float DashTime { get; private set; }

		[field: Header("SLIDING")]
		[field: SerializeField] public float SlideSpeed { get; private set; }
        [field: SerializeField] public float SlideTime { get; private set; }
        [field: SerializeField] public float SlideCooldown { get; private set; }

        [field: Header("COLIDER SIZE")]
        [field: SerializeField] public float StandColiderHeight { get; private set; }
        [field: SerializeField] public float CrouchColiderHeight { get; private set; }
        #endregion
    }
}
