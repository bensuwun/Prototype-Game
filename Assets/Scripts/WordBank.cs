using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WordBank : MonoBehaviour
{

    public TextAsset file = null;
    // Original words are the words from the text file, working words are words passed to the game
    private List<string> originalWords = new List<string>();
    private List<string> workingWords = new List<string>();

    // reads file before the game starts
    private void Awake() {
        ReadFile();
        workingWords.AddRange(originalWords);
        ConvertToLower(workingWords);
        Shuffle(workingWords);
    }

    // Reads the file connected to the prefab
    private void ReadFile() {
        var splitFile = new string[] {"\r\n", "\r", "\n"};
        var words = file.text.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < words.Length; i++) {
            // print("word " + i + ": " + words[i]);
            originalWords.Add(words[i]);
        }
    }

    // shuffles the words
    private void Shuffle(List<string> list) {
        for (int i = 0; i < list.Count; i++) {
            int random = Random.Range(i, list.Count);
            string temp = list[i];

            list[i] = list[random];
            list[random] = temp;
        }
    }

    // converts words to lower case
    private void ConvertToLower(List<string> list) {
        for(int i = 0; i < list.Count; i++) {
            list[i] = list[i].ToLower();
        }
    }

    // gets wordCount words from the word bank and returns it as a string split by " "
    public string GetWords() {
        int wordCount = 6;
        string newWord = string.Empty;
        string newWords = string.Empty;

        for (int i = 0; i < wordCount; i++) {
            if (workingWords.Count == 0) {
                // working words is empty
                workingWords.AddRange(originalWords);
            }
            
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

// TestStats.calculateTestSeconds(performance.now())