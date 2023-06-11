using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnUnits : MonoBehaviour
{
    [SerializeField] GameEvent spawnPlayerEvent;
    [SerializeField] UnityEvent spawnPlayerResponse;

    public GameObject playerPrefab;
    GameObject player;
    bool playerSpawned = false;
    LoopEraseRandomWalk levelGenerator;
    public Transform spawnPosition;
    void Awake()
    {
        player = Instantiate(playerPrefab, spawnPosition.transform.position, Quaternion.identity);
        levelGenerator = GetComponent<LoopEraseRandomWalk>();
    }

    private void SpawnPlayer()
    {
        if (levelGenerator == null)
        {
            player.transform.position = spawnPosition.position;
            return;
        }

        foreach (GameObject cell in levelGenerator.activeCells)
        {
            if (cell.tag == "Start" && !playerSpawned)
            {
                player.transform.position = cell.transform.position;
                playerSpawned = true;
            }
        }
    }

    public void OnEventRaised()
    {
        spawnPlayerResponse.Invoke();
    }
}
