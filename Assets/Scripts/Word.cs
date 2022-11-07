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
    public int LastIndex { get; set; }
    public string Text { get; private set; }

    public Word(string text) {
        this.Text = text;
        this.nCorrect = 0;
        this.LastIndex = 0;
    }

    

}