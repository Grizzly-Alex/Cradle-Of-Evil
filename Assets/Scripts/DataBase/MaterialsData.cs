using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MaterialsData 
{
    [field: SerializeField] public PhysicsMaterial2D NoFriction { get; private set; }
    [field: SerializeField] public PhysicsMaterial2D Friction { get; private set; }
}
