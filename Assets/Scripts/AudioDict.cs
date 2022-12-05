using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDict 
{
    public static Dictionary<string,int> audioMarker = new Dictionary<string, int>(){
        {"Intro", 0}, // first area
        {"What is this thing?", 1}, // rainy
        {"my name is Emagres, pleased to meet you.", 2}, // ivy
        {"  ", 3}, //2010
        {"   ", 4}, // library
        {"PostAmogus", 5}, // 2030
        {"        ", 6}, //ending
    };
}
