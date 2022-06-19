using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void StartLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    // public void StartLevel2()
    // {
    //     
    // }

    public void Exit()
    {
        Application.Quit();
    }
}
