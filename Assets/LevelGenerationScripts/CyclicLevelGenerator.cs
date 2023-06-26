using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclicLevelGenerator : MonoBehaviour
{
    Grid grid;
    public GameObject cellPrefab;
    public Dictionary<int, GameObject> cellObjectPool = new Dictionary<int, GameObject>();
    [SerializeField] GameObject startCell;
    [SerializeField] GameObject exitCell;
    [SerializeField] GameObject nextCell;
    int cellNumber;
    [SerializeField] Transform activeCellsParent;
    bool reachedEnd = false;
    private void Awake()
    {
        grid = GetComponent<Grid>();
    }
    private void Start()
    {
        GenerateCells();
        SetStartCell();
        ActivateNextCell(GetNextCell(startCell));
    }
    private void Update()
    {
        if(!reachedEnd)
        ActivateNextCell(GetNextCell(nextCell));
    }
    private void GenerateCells()
    {
        for (int x = 0; x < grid.gridWidth; x++)
        {
            for (int y = 0; y < grid.gridHeight; y++)
            {
                Vector2 point = new Vector2(x, y);
                GameObject cell = Instantiate(cellPrefab, point, Quaternion.identity, transform);
                cellObjectPool.Add(x, cell);
                cell.SetActive(false);
                RoomDimensions cellDimensions = cell.GetComponent<RoomDimensions>();
                cellDimensions.roomDimensions = new Vector3(grid.cellWidth, grid.cellHeight, 0);
            }
        }
    }
    private void SetStartCell()
    {
        startCell = cellObjectPool[cellNumber];
        startCell.name = "Cell#" + cellNumber;
        startCell.tag = "Start";
        startCell.transform.parent = activeCellsParent;
        startCell.SetActive(true);
        startCell.GetComponent<ColorManager>().spriteRenderer.color = Color.green;
        cellNumber++;
    }
    private void ActivateNextCell(GameObject cell)
    {
        nextCell = cell;
        nextCell.SetActive(true);
        nextCell.name = "Cell#" + cellNumber;
        nextCell.transform.parent = activeCellsParent;
        nextCell.GetComponent<ColorManager>().spriteRenderer.color = Color.yellow;
        cellNumber++;
        if (cellNumber == cellObjectPool.Count)
        {
            reachedEnd = true;
            exitCell = cell;
            exitCell.tag = "Exit";
            exitCell.GetComponent<ColorManager>().spriteRenderer.color = Color.red;
        }
    }
    private GameObject GetNextCell(GameObject cell)
    {
        CellWalls startCellWalls = cell.GetComponent<CellWalls>();
        startCellWalls.directionTowardsNextRoom = Vector2.right;
        GameObject _nextCell = cellObjectPool[cellNumber];
        CellWalls cellWalls = _nextCell.GetComponent<CellWalls>();
        cellWalls.previousRoomDirection = Vector2.right;
        return _nextCell;
    }
}
