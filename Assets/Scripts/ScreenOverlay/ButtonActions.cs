using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{

    public MenuManager menuManager;
    public void RestartScene()
    {
        var index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index, LoadSceneMode.Single);

        menuManager.pauseGame = false;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Jogo fechou");
    }
}
