using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] CollectionSystem collectionSystem;
    [SerializeField] CameraManager camManager;
    [SerializeField] ClawMachine clawMachine;
    [SerializeField] ClawHandler clawHandler;


    // Update is called once per frame
    void Update()
    {
        if (collectionSystem.plushiesCollected >= 14)
        {
            WinGame();
        }

        if (enemyManager.travelPercentage >= 99.99f)
        {
            LoseGame();
        }
    }


    [SerializeField] GameObject winScreen;
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] GameObject loseScreen;
    public void WinGame()
    {
        winScreen.SetActive(true);
        clawMachine.enabled  = false;
        camManager.enabled   = false;
        enemyManager.enabled = false;
        clawHandler.enabled  = false;
        Cursor.lockState = CursorLockMode.None;
    }
    public void LoseGame()
    {
        loseScreen.SetActive(true);
        clawMachine.enabled  = false;
        camManager.enabled   = false;
        enemyManager.enabled = false;
        clawHandler.enabled  = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
