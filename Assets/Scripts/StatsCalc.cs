using System;
using System.Collections.Generic;
using UnityEngine;
public class StatsCalc : MonoBehaviour
{
    public float start;
    public float end;
    
    // gets the start time
    public void getStart() {
        start = Time.time;
    }
    // gets the end time
    public void getEnd() {
        end = Time.time;
    }
    // calculates the current WPM
    public string getCurrWPM(int numCorrectChars, int numSpace) {
        getEnd();
        float currDiff = end - start;
        // removed numspace in the computation
        // double currWPM = Math.Floor(((numCorrectChars + numSpace) * (60 / currDiff) / 5));
        double currWPM = Math.Floor(((numCorrectChars + numSpace) * (60 / currDiff) / 5));
        return("" + currWPM);
    } 
    // calculates raw WPM (including mistakes)
    public string getRawWPM(int numCharsTyped, int numSpace) {
        float currDiff = end - start;
        double rawWPM = Math.Floor(((numCharsTyped + numSpace) * (60 / currDiff) / 5));
        return("" + rawWPM);
    } 
}