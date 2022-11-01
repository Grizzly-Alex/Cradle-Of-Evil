using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    [field: SerializeField] public Movement Movement { get; private set; }
    [field: SerializeField] public CollisionSenses CollisionSenses { get; private set; }

}
