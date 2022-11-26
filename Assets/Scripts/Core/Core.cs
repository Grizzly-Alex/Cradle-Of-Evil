using UnityEngine;

public sealed class Core : MonoBehaviour
{
    [field: SerializeField] public Movement Movement { get; private set; }
    [field: SerializeField] public CollisionSensors CollisionSensors { get; private set; }

    public void LogicUpdate()
    {
        Movement.LogicUpdate();
    }
}
