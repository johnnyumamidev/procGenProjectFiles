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

    private void OnEnable()
    {
        instance = this;
        if (gameOverText == null) return;
        gameOverText.SetActive(false);
    }
    private void Update()
    {
        if(playerManager != null) currencyText.text = "Gems: " + playerManager.playerInventory.currentCurrency;
    }

    public void GameOverScreen()
    {
        gameOverText.SetActive(true);
    }
}
