using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject gameOverText;
    public TextMeshProUGUI currencyText;
    public PlayerManager playerManager;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if(playerManager != null) currencyText.text = "Gems: " + playerManager.playerInventory.currentCurrency;
        
        if (gameOverText == null) return;
        gameOverText.SetActive(false);

        if (GameStateManager.instance.currentState == "Game Over")
        {
            gameOverText.SetActive(true);
        }
    }
}
