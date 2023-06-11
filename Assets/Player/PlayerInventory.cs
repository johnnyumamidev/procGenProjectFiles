using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour, IEventListener
{
    [SerializeField] GameEvent shopEvent;
    [SerializeField] UnityEvent shopResponse;
    [SerializeField] GameEvent subtractCurrencyEvent;
    [SerializeField] UnityEvent subtractCurrency;

    public int testCurrency;
    int playerCurrency;
    public int currentCurrency;

    private void OnEnable()
    {
        if(shopEvent != null)
            shopEvent.RegisterListener(this);
        if(subtractCurrencyEvent != null) 
            subtractCurrencyEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        shopEvent.UnregisterListener(this);
        subtractCurrencyEvent.UnregisterListener(this);
    }

    public void OnEventRaised(GameEvent gameEvent)
    {
        if(gameEvent == shopEvent)
            shopResponse.Invoke();
        if (gameEvent == subtractCurrencyEvent)
            subtractCurrency.Invoke();
    }

    private void Awake()
    {
        playerCurrency = testCurrency;
    }

    public void UpdateCurrency(int value)
    {
        playerCurrency += value;
    }

    public void HandleInventory()
    {
        currentCurrency = playerCurrency;
        currentCurrency = Mathf.Clamp(playerCurrency, 0, currentCurrency);
    }

    public void SetItemSlot(ShopSlot itemSlot)
    {
        shopResponse.RemoveAllListeners();
        shopResponse.AddListener(itemSlot.CheckPlayerCurrency(this));
    }
}
