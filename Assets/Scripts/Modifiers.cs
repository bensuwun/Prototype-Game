using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Modifiers : MonoBehaviour
{
    public Player player = null;
    public TextMeshProUGUI wpm = null;
    public CustomTyper typer = null;
    public PlayerInventory inventory = null;
    public WordBank wordBank = null;
    private List<Word> previousWords = null;
    private bool testing = true;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt)){
            healthRegen();
        }

        if(Input.GetKeyDown(KeyCode.LeftControl)){
            buttonMash();
        }

        if(Input.GetKeyDown(KeyCode.Tab)){
            clearDebuffs();
        }

        if(Input.GetKeyDown(KeyCode.RightArrow)){
            inventory.longWordsFlag = true;
            extraLongWords();
        }
        
    }

    void healthRegen(){
        if(inventory.hpRegenFlag || testing){ // check if player has hp regen buff
            // int n_wpm = Mathf.FloorToInt((float)0.75 * CombineNumber(wpm.GetParsedText().ToIntArray()));
            // int hpRegen = n_wpm > 20 ? 20 : n_wpm;
            player.increaseHealth(15 );
            inventory.hpRegenFlag = false;
        }
    }

    void buttonMash(){
        var words = typer.getWordList(2);
        Word newWord = null;
        List<Word> newWords = new List<Word>();
        if(inventory.buttonMashFlag || testing){ // check inventory condition
            foreach (var word in words)
            {
                newWord = createMashWord("a",word.Text.Length);
                newWords.Add(newWord);
            }
            typer.SetWordListWords(newWords,2);
            // Debug.Log(string.Format("Len: {0}", newWords.Count));
            inventory.buttonMashFlag = false;
        }

    }

    Word createMashWord(string mono, int length){
        string mashWord = "";
        for(int i = 0; i < length; i ++){
            mashWord += mono;
        }
        return new Word(mashWord);
    }

    void clearDebuffs(){
        if(inventory.clearDebuffFlag || testing){
            inventory.shortSightedFlag = false;
            inventory.armsSpaghettiFlag = false;
            revertLongWords();
        }
    }

    void extraLongWords(){
        if(inventory.longWordsFlag){
            // Word newWord = null;
            previousWords = typer.getWordList(2);
            string wordSoup = "";
            List<Word> newWords = new List<Word>();
            wordSoup = wordBank.GetLongWords();
            foreach(string str in wordSoup.Split(" ")){
                Word newWord = new Word(str);
                newWords.Add(newWord);
            }
            typer.SetWordListWords(newWords,2);
            // Debug.Log(string.Format("Len: {0}", newWords.Count));
            inventory.longWordsFlag = false;
        }
    }

    void revertLongWords(){
        if(previousWords != null){
            typer.SetWordListWords(previousWords, 2);
        }
    }
}
