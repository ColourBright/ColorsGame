using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void StartLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void StartLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void Cool()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
