using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScreenSwitcher : MonoBehaviour
{
    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
