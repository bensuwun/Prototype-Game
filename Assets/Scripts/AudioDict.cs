using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDict 
{
    public static Dictionary<string,int> audioMarker = new Dictionary<string, int>(){
        {"Intro", 0}, // first area
        {"What is this thing?", 1}, // streets
        {"  ", 2}, // lib
        {"PostAmogus", 3}, //2030
        {"        ", 4}, // ending
    };
}
