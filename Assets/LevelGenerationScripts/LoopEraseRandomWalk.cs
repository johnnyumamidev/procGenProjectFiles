using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class LoopEraseRandomWalk : MonoBehaviour
{
    Grid grid;
    public GameObject cellPrefab;

    List<Vector2> directions = new List<Vector2> { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    List<string> directionNames = new List<string> { "North", "East", "South", "West" };
    Vector2 lastDirection;

    public Vector2 targetCell;
    Dictionary<int, Vector2> cellPath = new Dictionary<int, Vector2>();
    public List<Vector2> cellPoints;
    List<GameObject> treasureRooms = new List<GameObject>();
    public List<GameObject> cellObjectPool = new List<GameObject>();
    public Dictionary<Vector2, GameObject> activeCells = new Dictionary<Vector2, GameObject>();
    public List<GameObject> fillerCells = new List<GameObject>();
    [SerializeField] int pathIndex = 0;
    public float timeBetweenIteration;

    public int numberOfLoops;
    public int pathLength;
    public bool pathActive;

    [SerializeField] GameEvent walkCompleteEvent;

    public GameObject emptyCell;

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
        }

        GameObject cell = NewActiveCell();
        Vector2 cellPoint = cellPath[pathIndex];
        Debug.Log("cell#: " + pathIndex + " @ "+ cellPoint);
        
        for(int i = 0; i < directions.Count; i++)
        {
            Vector2 neighbor = cellPoint + directions[i];
            if (!grid.points.Contains(neighbor))
            {
                Debug.Log(neighbor + " goes out of bounds/ " + i);
                continue;
            }
            else if (directions[i] == -lastDirection)
            {
                Debug.Log(neighbor + " backpedals/ " + i);
                continue;
            }
            else if (directions[i] == lastDirection)
            {
                continue;
            }
            else
            {
                neighbors.Add(neighbor);
                neighborDirection.Add(directions[i]);
                Debug.Log("adding " + neighbor + " to possible neighbors/ " + i);
            }
        }
        if(neighbors.Count <= 0)
        {
            Debug.LogError("no valid direction");
        }
        int randomNeighborIndex = Random.Range(0, neighbors.Count);
        Vector2 nextPoint = neighbors[randomNeighborIndex];
        Vector2 randomDirection = neighborDirection[randomNeighborIndex];
        if (cellPath.ContainsValue(nextPoint))
        {
            GetKeyFromValue(cellPath, nextPoint, out int gridKey);
            Debug.Log("loop intersection at " + cell.name + " and Cell# " + gridKey + ", erasing loop");
            Debug.Log("new start point: " + gridKey);
            for (int i = gridKey; i <= pathIndex; i++)
            {
                cellObjectPool[i].SetActive(false);
                activeCells.Remove(cellPath[i]);
                cellPath.Remove(i);
                Debug.Log("destroying: " + cellObjectPool[i].name);
            }

            pathIndex = gridKey - 1;
            numberOfLoops++;
        }
        lastDirection = randomDirection;
        DisableCellWalls(cell, randomDirection);
        neighbors.Clear();
        neighborDirection.Clear();

        if (pathIndex + 1 < pathLength)
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
        activeCells.Add(cellPath[pathIndex], cell);
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

    private void GetRandomDirection(out Vector2 randomDirection)
    {
        int randomDirectionIndex = Random.Range(0, directions.Count - 1);
        randomDirection = directions[randomDirectionIndex];
    }

    List<Vector2> points;
    int branchingPathLoopCount;
    int branchingPathIndex;
    public int desiredBranchingPaths = 1;
    public int minimumCellsFromStartCell = 4;
    List<Vector2> neighbors = new List<Vector2>();
    List<Vector2> neighborDirection = new List<Vector2>();
    List<Vector2> branchingCellPoints = new List<Vector2>();
    private void CreateBranchingPaths()
    {
        List<Vector2> availablePoints = new List<Vector2>(cellPath.Values);
    GetRootCell:
        neighbors.Clear();
        neighborDirection.Clear();
        int random = Random.Range(0, availablePoints.Count);
        Vector2 randomPointWithinPath = availablePoints[random];
        GetKeyFromValue(cellPath, randomPointWithinPath, out int pathKey);
        GetKeyFromValue(cellPath, startCell.transform.position, out int startKey);
        GameObject rootCell = cellObjectPool[pathKey];

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
            else if(neighborPoint.y > cellYPoint.Max())
            {
                Debug.Log("neighbor is above the exit room");
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
            Debug.Log(rootCell.name + " is surrounded on all sides, get new starting point");
            goto GetRootCell;
        }
        else if(Mathf.Abs(pathKey - startKey) < minimumCellsFromStartCell)
        {
            Debug.Log(rootCell.name + " too close to starting room");
            goto GetRootCell;
        }


        rootCell.name += " branch";
        int randomNeighborIndex = Random.Range(0, neighbors.Count-1);
        Vector2 nextPoint = neighbors[randomNeighborIndex];
        availablePoints.Remove(randomPointWithinPath);

        pathIndex++;
        cellPath.Add(pathIndex, nextPoint);
        GameObject cell = NewActiveCell();
        Debug.Log("!! starting branching path @ point: " + rootCell.name + " heading to: " + nextPoint + " " + cell.name); ;
        cell.name = cell.name + " Treasure Room";
        cell.tag = "TreasureRoom";

        ColorManager colorManager = cell.GetComponent<ColorManager>();
        colorManager.spriteRenderer.color = Color.cyan;

        branchingPathLoopCount++;
        branchingCellPoints.Add(randomPointWithinPath);
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
        int randomXIndex = Random.Range(1, cellXPoint.Count - 1);
        int randomYIndex = Random.Range(1, cellYPoint.Count - 1);
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
        foreach (GameObject cell in activeCells.Values)
        {
            if (cell.transform.position.y != lowestCellYPoint) continue;
            possibleStartCells.Add(cell);
        }
        int randomIndex = Random.Range(0, possibleStartCells.Count - 1);
        possibleStartCells[randomIndex].tag = "Start";
        startCell = possibleStartCells[randomIndex];

        List<GameObject> possibleEndCells = new List<GameObject>();
        foreach (GameObject cell in activeCells.Values)
        {
            if (cell.transform.position.y != highestCellYPoint) continue;
            possibleEndCells.Add(cell);
        }
        int randomEndCellIndex = Random.Range(0, possibleEndCells.Count - 1);
        exitCell = possibleEndCells[randomEndCellIndex];
        exitCell.name = "Exit";
        exitCell.tag = "Exit";

        //get list of all cells on highest level of grid
        //randomly select one to be the last room
    }

    List<Vector2> gridPoints;
    private void GetRemainingGridProints()
    {
        points = new List<Vector2>(cellPath.Values);
        gridPoints = new List<Vector2>(grid.points);

        foreach (var cell in points)
        {
            if (gridPoints.Contains(cell))
            {
                gridPoints.Remove(cell);
            }
        }

        AssignCellsToFloorLevel();
        StartCoroutine(FillRemainingPointsOnGrid());
    }

    private void AssignCellsToFloorLevel()
    {
        foreach(GameObject cell in activeCells.Values)
        {

        }
    }

    private IEnumerator FillRemainingPointsOnGrid()
    {
        int gridIndex = 0;
        int cellIndex = pathIndex+1;
        while (gridIndex < gridPoints.Count)
        {
            GameObject cell = Instantiate(emptyCell);
            cell.SetActive(true);
            cell.tag = "Filler";
            cell.name = "Empty Cell";
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

        if(gridIndex >= gridPoints.Count)
        {
            Debug.Log("level generation complete !!");
            walkCompleteEvent.Raise();
        }
    }

    private void ResetNextPoint(Vector2 currentDirection, out Vector2 nextPoint, out Vector2 direction)
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
        direction = newDirection;
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