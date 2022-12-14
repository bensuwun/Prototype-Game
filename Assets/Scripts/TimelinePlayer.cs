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
        PlayableDirector previousTimeline;
        // directorsIndex = CutsceneDict.cutsceneMarker[marker];
        bool haveCutscene = CutsceneDict.cutsceneMarker.TryGetValue(marker, out directorsIndex);
        if(directorsIndex < directors.Count && haveCutscene){
            // Debug.Log(directorsIndex);
            // button.interactable = false;
            previousTimeline =  directorsIndex > 1 ? directors[directorsIndex-1] : null;
               
            if(previousTimeline != null && previousTimeline.state == PlayState.Playing){
                Debug.Log("Pause");
                // previousTimeline.Pause();
            }
            directors[directorsIndex].Play();
        }

    }

    void Awake()
    {
        // int level = DataManager.GetLevel();
        int level = DataManager.GetLevel();
        Debug.Log(string.Format("level: {0}", level));
        if(level == 1){
            PlayTimeline("Intro");
        }else if(level == 2){
            PlayTimeline("PostTutorial");
        }else if(level == 3){
            PlayTimeline("PostAmogus");
        }else if(level == 4){
            PlayTimeline("PostEEmagres");
        }else if(level == 5){
            PlayTimeline("PostEE2magres");
        }

        testCutscenes();
    }

    void testCutscenes(){
        // directors[19].Play();
    }
}
