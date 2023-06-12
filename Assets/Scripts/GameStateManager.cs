using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour, IEventListener
{
    [SerializeField] GameEvent playerDeathEvent;
    [SerializeField] UnityEvent playerDeath;

    public static GameStateManager instance;
    public string currentState;
   
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    void Start()
    {
        StartGameState();
    }
    private void StartGameState()
    {
        UpdateGameState("In Progress");
    }
    public void UpdateGameState(string index)
    {
        currentState = index;
    }
    public void Retry()
    {
        StartGameState();
    }
    public void MainMenu()
    {
        Debug.Log("go to main menu");
        StartGameState();
    }
    private void OnEnable()
    {
        playerDeathEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        playerDeathEvent.UnregisterListener(this);   
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        playerDeath?.Invoke();
    }
}
