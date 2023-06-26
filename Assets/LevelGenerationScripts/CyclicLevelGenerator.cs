using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclicLevelGenerator : MonoBehaviour
{
    Grid grid;
    public GameObject cellPrefab;
    public Dictionary<Vector2, GameObject> cellObjectPool = new Dictionary<Vector2, GameObject>();

    [SerializeField] GameObject startCell;
    [SerializeField] GameObject exitCell;
    [SerializeField] GameObject nextCell;

    List<Vector2> directions = new List<Vector2> { Vector2.up, Vector2.right, Vector2.left };
    List<Vector2> neighbors = new List<Vector2>();
    int cellNumber;

    [SerializeField] Transform activeCellsParent;
    bool firstPathReachedTopLevel = false;
    List<Vector2> possibleDirections = new List<Vector2>();
    Vector2 lastDirection;
    private void Awake()
    {
        grid = GetComponent<Grid>();
    }
    private void Start()
    {
        GenerateCells();
        SetStartAndExitCells();
        StartWalk();
    }

    private void Update()
    {
        if(!firstPathReachedTopLevel)
        ActivateNextCell(GetNextCell(nextCell.transform.position));
    }


    private void GenerateCells()
    {
        for (int x = 0; x < grid.gridWidth; x++)
        {
            for (int y = 0; y < grid.gridHeight; y++)
            {
                Vector2 point = new Vector2(x, y);
                GameObject cell = Instantiate(cellPrefab, point, Quaternion.identity, transform);
                cellObjectPool.Add(point, cell);
                cell.SetActive(false);
                RoomDimensions cellDimensions = cell.GetComponent<RoomDimensions>();
                cellDimensions.roomDimensions = new Vector3(grid.cellWidth, grid.cellHeight, 0);
            }
        }
    }

    private void SetStartAndExitCells()
    {
        List<GameObject> lowestYCells = new List<GameObject>();
        List<GameObject> highestYCells = new List<GameObject>();

        foreach (Vector2 point in  cellObjectPool.Keys)
        {
            if (point.y == 0) lowestYCells.Add(cellObjectPool[point]);
            else if(point.y == grid.gridHeight-1) highestYCells.Add(cellObjectPool[point]);
        }

        startCell = GetRandomCell(lowestYCells);
        startCell.name = "Cell#" + cellNumber;
        startCell.transform.parent = activeCellsParent;
        startCell.SetActive(true);
        startCell.GetComponent<ColorManager>().spriteRenderer.color = Color.green;
        cellNumber++;

        exitCell = GetRandomCell(highestYCells);
        exitCell.name = "Exit";
        exitCell.transform.parent = activeCellsParent;
        exitCell.SetActive(true);
        exitCell.GetComponent<ColorManager>().spriteRenderer.color = Color.red;
    }
    GameObject GetRandomCell(List<GameObject> list)
    {
        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }

    private void StartWalk()
    {
        Vector2 startCellPosition = startCell.transform.position;
        ActivateNextCell(GetNextCell(startCellPosition));
    }

    private void ActivateNextCell(GameObject cell)
    {
        nextCell = cell;
        nextCell.SetActive(true);
        nextCell.name = "Cell#" + cellNumber;
        nextCell.transform.parent = activeCellsParent;
        nextCell.GetComponent<ColorManager>().spriteRenderer.color = Color.yellow;
        cellNumber++;
    }

    private GameObject GetNextCell(Vector2 point)
    {
        Vector2 neighbor = GetNeighbor(point);
        return cellObjectPool[neighbor];
    }

    private Vector2 GetNeighbor(Vector2 position)
    {
        neighbors.Clear();
        for (int i = 0; i < directions.Count; i++)
        {
            Vector2 neighborPoint = position + directions[i];
            if (!cellObjectPool.ContainsKey(neighborPoint))
            {
                Debug.Log(neighborPoint + " is out of bounds");
            }
            else if (cellObjectPool[neighborPoint].activeSelf == true)
            {
                Debug.Log(neighborPoint + " is already an active cell");
            }
            else if (cellNumber == 1 && directions[i] == Vector2.up)
            {
                Debug.Log("first cell cannot go up, must go left or right");
            }
            else
            {
                Debug.Log("valid neighbor: " + neighborPoint + ", direction: " + directions[i]);
                neighbors.Add(neighborPoint);
                possibleDirections.Add(directions[i]);
            }
        }
        if(neighbors.Count <= 0)
        {
            Debug.LogError("no valid neighbors");
        }
        int randomNeighborIndex = Random.Range(0, neighbors.Count);
        Vector2 point = neighbors[randomNeighborIndex];
        lastDirection = possibleDirections[randomNeighborIndex];
        Debug.Log("chosen neighbor: " + point);
        if(point.y >= grid.gridHeight - 1)
        {
            firstPathReachedTopLevel = true;
            Debug.Log("reached top level");
        }
        return point;
    }
}
