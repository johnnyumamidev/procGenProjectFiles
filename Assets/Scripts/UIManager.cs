using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverText;

    private void Update()
    {
        gameOverText.SetActive(false);

        if (GameStateManager.instance.currentState == "Game Over")
        {
            gameOverText.SetActive(true);
        }
    }
}
