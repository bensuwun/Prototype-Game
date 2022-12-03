using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadNextScene : MonoBehaviour
{
    void OnEnable()
    {
        PlayBattleScene();
    }

    void PlayBattleScene(){
        // DataManager.SaveLevel(level);
        SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
    }
}
