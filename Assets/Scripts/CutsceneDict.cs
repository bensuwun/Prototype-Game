using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneDict
{
    public static Dictionary<string,int> cutsceneMarker = new Dictionary<string, int>(){
        {"Intro", 0}, // intro
        {"If only mom and dad...", 1}, // trip
        {"What is this thing?", 2}, // portal
        {"my name is Emagres, pleased to meet you.", 3}, // ivy house
        {" ", 4}, // tutorial boss
        {"PostTutorial", 5}, // after tutorial
        {"  ", 6}, // 2010 transition
        {"   ", 7}, // library transition
        {"After examining the computer, he unplugs the keyboard that was connected before and plugs in his own keyboard.", 8}, // keyboard swap
        {"Anyway good luck on whatever you need. I’ll be leaving now.", 9}, // cameron vanish
        {"Is this real? I am not dreaming am I? I really traveled to the future.",10}, // amogus appears
        {"    ", 11}, // amogus fight
        {"PostAmogus", 12}, // after amogus fight 2030 Transition
        {"     ", 13}, // elder emagres fight
        {"PostEEmagres",14}, // after eemagres fight
        {"      ", 15}, // loss from scripted eemagres fight
        {"PostEE2magres",16}, // after scripted loss
        {"Trust me you will like it here. Take care.", 17}, // EEmagres portal out
        {"       ", 18}, // ECameron arrives
        {"Here let me help you up. Names Cameron by the way.", 19}, //Ecameron helps
        {"        ", 20}, // end scene
        {"Lovely, now you go ahead and practice more while I make up some tea.", 21}, // ivy vanish
        {"A typewriter, that thing in your bag.", 22} // device appear


    };
    
}
