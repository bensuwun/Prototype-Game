using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Modifiers : MonoBehaviour
{
    public Player player = null;
    public TextMeshProUGUI wpm = null;
    public CustomTyper typer = null;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt)){
            healthRegen();
        }

        if(Input.GetKeyDown(KeyCode.LeftControl)){
            buttonMash();
        }

        
    }

    void healthRegen(){
        if(true){ // check if player has hp regen buff
            // int n_wpm = Mathf.FloorToInt((float)0.75 * CombineNumber(wpm.GetParsedText().ToIntArray()));
            // int hpRegen = n_wpm > 20 ? 20 : n_wpm;
            player.increaseHealth(15 );
        }
    }

    void buttonMash(){
        var words = typer.getWordList(2);
        Word newWord = null;
        List<Word> newWords = new List<Word>();
        if(true){ // check inventory condition
            foreach (var word in words)
            {
                newWord = createMashWord("a",word.Text.Length);
                newWords.Add(newWord);
            }
            typer.SetWordListWords(newWords,2);
            // Debug.Log(string.Format("Len: {0}", newWords.Count));
        }

    }

    Word createMashWord(string mono, int length){
        string mashWord = "";
        for(int i = 0; i < length; i ++){
            mashWord += mono;
        }
        return new Word(mashWord);
    }

    int CombineNumber(int[] arr){
        int accum = 0;
        int i = 0;
        foreach (var num in arr)
        {
            accum += (num*(arr.Length-i));
            i++;
        }

        return accum;
    }
}
