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
}