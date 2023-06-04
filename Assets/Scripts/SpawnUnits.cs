using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnUnits : MonoBehaviour
{
    public GameObject playerPrefab;
    GameObject player;
    bool playerSpawned = false;
    LoopEraseRandomWalk levelGenerator;
    public Transform spawnPosition;
    void Awake()
    {
        player = Instantiate(playerPrefab, spawnPosition.transform.position, Quaternion.identity);
        levelGenerator = GetComponent<LoopEraseRandomWalk>();
        EventManager.instance.AddListener("spawn_player", SpawnPlayer());
    }

    private void PlayerSpawn()
    {
        player.transform.position = spawnPosition.position;
    }

    private UnityAction SpawnPlayer()
    {
        UnityAction action = () =>
        {
            if (levelGenerator == null)
            {
                PlayerSpawn();
                return;
            }

            foreach (GameObject cell in levelGenerator.activeCells)
            {
                if(cell.tag == "Start" && !playerSpawned)
                {
                    player.transform.position = cell.transform.position;
                    playerSpawned = true;
                }
            }
        };
        return action;
    }
}
