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
    [SerializeField] GameEvent playerSpawnEvent;
    [SerializeField] GameEvent shopkeepEvent;
    [SerializeField] UnityEvent shopkeep;

    public Button retryButton;

    public static UIManager instance;
    public GameObject gameOverText;
    public TextMeshProUGUI currencyText;
    public PlayerManager playerManager;
    private void Awake()
    {
        retryButton.onClick.AddListener(RaiseSpawnEvent);
    }
    private void Update()
    {
        if (playerManager != null) currencyText.text = "Gems: " + playerManager.playerInventory.currentCurrency;
        CloseMenu();
    }

    public void GameOverScreen()
    {
        gameOverText.SetActive(true);
    }
    private void OnEnable()
    {
        instance = this;
        if(playerDeathEvent != null) playerDeathEvent.RegisterListener(this);
        shopkeepEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        playerDeathEvent.UnregisterListener(this);
        shopkeepEvent.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        Debug.Log("player death");
        if (gameEvent == playerDeathEvent) playerDeath.Invoke();
        if (gameEvent == shopkeepEvent) shopkeep.Invoke();
    }
    private void RaiseSpawnEvent()
    {
        playerSpawnEvent.Raise();
        GameStateManager.instance.currentState = "In Progress";
        gameOverText.SetActive(false);
    }
    public void CloseMenu()
    {
        if (playerManager.playerInput.performCancel != 0)
        {
            Debug.Log("close menu");
            if(itemsDisplay != null) itemsDisplay.SetActive(false);
        }
    }
    public GameObject itemsDisplay;
    public void DisplayItems()
    {
        if (itemsDisplay != null)
        {
            itemsDisplay.SetActive(true);
        }
    }
}
