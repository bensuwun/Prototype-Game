using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleSoundManager : MonoBehaviour{
    public AudioSource BGMAudioSource;
    public AudioSource SFXComboAudioSource; // For combos
    public AudioSource SFXOtherAudioSource; // For low hp and round end sfx 
    public List<AudioClip> BGMAudioClips;
    public List<AudioClip> SFXCombos;
    public List<AudioClip> SFXRoundEndClips;
    public AudioClip SFXLowHP;

    
    // Play background music in battle scene
    public void PlayBGM(int level) {
        if (level >= 1 && level <= 3) {
            int clipIndex = level - 1;
            BGMAudioSource.loop = true;
            BGMAudioSource.clip = BGMAudioClips[clipIndex];
            BGMAudioSource.Play();
        }   
    }

    // Play combo SFX in battle scene (1-5)
    public void PlaySFXCombo(int comboNumber) {
        if (comboNumber >= 1 && comboNumber <= 5) {
            int clipIndex = comboNumber - 1;
            Debug.Log("Playing sound number " + clipIndex);
            SFXComboAudioSource.PlayOneShot(SFXCombos[clipIndex]);
        }
    }

    // Play low HP SFX (loop) when player's health is low
    public void PlaySFXLowHP() {
        SFXOtherAudioSource.clip = SFXLowHP;
        SFXOtherAudioSource.loop = true;
        SFXOtherAudioSource.Play();
    }

    // Post-battle BGM
    public void PlayBGMResult(string result, bool endBGM = true) {
        if (endBGM) {
            StopBGM();
        }
        
        StopSFXLowHP();
        if (result.Equals("VICTORY")) {
            BGMAudioSource.loop = true;
            BGMAudioSource.clip = SFXRoundEndClips[0];
            BGMAudioSource.Play();
        }
        else if (result.Equals("DEFEAT")) { 
            BGMAudioSource.PlayOneShot(SFXRoundEndClips[1]);
        }
        else {
            Debug.LogWarning(String.Format("[WARNING]: Invalid parameter '{0}' passed to BattleSoundManager.PlayBGMResult()"
            , result));
        }
    }

    public void StopSFXLowHP() {
        SFXOtherAudioSource.Stop();
    }

    public void StopBGM() {
        BGMAudioSource.Stop();
    }
}