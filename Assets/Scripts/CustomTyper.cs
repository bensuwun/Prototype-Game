using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomTyper : MonoBehaviour
{
    // Current word output
    public TextMeshProUGUI wordOutput;

    // Word Bank
    public WordBank wordBank = null;

    private string sourceString = string.Empty;
    private List<Word> wordList = new List<Word>();
    private StringBuilder sb;

    // Indexing current word and current char
    private int wordIndex = 0;
    private int charIndex = 0;  // also represents number of typed characters
    private int caretPosition = 0;

    // Colors for correct, incorrect, default characters
    private string correctColor = "green";
    private string incorrectColor = "red";
    private string defaultColor = "#808080";

    // Enums
    private enum Enums {
        // Input Formats
        IncorrectInputFormat = 0,
        CorrectInputFormat = 1,
        BackspaceInputFormat = 2,   // For resetting to default color when backspace is pressed
    }

    // Start is called before the first frame update
    void Start() {
        SetCurrentWords();
    }

    // Sets the current words shown on screen
    private void SetCurrentWords() {
        // Get the words from the word bank
        sourceString = wordBank.GetWords();

        // Parse source string to list of words
        foreach(string str in sourceString.Split(" ")) {
            wordList.Add(new Word(str));
        }

        // Display source string in output text
        wordOutput.text = sourceString;

        sb = new StringBuilder(sourceString);
    }

    // Update is called once per frame
    void Update() {
        string inputString = Input.inputString;
        
        if (inputString.Length == 1) {
            switch (CheckInput(inputString[0])) {
                // Character - can further be correct, incorrect, or excess
                case 0:
                    EnterChar(inputString);
                    break;
                // Spacebar
                case 1:
                    EnterSpacebar();
                    break;
                
                // Backspace
                case 2:
                    EnterBackspace();
                    break;

            }
            // TODO: implement AreWordsComplete()
            // Check if the current words on the screen are already finished and set new words
            if (AreWordsComplete()) {
                SetCurrentWords();
            }
        } 
    }

    // TODO: Check whether or not the remaining words are 0
    // Checks if the current line is already finished, return true if done, false if not
    private bool AreWordsComplete() {
        // check length of the remaining line
        return false;
    }

    /**
        Returns the type of the given input
        0 - character
        1 - spacebar
        2 - backspace
    */
    int CheckInput(char input) {
        int type = 0;

        // spacebar
        if (input == ' ')
            type = 1;

        // backspace
        else if (input == '\b')
            type = 2;
    
        return type;
    }

    /**
        Returns colored character in rich text format
    */
    string FormatInput(string input, int mode) {
        if (mode == (int) Enums.CorrectInputFormat) {
            return String.Format("<color={0}>{1}</color>", correctColor, input);
        }
        else if (mode == (int) Enums.IncorrectInputFormat) {
            return String.Format("<color={0}>{1}</color>", incorrectColor, input);
        }
        else 
            return String.Format("<color={0}>{1}</color>", defaultColor, input);
    }

    /**
        Entered a character (A-Z).
        Checks if input is correct, incorrect, or an excess.
    */
    void EnterChar(string input) {
        Debug.Log(string.Format("Word Index: {0} | Char Index: {1} | Caret Position: {2}", 
            wordIndex, charIndex, caretPosition));

        // If current word input is already longer than actual word, show as incorrect
        if (wordList[wordIndex].IsFullyTyped()) {
            string formattedInput = FormatInput(input, (int) Enums.IncorrectInputFormat);
            sb.Insert(caretPosition, formattedInput);

            // Update word output
            wordOutput.text = sb.ToString();

            // Update caret position
            caretPosition += formattedInput.Length;

            // Update charIndex
            charIndex += 1;
        }

        // Check if input char is correct
        else if (input[0] == wordList[wordIndex].GetNextChar()) {
            // Replace character with green character
            string formattedInput = FormatInput(input, (int) Enums.CorrectInputFormat);
            sb.Remove(caretPosition, 1).Insert(caretPosition, formattedInput);

            // Update word output
            wordOutput.text = sb.ToString();

            // Update caret position
            caretPosition += formattedInput.Length;

            // Update char index and word properties
            charIndex += 1;
            wordList[wordIndex].nTyped += 1;
            wordList[wordIndex].nCorrect += 1;
        }

        // Incorrect
        else if (input[0] != wordList[wordIndex].GetNextChar()) {
            string origChar = wordList[wordIndex].Text[charIndex].ToString();
            string formattedInput = FormatInput(origChar, (int) Enums.IncorrectInputFormat);
            sb.Remove(caretPosition, 1).Insert(caretPosition, formattedInput);

            // Update word output
            wordOutput.text = sb.ToString();

            // Update caret position
            caretPosition += formattedInput.Length;

            // Update char index and word properties
            charIndex += 1;
            wordList[wordIndex].nTyped += 1;
        }
    }

    /**
        Whitespace - end of current word
        Rich Text Tag - current word (check for excess)
    */
    void EnterBackspace() {
        int nTag = 0;
        bool onCurrentWord = false;
        
        char? lastChar = null;
        // Backspacing current word, check for excess
        if (charIndex > 0) {
            onCurrentWord = true;
            
            if (charIndex <= wordList[wordIndex].Text.Length)
                lastChar = wordList[wordIndex].GetPrevChar();
        }

        if (onCurrentWord) {
            // Current Implementation: Mid-Word
            while (nTag != 2) {
                caretPosition -= 1;
                if (sb[caretPosition] == '<') {
                    nTag++;
                }
                sb.Remove(caretPosition, 1);
            }
            
            // Not excess
            if (lastChar != null) {
                sb.Insert(caretPosition, lastChar);
                wordList[wordIndex].nTyped -= 1;
            }
            
            // Update word output
            wordOutput.text = sb.ToString();

            // Update word properties
            charIndex -= 1;  
        }
    }

    /**
        On spacebar input, checks for premature presses.
    */
    void EnterSpacebar() {
        // Check for premature spacebar (word has not finished yet)
        if (!wordList[wordIndex].IsFullyTyped()) {
            caretPosition += wordList[wordIndex].GetRemainingChars() + 1; /// + 1 for whitespace
        }
        // Move caret only one (user finished the word)
        else {
            caretPosition += 1;
        }
        // Next word, update indices
        wordIndex += 1;
        charIndex = 0;
    }

    
}
