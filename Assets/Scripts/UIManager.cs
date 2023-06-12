using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UIManager : MonoBehaviour, IEventListener
{
    [SerializeField] GameEvent playerDeathEvent;
    [SerializeField] UnityEvent playerDeath;
    public static UIManager instance;
    public GameObject gameOverText;
    public TextMeshProUGUI currencyText;
    public PlayerManager playerManager;

    private void OnEnable()
    {
        instance = this;
        if(playerDeathEvent != null) playerDeathEvent.RegisterListener(this);
    }
    private void Update()
    {
        if(playerManager != null) currencyText.text = "Gems: " + playerManager.playerInventory.currentCurrency;
    }

    public void GameOverScreen()
    {
        gameOverText.SetActive(true);
    }
    private void OnDisable()
    {
        playerDeathEvent.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        Debug.Log("player death");
        playerDeath.Invoke();
    }
}
