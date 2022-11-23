using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Modifiers : MonoBehaviour
{
    public Player player = null;
    public TextMeshProUGUI wpm = null;
    public CustomTyper typer = null;
    private PlayerInventory inventory = null;
    public WordBank wordBank = null;
    private List<Word> previousWords = null;
    private bool testing = true;
    public WordAnimator wordAnimator = null;
    void Start()
    {
        inventory = typer.GetInventory();
    }

    // Debuff variables (Ben)
    public List<TextMeshProUGUI> wordOutputs;
    private float wiggleHeight = 0.5f;
    private float wiggleSpeed = 15f;
    private float debuffCooldownDuration = 10f;
    public bool executingDebuff = false;
    private string lastDebuff = "";
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt)){
            healthRegen();
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)){
            buttonMash();
        }

        if(Input.GetKeyDown(KeyCode.Alpha3)){
            clearDebuffs();
        }

        if(Input.GetKeyDown(KeyCode.RightArrow)){
            inventory.longWordsFlag = true;
            extraLongWords();
        }
        
        if (inventory.armsSpaghettiFlag) {
            inventory.armsSpaghettiFlag = true;
            ArmsSpaghetti();
        }

        StartCoroutine(PickDebuffAfterTime(debuffCooldownDuration));
    }

    // ================ BUFFS ======================
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
        wordAnimator.wordNextLine(1);
        // Debug.Log(string.Format("Len: {0}", newWords.Count));
        inventory.longWordsFlag = false;

    }

    // ================ DEBUFFS ====================

    void revertLongWords(){
        if(previousWords != null){
            typer.SetWordListWords(previousWords, 2);
        }
    }
    
    void ArmsSpaghetti() {
        // List<TMP_Text> textComponents = typer.GetTMPText_Components();

        foreach (TMP_Text wordOutput in wordOutputs) {
            var textComponent = wordOutput.GetComponent<TMP_Text>();
            var textInfo = textComponent.textInfo;  // Info about text 

            for (int i = 0; i < textInfo.characterCount; i++) {
                var charInfo = textInfo.characterInfo[i];
                
                // Only do visible characters (ignore rich text)
                if (!charInfo.isVisible) {
                    continue;
                }

                var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

                // Iterate over meshInfo.vertices (draft copy)
                for (int j = 0; j < 4; j++) {
                    var orig = verts[charInfo.vertexIndex + j];
                    verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * wiggleSpeed + orig.x * 0.025f) * wiggleHeight, 0);
                }
            }
            // Iterate over meshInfo.mesh.vertices (real copy)
            for (int i = 0; i < textInfo.meshInfo.Length; i++) {
                var meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                textComponent.UpdateGeometry(meshInfo.mesh, i);
            }
        }
    }

    // Pick a random debuff every [n, m] seconds
    IEnumerator PickDebuffAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // TODO: Pick a random debuff (Ben)
        inventory.armsSpaghettiFlag = true;

        // TODO: Set duration of cooldown before next debuff (Ben)

        // TODO: Set duration of spaghetti and shortsighted debuffs (Ben)
        
        
    }
}
