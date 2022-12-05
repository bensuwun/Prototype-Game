using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScreenSwitcher : MonoBehaviour
{
    public void NewGame(int index)
    {
        DataManager.SaveLevel(1);
        DataManager.SaveDialogueIndex(0);
        SceneManager.LoadScene(sceneBuildIndex: index);
    }
    public void OpenScene(int index)
    {
        SceneManager.LoadScene(sceneBuildIndex: index);
    }

    public void ExitGame ()
    {
        Debug.Log("EXIT");
        Application.Quit();
    }
}
