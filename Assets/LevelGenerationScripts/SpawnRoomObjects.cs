using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnRoomObjects : MonoBehaviour
{
    public List<GameObject> outerWalls;
    public List<Transform> spawnPoints;
    public GameObject spawnObject;
    public RoomType roomType;
    public GameObject destructibleObject;
    public GameObject blankTile;
    public GameObject obstaclesParent;
    public List<GameObject> roomLayouts = new List<GameObject>();
    public float obstacleSpawnChance = 0.5f;

    List<GameObject> innerWalls = new List<GameObject>();
    List<GameObject> spawnedTiles = new List<GameObject>();
    public int emptyTileCount = 50;
    public LayerMask groundLayer;

    public enum RoomType
    {
        NORTH, EAST, SOUTH, WEST,
        NORTHEAST, NORTHWEST, SOUTHEAST, SOUTHWEST,
        CENTER
    }

    void Start()
    {
        CheckRoomType();
        AddSpawnPoints();
        SpawnWallTiles();
        SpawnAllRoomTiles();
    }

    private void SpawnAllRoomTiles()
    {
        Transform[] tiles = obstaclesParent.GetComponentsInChildren<Transform>();
        foreach (Transform t in tiles)
        {
            if (!t.CompareTag("Wall"))
            {
                GameObject obstacle = Instantiate(spawnObject, t.position, Quaternion.identity, gameObject.transform);
                SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();
                sr.color = Color.green;
                spawnedTiles.Add(obstacle);
            }
        }
        foreach (GameObject tile in innerWalls)
        {
            Transform[] points = tile.GetComponentsInChildren<Transform>();
            foreach (Transform t in points)
            {
                if (!t.CompareTag("Wall"))
                {
                    GameObject obstacle = Instantiate(spawnObject, t.position, Quaternion.identity, gameObject.transform);
                    SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();
                    sr.color = Color.green;
                    spawnedTiles.Add(obstacle);
                }
            }
        }
        //walk algorithm
        RemoveTiles();
    }

    private void RemoveTiles()
    {
        int randomStartTile = Random.Range(0, spawnedTiles.Count);
        Destroy(spawnedTiles[randomStartTile]);
        spawnedTiles.Remove(spawnedTiles[randomStartTile]);
    }
    private void SpawnObstacles()
    {
        int roomNumber = Random.Range(0, roomLayouts.Count);
        Transform[] _obstaclePoints = roomLayouts[roomNumber].GetComponentsInChildren<Transform>();
        foreach (Transform t in _obstaclePoints)
        {
            if (!t.CompareTag("Wall"))
            {
                int random = Random.Range(0, 10);
                if (random < 3)
                {
                    Instantiate(destructibleObject, t.position, Quaternion.identity, gameObject.transform);
                }
                else if(random > 3 && random < 8)
                {
                    Instantiate(blankTile, t.position, Quaternion.identity, gameObject.transform);
                }
                else
                {
                    GameObject obstacle = Instantiate(spawnObject, t.position, Quaternion.identity, gameObject.transform);
                    SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();
                    sr.color = Color.green;
                }
            }
        }
    }

    private void SpawnWallTiles()
    {
        foreach (Transform t in spawnPoints)
        {
            Instantiate(spawnObject, t.position, Quaternion.identity, gameObject.transform);
        }
    }

    private void AddSpawnPoints()
    {
        foreach (GameObject wall in outerWalls)
        {
            Transform[] activeWallSpawnPoints = wall.GetComponentsInChildren<Transform>();
            foreach (Transform t in activeWallSpawnPoints)
            {
                if(!t.CompareTag("Wall")) spawnPoints.Add(t);
            }
        }
    }

    private void CheckRoomType()
    {
        List<GameObject> wallChild = new List<GameObject>(); // indexes: 0=N,1=E,2=S,3=W
        foreach (Transform t in gameObject.GetComponentsInChildren<Transform>())
        {
            if (t.CompareTag("Wall")) wallChild.Add(t.gameObject);
        }

        if (roomType == RoomType.NORTHWEST)
        {
            outerWalls.Add(wallChild[0]);
            outerWalls.Add(wallChild[3]);
            innerWalls.Add(wallChild[2]);
            innerWalls.Add(wallChild[1]);
        }
        if (roomType == RoomType.NORTHEAST)
        {
            outerWalls.Add(wallChild[0]);
            outerWalls.Add(wallChild[1]);
            innerWalls.Add(wallChild[2]);
            innerWalls.Add(wallChild[3]);   
        }
        if (roomType == RoomType.SOUTHEAST)
        {
            outerWalls.Add(wallChild[2]);
            outerWalls.Add(wallChild[1]);
            innerWalls.Add(wallChild[0]);
            innerWalls.Add(wallChild[3]);   
        }
        if (roomType == RoomType.SOUTHWEST)
        {
            outerWalls.Add(wallChild[2]);
            outerWalls.Add(wallChild[3]);
            innerWalls.Add(wallChild[0]);
            innerWalls.Add(wallChild[1]);
        }
        if (roomType == RoomType.NORTH)
        {
            outerWalls.Add(wallChild[0]);
            innerWalls.Add(wallChild[2]);
            innerWalls.Add(wallChild[1]);
            innerWalls.Add(wallChild[3]);
        }
        if (roomType == RoomType.EAST)
        {
            outerWalls.Add(wallChild[1]);
            innerWalls.Add(wallChild[2]);
            innerWalls.Add(wallChild[0]);
            innerWalls.Add(wallChild[3]);
        }
        if (roomType == RoomType.SOUTH)
        {
            outerWalls.Add(wallChild[2]);
            innerWalls.Add(wallChild[0]);
            innerWalls.Add(wallChild[1]);
            innerWalls.Add(wallChild[3]);
        }
        if (roomType == RoomType.WEST)
        {
            outerWalls.Add(wallChild[3]);
            innerWalls.Add(wallChild[2]);
            innerWalls.Add(wallChild[1]);
            innerWalls.Add(wallChild[0]);
        }
        if (roomType == RoomType.CENTER)
        {
            innerWalls.Add(wallChild[3]);
            innerWalls.Add(wallChild[2]);
            innerWalls.Add(wallChild[1]);
            innerWalls.Add(wallChild[0]);
        }
    }
}
