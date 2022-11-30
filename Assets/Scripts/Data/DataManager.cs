using System.IO;
using UnityEngine;

public static class DataManager
{
    public static int GetLevel() {
        return PlayerPrefs.GetInt(DataKeys.Level, 1);
    }

    public static void SaveLevel(int level) {
        PlayerPrefs.SetInt(DataKeys.Level, level);
    }

    public static int GetDialogueIndex(){
        return PlayerPrefs.GetInt(DataKeys.dialogueIndex, 0);
    }

    public static void SaveDialogueIndex(int index){
        PlayerPrefs.SetInt(DataKeys.dialogueIndex, index);
    }
}