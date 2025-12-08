using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool pauseGame = false;
    public GameObject gameOverMenu;

    public Player player;

    public void Update()
    {
        if(pauseGame != true)
        {
            Time.timeScale = 1f;
            Debug.Log("Game ongoing");

        }
    }
    public void OpenGameOverScreen()
    {
        player.setPoint();
        Debug.Log("score atualizado");
        
        pauseGame = true;
        Debug.Log("Game Pause");
        Time.timeScale = 0f;
        gameOverMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
