using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FadeOutSound {
    public static IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0.1) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
 
            yield return null;
        }
 
        audioSource.Stop ();
        audioSource.volume = startVolume;
        SceneManager.LoadScene("StoryScene");
    }
}
