using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource BGMAudioSource;
    public List<AudioClip> BGMAudioClips;

    
    // Play background music in battle scene
    public void PlayBGM(string marker) {
        int idxAudio = 0;
        Debug.Log(string.Format("Audio Mark: {0}",marker));
        bool hasBGM = AudioDict.audioMarker.TryGetValue(marker, out idxAudio);
        if(hasBGM && idxAudio < BGMAudioClips.Count){
            StopBGM();
            BGMAudioSource.clip = BGMAudioClips[idxAudio];
            BGMAudioSource.loop = true;
            BGMAudioSource.Play();
        }
    }
    public void StopBGM() {
        BGMAudioSource.Stop();
    }
    void Awake()
    {
        // int level = DataManager.GetLevel();
        int level = DataManager.GetLevel();
        Debug.Log(string.Format("level: {0}", level));
        if(level == 1){
            PlayBGM("Intro");
        }else if(level == 3){
            PlayBGM("PostAmogus");
        }
    }
}
