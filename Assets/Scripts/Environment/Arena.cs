using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Arena : MonoBehaviour, IEventListener
{
    [SerializeField] GameEvent combatCompleteEvent;
    [SerializeField] UnityEvent combatComplete;
    [SerializeField] GameEvent enemyDefeatedEvent;
    [SerializeField] UnityEvent enemyDefeated;
    [SerializeField] GameEvent enemySpawnedEvent;
    [SerializeField] UnityEvent enemySpawned;
    public List<Transform> enemySpawnPoints = new List<Transform>();
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public GameObject chestPrefab;
    public Transform rewardSpawnPoint;
    public GameObject enemy;
    private void OnEnable()
    {
        combatCompleteEvent?.RegisterListener(this);
        enemyDefeatedEvent?.RegisterListener(this);
    }
    private void OnDisable()
    {
        combatCompleteEvent?.UnregisterListener(this);
        enemyDefeatedEvent?.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        if (gameEvent == combatCompleteEvent) combatComplete?.Invoke();
        if (gameEvent == enemyDefeatedEvent) enemyDefeated?.Invoke();
    }

    void Start()
    {
        enemy = Instantiate(enemyPrefabs[0], enemySpawnPoints[0].position, Quaternion.identity);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            combatCompleteEvent?.Raise();
        }
    }
    public void SpawnChest()
    {
        chestPrefab.SetActive(true);
        chestPrefab.transform.position = rewardSpawnPoint.position;
    }
}
