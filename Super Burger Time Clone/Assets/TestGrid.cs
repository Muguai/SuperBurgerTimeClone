using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    private Grid grid;
    public GameObject debugGrid;

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Vector3Int cellPosition = grid.WorldToCell(debugGrid.transform.position);
            Debug.Log("Cell Pos: " + cellPosition);
        }
    }
}
