using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TimelinePlayer : MonoBehaviour
{
    public List<PlayableDirector> directors;
    private int directorsIndex = 0;
    public Button button;
    public void PlayTimeline(string marker)
    {
        // directorsIndex = CutsceneDict.cutsceneMarker[marker];
        bool haveCutscene = CutsceneDict.cutsceneMarker.TryGetValue(marker, out directorsIndex);
        if(directorsIndex < directors.Count && haveCutscene){
            // Debug.Log(directorsIndex);
            button.interactable = false;
            directors[directorsIndex].Play();
        }
    }

    void Awake()
    {
        PlayTimeline("Intro");
    }
}
