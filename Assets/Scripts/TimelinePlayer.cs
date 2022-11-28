using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelinePlayer : MonoBehaviour
{
    public List<PlayableDirector> directors;
    private int directorsIndex = 0;
    public void StartTimeline()
    {
        Debug.Log(directorsIndex);
        directors[directorsIndex].Play();
        directorsIndex++;
    }

    void Awake()
    {
        StartTimeline();
    }
}
