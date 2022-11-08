using System;

/**
    Word class to store word properties for calculation:
    @param text (string) - word in string form
    @param lastIndex (int) - last index used in word (in case of word skipping)
    @param nCorrect (int) - number of correctly typed characters
*/
public class Word {
    // Getters and Setters
    public int nCorrect { get; set; }
    public int nTyped { get; set; }  // Represents index of last typed character
    public string Text { get; private set; }

    public Word(string text) {
        this.Text = text;
        this.nCorrect = 0;
        this.nTyped = 0;
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