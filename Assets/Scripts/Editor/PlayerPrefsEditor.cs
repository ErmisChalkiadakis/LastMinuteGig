using UnityEditor;
using UnityEngine;
public class PlayerPrefsEditor
{
    [MenuItem("PlayerPrefs/Clear Player Prefs")]
    static void ClearPrefs()
    {
        Debug.Log($"Deleted ALL player prefs.");
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("PlayerPrefs/Player Played Tutorial")]
    static void SetPlayerPlayedTutorial()
    {
        Debug.Log($"Setting player played tutorial.");
        PlayerPrefs.SetInt("PlayedTutorial", 1);
    }
}