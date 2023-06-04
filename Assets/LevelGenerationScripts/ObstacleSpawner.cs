using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public List<Transform> wallSpawnPoints = new List<Transform>();
    public GameObject wallTiles;
    public void SpawnTiles()
    {
        foreach (Transform tile in wallSpawnPoints)
        {
            Instantiate(wallTiles, tile.position, Quaternion.identity, transform);
        }
    }
}
