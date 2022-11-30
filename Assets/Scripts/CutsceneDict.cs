using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneDict
{
    public static Dictionary<string,int> cutsceneMarker = new Dictionary<string, int>(){
        {"Intro", 0}, // intro
        {"If only mom and dad...", 1}, // trip
        {"What is this thing?", 2}, // portal
        {"I’d appreciate that Miss Ivy, my name is Emagres, pleased to meet you.", 3}, // ivy house
        {" ", 4}, // tutorial boss
        {"PostTutorial", 5}, // after tutorial
        {"  ", 6}, // 2010 transition
        {"   ", 7}, // library transition
        {"After examining the computer, he unplugs the keyboard that was connected before and plugs in his own keyboard.", 8}, // keyboard swap
        {"Anyway good luck on whatever you need. I’ll be leaving now.", 9}, // cameron vanish
        {"Is this real? I am not dreaming am I? I really traveled to the future.",10}, // amogus appears
        {"    ", 11}, // amogus fight
        {"PostAmogus", 11}, // after amogus fight -- stopped here
        {"     ", 1}, // elder cameron fight
        {"      ", 1}, // loss from scripted cameron fight
        {"       ", 1}, // end scene

    };
    
}
