using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour
{
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

    private void Update()
    {
        HandleGameStates();
    }

    private void HandleGameStates()
    {

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

}
