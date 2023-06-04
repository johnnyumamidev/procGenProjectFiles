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
        EventManager.instance.AddListener("game_over", GameOverState());
        EventManager.instance.AddListener("stage_complete", StageCompleteState());
    }
    void Start()
    {
        StartGameState();
    }

    private void StartGameState()
    {
        UpdateGameState("In Progress");
        EventManager.instance.TriggerEvent("spawn_player");
    }

    private void Update()
    {
        HandleGameStates();
    }

    private void HandleGameStates()
    {
        if (currentState == "Game Over")
        {
            Debug.Log("player health = 0...GAME OVER");
        }
        if(currentState == "Complete")
        {
            Debug.Log("Stage Complete!");
        }
    }

    public void UpdateGameState(string index)
    {
        currentState = index;
    }

    private UnityAction GameOverState()
    {
        UnityAction action = () =>
        {
            UpdateGameState("Game Over");
        };
        return action;
    }
    private UnityAction StageCompleteState()
    {
        UnityAction action = () =>
        {
            UpdateGameState("Complete");
        };
        return action;
    }

    public void Retry()
    {
        EventManager.instance.TriggerEvent("retry");
        StartGameState();
    }

    public void MainMenu()
    {
        Debug.Log("go to main menu");
        EventManager.instance.TriggerEvent("retry");
        StartGameState();
    }
}
