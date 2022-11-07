using UnityEngine;

public class CoreComponent: MonoBehaviour
{
    protected Core core;

    protected virtual void Awake()
    {
        core = GetComponent<Core>();
    }  
}
