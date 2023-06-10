using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int testCurrency;
    int playerCurrency;
    public int currentCurrency;
    private void Awake()
    {
        playerCurrency = testCurrency;

        EventManager.instance.AddListener("increase_currency", IncreaseCurrency());
        EventManager.instance.AddListener("decrease_currency", DecreaseCurrency());
    }

    private UnityAction IncreaseCurrency()
    {
        UnityAction action = () =>
        {
            playerCurrency++;
        };
        return action;
    }

    private UnityAction DecreaseCurrency()
    {
        UnityAction action = () =>
        {
            playerCurrency--;
        };
        return action;
    }
    public void HandleInventory()
    {
        currentCurrency = playerCurrency;
    }
}
