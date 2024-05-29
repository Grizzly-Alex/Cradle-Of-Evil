using CoreSystem.Components;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LadderTrigger : MonoBehaviour
{
    private Sensor sensor;
    private Grid grid;
    private void Awake()
    {
        sensor = GameObject.FindAnyObjectByType<Sensor>();
        grid = GameObject.FindAnyObjectByType<Grid>();

    }

    private void Start()
    {
        Debug.Log(sensor);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {     
        sensor.IsLadder = true;


        Vector3Int cellPosition = grid.WorldToCell(other.transform.position);
        Debug.Log(cellPosition);
        Vector3 centerOfCell = grid.GetCellCenterWorld(cellPosition);
        Debug.Log(centerOfCell);


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        sensor.IsLadder = false;
    }
}
