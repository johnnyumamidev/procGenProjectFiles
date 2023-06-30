using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclicLevelGenerator : MonoBehaviour
{
    [SerializeField] GameEvent levelGenerationCompleteEvent;

    [SerializeField] DungeonManager dungeonManager;
    [SerializeField] int numberOfElevatorRooms = 2;
    [SerializeField] int width;
    [SerializeField] int height;
    public List<Vector2> points = new List<Vector2>();
    public Transform startGridPosition;

    public GameObject cellPrefab;
    public Dictionary<int, GameObject> activeCells = new Dictionary<int, GameObject>();
    public GameObject startCell;
    [SerializeField] GameObject exitCell;
    [SerializeField] GameObject lockedRoomCell;
    [SerializeField] GameObject nextCell;
    int cellNumber;
    [SerializeField] Transform activeCellsParent;
    bool reachedEnd = false;
    private void Awake()
    {
        DetermineGridSize();
        GenerateGrid();
    }

    private void DetermineGridSize()
    {
        width = numberOfElevatorRooms + dungeonManager.currentDungeon.startingFloorCount;
        Debug.Log("determine grid width: " + width);
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 point = new Vector2(startGridPosition.position.x + x, startGridPosition.position.y + y);
                points.Add(point);
            }
        }
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
        for (int i = 0; i < width; i++)
        {
            Debug.Log("spawning cell" + i);
            Vector2 point = new Vector2(i, 0);
            GameObject cell = Instantiate(cellPrefab, point, Quaternion.identity, transform);
            activeCells.Add(i, cell);
            cell.SetActive(false);
            RoomDimensions cellDimensions = cell.GetComponent<RoomDimensions>();
            cellDimensions.roomDimensions = new Vector3(1, 1, 0);
        }
        /*
        int i = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Debug.Log("spawning cell");
                Vector2 point = new Vector2(x, y);
                GameObject cell = Instantiate(cellPrefab, point, Quaternion.identity, transform);
                cellObjectPool.Add(i, cell);
                cell.SetActive(false);
                RoomDimensions cellDimensions = cell.GetComponent<RoomDimensions>();
                cellDimensions.roomDimensions = new Vector3(1, 1, 0);
                i++;
            }
        */
    }
    private void SetStartCell()
    {
        startCell = activeCells[cellNumber];
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
        if (cellNumber == activeCells.Count)
        {
            reachedEnd = true;
            levelGenerationCompleteEvent?.Raise();
            exitCell = cell;
            exitCell.tag = "Exit";
            exitCell.GetComponent<ColorManager>().spriteRenderer.color = Color.red;
        }
        else if(cellNumber == activeCells.Count - 1)
        {
            lockedRoomCell = cell;
            lockedRoomCell.tag = "LockedRoom";
        }
    }
    private GameObject GetNextCell(GameObject cell)
    {
        CellWalls startCellWalls = cell.GetComponent<CellWalls>();
        startCellWalls.directionTowardsNextRoom = Vector2.right;
        GameObject _nextCell = activeCells[cellNumber];
        CellWalls cellWalls = _nextCell.GetComponent<CellWalls>();
        cellWalls.previousRoomDirection = Vector2.right;
        return _nextCell;
    }
}
