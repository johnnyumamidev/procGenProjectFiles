using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnUnits : MonoBehaviour, IEventListener
{
    [SerializeField] GameEvent levelGenerationComplete;
    [SerializeField] UnityEvent setSpawnPosition;

    [SerializeField] CyclicLevelGenerator levelGenerator;
    public GameObject playerPrefab;
    public Transform spawnPosition;
    public void GetStartingRoom()
    {
        GameObject startCellLayout = levelGenerator.startCell.GetComponent<CellLayout>().elevatorEnter;
        Transform parent = startCellLayout.transform;

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform elevator = parent.GetChild(i);
            if (elevator.CompareTag("Elevator"))
            {
                for (int j = 0; j < elevator.childCount; j++)
                {
                    Transform spawnPoint = elevator.GetChild(j);
                    if (spawnPoint.CompareTag("PlayerSpawn"))
                    {
                        spawnPosition = spawnPoint;
                    }
                }
            }
        }
    }
    public void SpawnPlayer()
    {
        Instantiate(playerPrefab, spawnPosition.position, Quaternion.identity);
    }

    public void OnEventRaised(GameEvent gameEvent)
    {
        if(gameEvent == levelGenerationComplete)
        {
            Debug.Log("** level generation complete **");
            setSpawnPosition?.Invoke();
        }
    }

    private void OnEnable()
    {
        levelGenerationComplete?.RegisterListener(this);
    }
    private void OnDisable()
    {
        levelGenerationComplete?.UnregisterListener(this);
    }
}
