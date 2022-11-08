using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WordBank : MonoBehaviour
{
    public TextAsset file = null;
    private List<string> originalWords = new List<string>();

    private List<string> workingWords = new List<string>();

    private void Awake() {
        ReadFile();
        workingWords.AddRange(originalWords);
        ConvertToLower(workingWords);
        Shuffle(workingWords);
    }

    private void ReadFile() {
        var splitFile = new string[] {"\r\n", "\r", "\n"};
        var words = file.text.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < words.Length; i++) {
            // print("word " + i + ": " + words[i]);
            originalWords.Add(words[i]);
        }
    }

    private void Shuffle(List<string> list) {
        for (int i = 0; i < list.Count; i++) {
            int random = Random.Range(i, list.Count);
            string temp = list[i];

            list[i] = list[random];
            list[random] = temp;
        }
    }

    private void ConvertToLower(List<string> list) {
        for(int i = 0; i < list.Count; i++) {
            list[i] = list[i].ToLower();
        }
    }

    public string GetWords() {
        string newWord = string.Empty;
        string newWords = string.Empty;

        for (int i = 0; i < 10; i++) {
            if (workingWords.Count != 0) {
                newWord = workingWords.Last();
                workingWords.Remove(newWord);
                newWords += newWord;
                newWords += " ";
            }
        }
        return newWords;
    }
}
