using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnUnits : MonoBehaviour, IEventListener
{
    [SerializeField] GameEvent levelGenerationComplete;
    [SerializeField] UnityEvent spawnPlayer;

    public GameObject playerPrefab;
    GameObject player;
    bool playerSpawned = false;
    LoopEraseRandomWalk levelGenerator;
    public Transform spawnPosition;
    void Awake()
    {
        levelGenerator = GetComponent<LoopEraseRandomWalk>();
    }

    public void GetStartingRoom()
    {
        if(levelGenerator == null) spawnPosition = GameObject.FindGameObjectWithTag("Start").transform;
    }
    public void SpawnPlayer()
    {
        if (levelGenerator == null)
        {
            player.transform.position = spawnPosition.position;
            return;
        }

        foreach (GameObject cell in levelGenerator.activeCells.Values)
        {
            if (cell.tag == "Start" && !playerSpawned)
            {
                player = Instantiate(playerPrefab,cell.transform.position, Quaternion.identity);
                playerSpawned = true;
            }
        }
    }

    public void OnEventRaised(GameEvent gameEvent)
    {
        spawnPlayer.Invoke();
    }

    private void OnEnable()
    {
        levelGenerationComplete.RegisterListener(this);
    }
    private void OnDisable()
    {
        levelGenerationComplete.UnregisterListener(this);
    }
}
