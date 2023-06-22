using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }
}
