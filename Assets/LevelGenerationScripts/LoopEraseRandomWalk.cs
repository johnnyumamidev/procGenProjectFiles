using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;

public class LoopEraseRandomWalk : MonoBehaviour
{
    Grid grid;
    public GameObject cellPrefab;

    List<Vector2> directions = new List<Vector2> { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    List<string> directionNames = new List<string> { "North", "East", "South", "West" };
    Vector2 lastDirection;
    public string lastDirectionName;

    public Vector2 targetCell;
    Dictionary<int, Vector2> cellPath = new Dictionary<int, Vector2>();
    public List<Vector2> cellPoints;
    List<GameObject> treasureRooms = new List<GameObject>();
    public List<GameObject> cellObjectPool = new List<GameObject>();
    public List<GameObject> activeCells = new List<GameObject>();
    public List<GameObject> fillerCells = new List<GameObject>();
    [SerializeField] int pathIndex = 0;
    public float timeBetweenIteration;

    public int numberOfLoops;
    public int pathLength;
    public bool pathActive;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Start()
    {
        targetCell = GetPoint();

        for(int i = 0; i < (grid.gridHeight * grid.gridWidth) * 1.5f; i++)
        {
            GameObject cell = Instantiate(cellPrefab, transform.position, Quaternion.identity, transform);
            cell.name = "Cell#: " + (i);
            cellObjectPool.Add(cell);
            cell.SetActive(false);
            RoomDimensions cellDimensions = cell.GetComponent<RoomDimensions>();
            cellDimensions.roomDimensions = new Vector3(grid.cellWidth, grid.cellHeight, 0);
        }
        InvokeRepeating("HandleRandomWalk", 0, timeBetweenIteration);
    }

    private void Update()
    {
        int directionIndex = 0;
        if (lastDirection == Vector2.up) { directionIndex = 0; }
        if (lastDirection == Vector2.right) { directionIndex = 1; }
        if (lastDirection == Vector2.down) { directionIndex = 2; }
        if (lastDirection == Vector2.left) { directionIndex = 3; }
        lastDirectionName = directionNames[directionIndex]; 
    }

    private Vector2 GetPoint()
    {
        int randomIndex = Random.Range(0, grid.points.Count);
        Vector2 point = grid.points[randomIndex];
        return point;
    }

    private void HandleRandomWalk()
    {
        if (pathIndex == 0)
        {
            pathActive = true;
            if (cellPath.ContainsKey(pathIndex)) return;
            cellPath.Add(pathIndex, GetPoint());
            Debug.Log("start point: " + cellPath[pathIndex]);
        }

        GameObject cell = NewActiveCell();

        GetRandomDirection(out int randomDirectionIndex, out Vector2 randomDirection);
        Vector2 nextPoint = cellPath[pathIndex] + (randomDirection * cell.GetComponent<RoomDimensions>().roomDimensions);

        Debug.Log(cellObjectPool[pathIndex].name + " walking to => " + directionNames[randomDirectionIndex] + " // " + nextPoint);
        Debug.Log("direction: " + directionNames[randomDirectionIndex] + ", " + randomDirection);

    CheckPath:
        if (pathIndex > 0 && randomDirection == -lastDirection)
        {
            Debug.Log("backpedal");
            ResetNextPoint(randomDirection, out nextPoint, out randomDirectionIndex, out randomDirection);
            goto CheckPath;
        }
        else if (!grid.points.Contains(nextPoint))
        {
            Debug.Log("out of bounds");
            ResetNextPoint(randomDirection, out nextPoint, out randomDirectionIndex, out randomDirection);
            goto CheckPath;
        }

        if (cellPath.ContainsValue(nextPoint))
        {
            GetKeyFromValue(cellPath, nextPoint, out int gridKey);
            Debug.Log("loop intersection at " + cell.name + " and Cell# " + gridKey + ", erasing loop");
            Debug.Log("new start point: " + gridKey);
            for (int i = gridKey; i <= pathIndex; i++)
            {
                cellObjectPool[i].SetActive(false);
                cellPath.Remove(i);
                Debug.Log("destroying: " + cellObjectPool[i].name);
            }

            pathIndex = gridKey - 1;
            numberOfLoops++;
        }
        
        lastDirection = randomDirection;
        DisableCellWalls(cell, randomDirection);

        if (pathIndex < pathLength)
        {
            pathIndex++;
            if (pathIndex == 0) return;
            cellPath.Add(pathIndex, nextPoint);
        }
        else
        {
            pathActive = false;
            cellPoints = new List<Vector2>(cellPath.Values);
            Debug.Log("intial path complete");
            CancelInvoke();
            SetStartAndEndCells();
            InvokeRepeating("CreateBranchingPaths", 0, timeBetweenIteration);
        }
    }

    private GameObject NewActiveCell()
    {
        GameObject cell = cellObjectPool[pathIndex];
        cell.SetActive(true);
        cell.transform.position = cellPath[pathIndex];
        ColorManager colorManager = cell.GetComponent<ColorManager>();
        colorManager.spriteRenderer.color = Color.white;
        activeCells.Add(cell);
        return cell;
    }

    private void DisableCellWalls(GameObject cell, Vector2 randomDirection)
    {
        WallsManager cellWalls = cell.GetComponent<WallsManager>();
        cellWalls.roomIndex = pathIndex;
        if (pathIndex > 0)
        {
            cellWalls.lastDirection = cellObjectPool[pathIndex - 1].GetComponent<WallsManager>().direction;
        }
        cellWalls.direction = randomDirection;
    }

    private void GetRandomDirection(out int randomDirectionIndex, out Vector2 randomDirection)
    {
        randomDirectionIndex = Random.Range(0, directions.Count - 1);
        randomDirection = directions[randomDirectionIndex];
    }

    List<Vector2> points;
    int branchingPathLoopCount;
    int branchingPathIndex;
    public int desiredBranchingPaths = 1;
    public int minimumCellsFromStartCell = 4;
    List<Vector2> neighbors = new List<Vector2>();
    List<Vector2> neighborDirection = new List<Vector2>();
    private void CreateBranchingPaths()
    {
    //get starting point
    StartBranch:
        GetPointOnPath(out Vector2 randomPointWithinPath);
        if (!cellPath.ContainsValue(randomPointWithinPath))
        {
            Debug.Log(randomPointWithinPath + ", branch is not connected to main path");
            goto StartBranch;
        }
        GetKeyFromValue(cellPath, randomPointWithinPath, out int pathKey);
        GetKeyFromValue(cellPath, startCell.transform.position, out int startCellKey);
        if (Mathf.Abs(pathKey - startCellKey) < minimumCellsFromStartCell)
        {
            Debug.Log(randomPointWithinPath + " start branch cell set too close to start of level");
            goto StartBranch;
        }
        //check neighbors and ensure that cell has at least one open neighbor
        //if no available neighbors, get new starting point
        for (int i = 0; i < directions.Count; i++)
        {
            Vector2 neighborPoint = randomPointWithinPath + directions[i];
            Debug.Log("getting " + randomPointWithinPath + " neighbors: " + neighborPoint);
            if (cellPath.ContainsValue(neighborPoint))
            {
                Debug.Log(neighborPoint + " exists on path");
                continue;
            }
            else if (!grid.points.Contains(neighborPoint))
            {
                Debug.Log(neighborPoint + " is out of bounds");
                continue;
            }
            else
            {
                neighbors.Add(neighborPoint);
                neighborDirection.Add(directions[i]);
            }
        }
        if (neighbors.Count <= 0)
        {
            Debug.Log("cell is surrounded on all sides, get new starting point");
            goto StartBranch;
        }
        GameObject rootCell = activeCells[pathKey];
        int randomNeighborIndex = Random.Range(0, neighbors.Count-1);
        Vector2 nextPoint = neighbors[randomNeighborIndex];

        pathIndex++;
        cellPath.Add(pathIndex, nextPoint);
        GameObject cell = NewActiveCell();
        Debug.Log("!! starting branching path @ point: " + rootCell.name + " heading to: " + nextPoint + " " + cell.name); ;

        WallsManager cellOnPathWallManager = rootCell.GetComponent<WallsManager>();
        cellOnPathWallManager.DisableWallToTreasureRoom(neighborDirection[randomNeighborIndex]);
        
        WallsManager cellWalls = cell.GetComponent<WallsManager>();
        cellWalls.lastDirection = neighborDirection[randomNeighborIndex];
        cellWalls.isTreasureRoom = true;

        ColorManager colorManager = cell.GetComponent<ColorManager>();
        colorManager.spriteRenderer.color = Color.cyan;

        branchingPathLoopCount++;
        neighbors.Clear();
        neighborDirection.Clear();
        treasureRooms.Add(cell);

        if (branchingPathLoopCount >= desiredBranchingPaths)
        {
            CancelInvoke();
            GetRemainingGridProints();

            //place chest holding key inside the room
            int randomTreasureRoom = Random.Range(0, treasureRooms.Count - 1);
            treasureRooms[randomTreasureRoom].GetComponent<ColorManager>().spriteRenderer.color = Color.yellow;
        }
    }

    private void GetPointOnPath(out Vector2 randomPointWithinPath)
    {
        int randomXIndex = Random.Range(2, cellXPoint.Count - 2);
        int randomYIndex = Random.Range(2, cellYPoint.Count - 2);
        randomPointWithinPath = new(cellXPoint[randomXIndex], cellYPoint[randomYIndex]);
    }

    List<int> cellXPoint = new List<int>();
    List<int> cellYPoint = new List<int>();
    GameObject startCell;
    GameObject exitCell;
    private void SetStartAndEndCells()
    {
        List<Vector2> cellGridPoints = new List<Vector2>(cellPath.Values);
        for (int i = 0; i < cellGridPoints.Count; i++)
        {
            cellYPoint.Add((int)cellGridPoints[i].y);
            cellXPoint.Add((int)cellGridPoints[i].x);
        }
        int lowestCellYPoint = cellYPoint.Min();
        int highestCellYPoint = cellYPoint.Max();
        int lowestCellXPoint = cellXPoint.Min();
        int highestCellXPoint = cellXPoint.Max();

        Vector2 bottomLeftmostPointOnPath = new Vector2(lowestCellXPoint, lowestCellYPoint);
        Vector2 topRightmostPointOnPath = new Vector2(highestCellXPoint, highestCellYPoint);
        Vector2 bottomRightmostPointOnPath = new Vector2(highestCellXPoint, lowestCellYPoint);
        Vector2 topLeftmostPointOnPath = new Vector2(lowestCellXPoint, highestCellYPoint);

        List<GameObject> possibleStartCells = new List<GameObject>();
        foreach(GameObject cell in activeCells)
        {
            if (cell.transform.position.y != lowestCellYPoint) continue;
            possibleStartCells.Add(cell);
        }
        int randomIndex = Random.Range(0, possibleStartCells.Count - 1);
        possibleStartCells[randomIndex].tag = "Start";
        startCell = possibleStartCells[randomIndex];

        List<GameObject> possibleEndCells = new List<GameObject>(); 
        foreach(GameObject cell in activeCells)
        {
            if(cell.transform.position.y != highestCellYPoint) continue;
            possibleEndCells.Add(cell);
        }
        int randomEndCellIndex = Random.Range(0, possibleEndCells.Count - 1);
        possibleEndCells[randomEndCellIndex].tag = "Exit";
        exitCell = possibleEndCells[randomEndCellIndex];

        EventManager.instance.TriggerEvent("spawn_player");
        //get list of all cells on highest level of grid
        //randomly select one to be the last room
    }

    List<Vector2> gridPoints;
    private void GetRemainingGridProints()
    {
        gridPoints = new List<Vector2>(grid.points);
        points = new List<Vector2>(cellPath.Values);

        foreach (var cell in points)
        {
            if (gridPoints.Contains(cell))
            {
                gridPoints.Remove(cell);
            }
        }

        StartCoroutine(FillRemainingPointsOnGrid());
    }
    private IEnumerator FillRemainingPointsOnGrid()
    {
        int gridIndex = 0;
        int cellIndex = pathIndex+1;
        while (gridIndex < gridPoints.Count)
        {
            GameObject cell = cellObjectPool[cellIndex];
            cell.SetActive(true);
            cell.tag = "Filler";
            cell.name = "Empty Cell";
            WallsManager cellWalls = cell.GetComponent<WallsManager>();
            cellWalls.wallsDisabled = true;
            fillerCells.Add(cell);
            if (cellIndex < cellObjectPool.Count - 1)
            {
                cellIndex++;
            }
            if (gridIndex == gridPoints.Count) break;
            cell.transform.position = gridPoints[gridIndex];
            gridIndex++;
            yield return new WaitForSeconds(timeBetweenIteration);
        }
    }

    private void ResetNextPoint(Vector2 currentDirection, out Vector2 nextPoint, out int index, out Vector2 direction)
    {
        int invalidIndex = directions.IndexOf(currentDirection);
    GetNewDirection:
        int newIndex = Random.Range(0, directions.Count);
        if(newIndex == invalidIndex)
        {
            Debug.Log("invalid direction");
            goto GetNewDirection;
        }

        Vector2 newDirection = directions[newIndex];
        nextPoint = cellPath[pathIndex] + newDirection;
        index = newIndex;
        direction = newDirection;
        Debug.Log("new direction: " + direction + " " + directionNames[index]);
    }
    public void GetKeyFromValue(Dictionary<int, Vector2> dictionary, Vector2 value, out int key)
    {
        key = 0;
        foreach (int keyVar in dictionary.Keys)
        {
            if (dictionary[keyVar] == value)
            {
                key = keyVar;
            }
        }
    }
}