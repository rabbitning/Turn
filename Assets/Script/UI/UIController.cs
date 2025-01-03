using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : EffectByViewChange
{
    public void StartGame()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void ToStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
