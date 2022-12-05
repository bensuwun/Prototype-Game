
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using UnityEngine;

/**
    Word class to store word properties for calculation:
    @param text (string) - word in string form
    @param lastIndex (int) - last index used in word (in case of word skipping)
    @param nCorrect (int) - number of correctly typed characters
*/
public class Word {
    // Getters and Setters
    public int nCorrect { get; set; }
    public int nTyped { get; set; }  // Represents index - 1 of last typed character
    public string Text { get; private set; }
    public List<bool> charStatuses; // Each value represents if a character at given index is correctly typed.

    public Word(string text) {
        this.Text = text;
        this.nCorrect = 0;
        this.nTyped = 0;
        initializeCharStatuses();
    }

    private void initializeCharStatuses() {
        charStatuses = new List<bool>();
        for (int i = 0; i < this.Text.Length; i++) {
            charStatuses.Add(false);
        }
    }
    /**
        Returns true if the last character was correctly typed.
    **/
    public bool IsPrevCharCorrectlyTyped() {
        bool isCorrect = false;

        char? prevChar = GetPrevChar();

        if (prevChar != null) {
            isCorrect = charStatuses[nTyped - 1];
        }

        Debug.Log("Previously typed status: " + isCorrect.ToString());

        return isCorrect;
    }

    public void SetCharStatus(int index, bool value) {
        if (index >= 0 && index < Text.Length)
            charStatuses[index] = value;
        else 
            Debug.LogWarning("[WARNING] Word.SetCharStatus() accepted an invalid index");
    }
    /**
        Helper function to calculate the remaining letters left for this word
    */
    public int GetRemainingChars() {
        return this.Text.Length - nTyped;
    }

    /**
        Helper function to return index of last untyped character (if any).
        If all characters have been typed, returns -1. 
    */
    public int GetNextIndex() {
        return nTyped != Text.Length ? nTyped : -1;
    }

    /**
        Helper function to get next expected char
    */
    public char? GetNextChar() {
        return IsFullyTyped() ? null : Text[nTyped];
    }

    public char? GetPrevChar() {
        return HasNothingTyped() ? null : Text[nTyped - 1];
    }

    /**
        Helper function to check if word has been fully typed (either correctly or incorrectly)
    */
    public bool IsFullyTyped() {
        return nTyped == Text.Length;
    }

    public bool HasNothingTyped() {
        return nTyped == 0;
    }

    /**
        Helper function to check if word was spelled correctly.
    */
    public bool IsSpelledCorrectly() {
        return nCorrect == Text.Length;
    }
    /**
        Helper function to check if each char is typed correctly.
    */
    public bool IsTypeCorrectly() {
        return false; // to be changed
    }

}