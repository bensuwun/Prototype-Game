using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public WordAnimator wordAnimator = null;
    public List<Image> modifiers; 
    private Color inactiveColor = new Color(0.5f,0.5f,0.5f,1f);
    private Color activeColor = new Color(1f,1f,1f,1f);
    private int NoneMashLong = 0;

    // Debuff variables (Ben)
    public List<TextMeshProUGUI> wordOutputs;
    public List<TMP_Text> textComponents;
    private float wiggleHeight = 0.5f;
    private float wiggleSpeed = 15f;
    private float debuffCooldownDuration = 3f;
    private float armsSpaghettiDuration = 5f;
    public bool executingDebuff = false;
    private string lastDebuff = "";

    // Debounce variables
    private bool endDebuffRunning = false;
    private bool hasTextChanged = false;

    public enum Buffs{
        HPRegen = 0,
        ButtonMash = 1,
        ClearDebuff = 2,
    }
    private enum Debuffs {
        ShortSighted = 0,
        LongWords = 1,
        ArmsSpaghetti = 2
    }

    void Start()
    {
        clearDebuffs();
        inventory.buttonMashFlag = false;
        inventory.clearDebuffFlag = false;
        inventory.hpRegenFlag = false;
        // Begin countdown for debuff
        StartCoroutine(ObtainDebuffAfterTime(debuffCooldownDuration));

        // Obtain text components for faster access
        textComponents = new List<TMP_Text>() {
            wordOutputs[0].GetComponent<TMP_Text>(),
            wordOutputs[1].GetComponent<TMP_Text>(),
            wordOutputs[2].GetComponent<TMP_Text>()
        };

    }

    void onEnable() {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
    }

    // Color 
    void ON_TEXT_CHANGED(Object obj) {
        if (obj == textComponents[0]) {
            hasTextChanged = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            healthRegen();
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)){
            buttonMash();
        }

        if(Input.GetKeyDown(KeyCode.Alpha3)){
            clearDebuffs();
        }

        if(inventory.longWordsFlag) {
            extraLongWords();
            inventory.longWordsFlag = false;
            StartCoroutine(EndDebuffAfterTime(armsSpaghettiDuration, (int)Debuffs.LongWords)); // can set possible range of time instead of constant duration (first param)
        }
        
        if(inventory.shortSightedFlag) {
            // ShortSighted(typer.GetCaretPosition());
            if (!endDebuffRunning){
                endDebuffRunning = true;
                StartCoroutine(EndDebuffAfterTime(armsSpaghettiDuration, (int)Debuffs.ArmsSpaghetti));
            }
        }

        if (inventory.armsSpaghettiFlag) {
            ArmsSpaghetti();

            // Begin executing timer for ArmsSpaghetti
            if (!endDebuffRunning){
                endDebuffRunning = true;
                StartCoroutine(EndDebuffAfterTime(armsSpaghettiDuration, (int)Debuffs.ArmsSpaghetti));
            }
                
        }
    }

    // ================ BUFFS ======================
    void healthRegen(){
        if(inventory.hpRegenFlag || testing){ // check if player has hp regen buff
            // int n_wpm = Mathf.FloorToInt((float)0.75 * CombineNumber(wpm.GetParsedText().ToIntArray()));
            // int hpRegen = n_wpm > 20 ? 20 : n_wpm;
            player.increaseHealth(15 );
            inventory.hpRegenFlag = false;
            SetBuffActive((int)Buffs.HPRegen, inventory.hpRegenFlag);
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
            SetBuffActive((int)Buffs.ButtonMash, inventory.buttonMashFlag);
        }
        NoneMashLong = 1;
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
            inventory.clearDebuffFlag = false;
            inventory.shortSightedFlag = false;
            inventory.armsSpaghettiFlag = false;
            revertLongWords();
            SetBuffActive((int)Buffs.ClearDebuff, inventory.clearDebuffFlag);

            modifiers[(int)Debuffs.ArmsSpaghetti + 3].color = inactiveColor;
            modifiers[(int)Debuffs.LongWords + 3].color = inactiveColor;
            modifiers[(int)Debuffs.ShortSighted + 3].color = inactiveColor; 
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
        NoneMashLong = 2;
        // Debug.Log(string.Format("Len: {0}", newWords.Count));
    }

    // ================ DEBUFFS ====================

    void revertLongWords(){
        if(previousWords != null && NoneMashLong == 2){
            typer.SetWordListWords(previousWords, 2);
        }
    }
    
    void ArmsSpaghetti() {
        // List<TMP_Text> textComponents = typer.GetTMPText_Components();
        // foreach (TMP_Text textComponent in textComponents) {
            var textComponent = textComponents[0];
            var textInfo = textComponent.textInfo;  // Info about text 

            for (int i = 0; i < textInfo.characterCount; i++) {
                var charInfo = textInfo.characterInfo[i];
                
                // Only do visible characters
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
        // }
    }

    // Starting index - current caret position
    void ShortSighted(int caretPosition) {
        Color32[] newVertexColors;
        Color32 c0;
        for(int i = 0; i < textComponents.Count; i++) {
            textComponents[i].ForceMeshUpdate();
            var textInfo = textComponents[i].textInfo;
            
            // If first text component, change starting index to caret position instead of 0
            for (int j = i == 0 ? caretPosition + 2 : 0; j < textInfo.characterCount; j++) {
                var charInfo = textInfo.characterInfo[j];
                
                if (!charInfo.isVisible) {
                    continue;
                }

                // Get the index of the material used by the current character
                int materialIndex = charInfo.materialReferenceIndex;

                // Get the vertex colors of the mesh used by this TMP object
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;

                // Get the id of first index used by character
                int vertexIndex = charInfo.vertexIndex;

                // Get current color of current character and set to transparent
                c0 = new Color32(charInfo.color.r, charInfo.color.g, charInfo.color.b, 0);

                // Update vertex color of character (transparency)
                newVertexColors[vertexIndex + 0] = c0;
                newVertexColors[vertexIndex + 1] = c0;
                newVertexColors[vertexIndex + 2] = c0;
                newVertexColors[vertexIndex + 3] = c0;

                // Push all updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer
                textComponents[i].UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            }
           
        }


    }

    // Pick a random debuff every [n, m] seconds
    IEnumerator ObtainDebuffAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Pick a random debuff
        int rngNum = UnityEngine.Random.Range(1,4);
        if (rngNum == 1){
            inventory.shortSightedFlag = true;
            Debug.Log("Debuff Sight");
            modifiers[(int)Debuffs.ShortSighted + 3].color = activeColor;
        }else if(rngNum == 2){
            inventory.armsSpaghettiFlag = true;
            Debug.Log("Debuff Spage");
            modifiers[(int)Debuffs.ArmsSpaghetti + 3].color = activeColor;
        }else if(rngNum == 3){
            inventory.longWordsFlag = true;
            Debug.Log("Debuff Long ");
            modifiers[(int)Debuffs.LongWords + 3].color = activeColor;
        }
    }

    /**
        After certain amount of time, end debuff given by debuff code.
        After debuff ends, start coroutine to pick another debuff after [n,m] amount of time
    */
    IEnumerator EndDebuffAfterTime(float time, int debuffCode)
    {
        yield return new WaitForSeconds(time);

        switch(debuffCode) {
            case (int)Debuffs.ArmsSpaghetti:
                inventory.armsSpaghettiFlag = false;
                char ch = textComponents[0].textInfo.characterInfo[0].character;
                textComponents[0].textInfo.characterInfo[0].character = ch;
                modifiers[(int)Debuffs.ArmsSpaghetti + 3].color = inactiveColor;
                break;
            case (int)Debuffs.LongWords:
                inventory.longWordsFlag = false;
                modifiers[(int)Debuffs.LongWords + 3].color = inactiveColor;
                break;
            case (int)Debuffs.ShortSighted:
                inventory.shortSightedFlag = false;
                modifiers[(int)Debuffs.ShortSighted + 3].color = inactiveColor; 
                break;
        }

        endDebuffRunning = false;

        // Pick random debuff after certain amount of time
        // TODO: Randomize value given range
        StartCoroutine(ObtainDebuffAfterTime(debuffCooldownDuration));
    }

    public void SetBuffActive(int index ,bool isActive){
        modifiers[index].color = isActive ? activeColor : inactiveColor;
    }
}
