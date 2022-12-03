using System.Xml.Serialization;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CustomTyper : MonoBehaviour
{
    private static int LEVEL;
    // Current word output
    public TextMeshProUGUI wordOutput;
    public TextMeshProUGUI wordOutput2;
    public TextMeshProUGUI wordOutput3;


    // Boss Image
    public Image bossImage;
    // Current WPM
    public TextMeshProUGUI currWPMText;
    public TextMeshProUGUI currAccText;

    public TextMeshProUGUI comboCounterText;
    public TextMeshProUGUI comboText;
    public Animator comboAnimator;
    public WordBank wordBank = null;
    public StatsCalc statsCalc = null;
    public Player player = null;
    public Boss boss = null;
    public WordAnimator wordAnimator = null;
    public PlayerInventory inventory = null;
    private string sourceString = string.Empty;
    private string sourceString2 = string.Empty;
    private string sourceString3 = string.Empty; 
    private List<Word> wordList = new List<Word>();
    private List<Word> wordList2 = new List<Word>();
    private List<Word> wordList3 = new List<Word>();
    public GameObject caret;
    private StringBuilder sb;
    public Modifiers modifier;
    

    private double currWPM = 0d;
    private double wpmThreshold = 0d;

    // Indexing current word and current char
    private int wordIndex = 0;
    private int charIndex = 0;  // also represents number of typed characters FOR A WORD
    private int lineCharIndex = 0; // number of typed characters FOR A LINE
    private int caretPosition = 0;

    // Colors for correct, incorrect, default characters
    public string correctColor = "green";
    public string incorrectColor = "red";
    public string defaultColor = "#808080";

    private int numCharsTyped = 0;
    private int numCorrectChars = 0;
    private int numSpace = 0;

    private float lastIdleTime = 0f;
    private float idleTimeLimit = 5f;

    private int comboCount = 0;
    // Sound variables
    List<bool> debounceCombos = new List<bool>() {
        false, false, false, false, false
    };
    bool debounceSFXLowHP = false;
    public BattleSoundManager soundManager;

    // Postgame Modal Variables
    public GameObject PanelModal;
    public GameObject modal;
    private bool isModalShowing = false;
    private bool buttonClicked = false; // For avoiding multiple clicks of buttons


    // Enums
    private enum Enums {
        // Input Formats
        IncorrectInputFormat = 0,
        CorrectInputFormat = 1,
        BackspaceInputFormat = 2,   // For resetting to default color when backspace is pressed
    }

    // Start is called before the first frame update
    void Start() {
        int level = DataManager.GetLevel();
        Debug.Log("Current Level: " + level);
        instantiateBattle(level);
    }

    public void instantiateBattle(int level) {
        LEVEL = level;
        float bossHP = 0f;
        float playerHP = 100f;

        switch(LEVEL) {
            case 1:
                // Remove or set Ivy as boss image
                bossImage.color = new Color32((byte)bossImage.color.r, (byte)bossImage.color.g, (byte)bossImage.color.b, 0);
                bossHP = 100f;
                wpmThreshold = 10d;
                idleTimeLimit = 10f;
                soundManager.PlayBGM(level);
                break;
            case 2:
                bossImage.sprite = Resources.Load<Sprite>("Sprites/Characters-bosses/AMOGUS");
                bossHP = 200f;
                wpmThreshold = 20d;
                idleTimeLimit = 4f;
                soundManager.PlayBGM(level);
                break;
            case 3:
                bossImage.sprite = Resources.Load<Sprite>("Sprites/Characters-bosses/Final Boss");
                bossHP = 300f;
                wpmThreshold = 30d;
                idleTimeLimit = 2f;
                soundManager.PlayBGM(level);
                break;
            case 4:
                bossImage.sprite = Resources.Load<Sprite>("Sprites/Characters-bosses/Final Boss idle");
                bossHP = 999999f;
                wpmThreshold = 9999d;
                idleTimeLimit = 0.3f;
                soundManager.PlayBGM(3); // same as previous level
                break;
            default:
                Debug.LogWarning("[WARNING] Current Level is not in range of allowed values. Current Level = " + level.ToString());
                break;
        }
        // sets the boss's max HP
        boss.setMaxHP(bossHP);
        // sets the player's max HP 
        player.setMaxHP(playerHP);

        // sets the words that are displayed
        StartCoroutine(InitializeWordLists());

        // gets the current time
        statsCalc.getStart();

        // show current WPM to 0
        currWPMText.text = "" + currWPM;
        currAccText.text = String.Format("100%");
        comboCounterText.text = "";
        comboText.text = "";

         // Display current WPM on screen
        StartCoroutine(checkWPMAndAccuracy());
        StartCoroutine(checkIdle());
        StartCoroutine(updateCombo());
        StartCoroutine(ComboSFXPlayer());
        StartCoroutine(checkLowHP());
    }

    IEnumerator InitializeWordLists(){
        // For initialization: Each list gets filled with words
        
        if(wordList.Count == 0){
            SetWordListWords(wordList, out sourceString);
            SetTextGUI(wordOutput, sourceString);
        }
        if(wordList2.Count == 0){
            SetWordListWords(wordList2, out sourceString2);
            SetTextGUI(wordOutput2, sourceString2);
        }
        if(wordList3.Count == 0){
            SetWordListWords(wordList3, out sourceString3);
            SetTextGUI(wordOutput3, sourceString3);         // Display source string in output text
        }

        sb = new StringBuilder(sourceString); // display string for first line ; basis for many code below

        yield return new WaitForSeconds(0.1f);
        UpdateVisualCaretPosition();
    }

    IEnumerator SetCurrentWords(){
        // Once wordlist 1 is empty (Player is done with the line)
        // wordlist 1 <- wordlist 2 <- wordlist 3
        // then wordlist 3 gets new set of words
        wordList = wordList2;
        wordList2 = wordList3;
        wordList3 = new List<Word>();

        // Update words to display
        sourceString = String.Copy(sourceString2);
        sourceString2 = String.Copy(sourceString3);
        // sourceString3 = string.Empty;

        SetWordListWords(wordList3, out sourceString3);

        // Push updated words to GUI
        wordAnimator.wordNextLine(0);
        SetTextGUI(wordOutput, sourceString);
        wordAnimator.wordNextLine(1);
        SetTextGUI(wordOutput2, sourceString2);
        wordAnimator.wordNextLine(2);
        SetTextGUI(wordOutput3, sourceString3);

        sb = new StringBuilder(sourceString); 

        yield return new WaitForSeconds(0.1f);
        UpdateVisualCaretPosition();
    }
    public List<Word> getWordList(int number){
        if(number == 1)
            return wordList;
        else if(number == 2)
            return wordList2;
        else if(number == 3)
            return wordList3; 
        else return null;
    }

    public int GetCaretPosition () {
        return caretPosition;
    }

    public void SetWordListWords(List<Word> words ,int number){
        string output = null;
        if(number == 1)
            wordList = words;
        else if(number == 2)
            wordList2 = words;
        else if(number == 3)
            wordList3 = words;

        foreach (var word in words){
            output += word.Text + " ";
            // Debug.Log(String.Format("Word : {0}", word.Text ));
        }
        output.Remove(output.LastIndexOf(" "));
        if(number == 1){
            SetTextGUI(wordOutput, output);
            sourceString = output;
        }else if(number == 2){
            SetTextGUI(wordOutput2, output);
            sourceString2 = output;
        }else if(number == 3){
            SetTextGUI(wordOutput3, output);
            sourceString3 = output;
        }
        Debug.Log(String.Format("Modified String: {0} Len: {1}",output, words.Count));
    }

    // Parse source string to list of words
    private void SetWordListWords(List<Word> words, out string outputStr){
        string stringFromBank = wordBank.GetWords();
        foreach(string str in stringFromBank.Split(" ")){
            Word newWord = new Word(str);
            words.Add(newWord);
        }
        outputStr = stringFromBank;
        // Debug.Log(String.Format("Output String: {0} Len: {1}",outputStr, words.Count));
        // Debug.Log(String.Format("Last Word: {0}", words[words.Count-1].IsFullyTyped()));
    }

    private void SetTextGUI(TextMeshProUGUI textArea, string str){
        textArea.SetText(str);
    }

    private void ResetIndeces(){
        caretPosition = 0;
        // wordIndex += 1; // temporary solution for extra " " at end of each set of words
        wordIndex = 0;
        lineCharIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(boss.isBossDead()) {
            if (!isModalShowing){
                StopAllCoroutines();
                DisplayResults((int)currWPM, statsCalc.getAccuracy(numCorrectChars, numCharsTyped), "VICTORY");
            }
                
        }
        else if (player.isPlayerDead()) {
            if (!isModalShowing) {
                StopAllCoroutines();
                DisplayResults((int)currWPM, statsCalc.getAccuracy(numCorrectChars, numCharsTyped), "DEFEAT");
            }
                
        }
        else {
            string inputString = Input.inputString;
            
            if (inputString.Length == 1) {
                // wordAnimators[0].SetTrigger("NextLineTrigger");
                lastIdleTime = Time.time;
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
                    default:
                        break;

                }

                // Check if the current words on the screen are already finished and set new words
                if (AreWordsComplete()) {
                    StartCoroutine(SetCurrentWords());
                    ResetIndeces();
                }
            } 
        }
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
        // Debug.Log(string.Format("Word Index: {0} | Char Index: {1} | Caret Position: {2}", 
            // wordIndex, charIndex, caretPosition));
            
        // if enter number(use buff) do nothing
        if (Char.IsNumber(input[0])) return;

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
            lineCharIndex += 1;
            player.TakeDamage(1);
            ResetCombo();
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
            lineCharIndex += 1;
            wordList[wordIndex].SetCharStatus(wordList[wordIndex].nTyped, true);
            wordList[wordIndex].nTyped += 1;
            wordList[wordIndex].nCorrect += 1;
            numCorrectChars += 1;
            

            boss.TakeDamage(.5f, wpmThreshold, currWPM, comboCount);

            comboCount += 1;
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
            lineCharIndex += 1;
            wordList[wordIndex].SetCharStatus(wordList[wordIndex].nTyped, false);
            wordList[wordIndex].nTyped += 1;
            player.TakeDamage(1);
            ResetCombo();
        }
        
        UpdateVisualCaretPosition();
        numCharsTyped += 1;
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

                // Check if lastChar was correctly typed
                if (wordList[wordIndex].IsPrevCharCorrectlyTyped()) {
                    numCorrectChars -= 1;
                    wordList[wordIndex].SetCharStatus(wordList[wordIndex].nTyped - 1, false);
                }
                wordList[wordIndex].nTyped -= 1;
            }
            
            // Update word output
            wordOutput.text = sb.ToString();

            // Update word properties
            charIndex -= 1;  
            lineCharIndex -= 1;
            numCharsTyped -= 1;
        }
        
        UpdateVisualCaretPosition();
        ResetCombo();
    }

    /**
        On spacebar input, checks for premature presses.
    */
    void EnterSpacebar() {
  
        // do nothing if no character typed
        if(charIndex == 0){
            return;
        }
        // Check for premature spacebar (word has not finished yet)
        if (!wordList[wordIndex].IsFullyTyped()) {
            ResetCombo();
            caretPosition += wordList[wordIndex].GetRemainingChars() + 1; /// + 1 for whitespace
            lineCharIndex += wordList[wordIndex].GetRemainingChars(); 
        }
        // Move caret only one (user finished the word)
        else {
            caretPosition += 1;
            numSpace += 1;
        }
        // Next word, update indices
        wordIndex += 1;
        charIndex = 0;
        lineCharIndex += 1;
        UpdateVisualCaretPosition();
    }

    void UpdateVisualCaretPosition() {
        TMP_TextInfo textInfo = wordOutput.GetComponent<TMP_Text>().textInfo;
        TMP_CharacterInfo charInfo = textInfo.characterInfo[lineCharIndex];
        
        // Get vector position of current character 
        Vector3 currentPosition = charInfo.bottomLeft;

        // Move caret to character position
        caret.transform.localPosition = new Vector3(currentPosition.x, currentPosition.y + 15, 0);
    }

    // Checks if the current line is already finished, return true if done, false if not
    private bool AreWordsComplete() {
        // check length of the remaining line
        // no next word
        bool isThereNoNextWord = wordList.Count == (wordIndex + 1);
        bool isWordFullyTyped = wordList[wordIndex].IsFullyTyped();
        return isThereNoNextWord && isWordFullyTyped;
    }

    private void DisplayResults(int WPM, int accuracy, string result) {
        isModalShowing = true;

        GameObject WPMResult = modal.transform.Find("WPMResult").gameObject;
        GameObject accuracyResult = modal.transform.Find("AccuracyResult").gameObject;
        GameObject button = modal.transform.Find("Button").gameObject;
        GameObject title = modal.transform.Find("Title").gameObject;

        soundManager.PlayBGMResult(result);

        // Set title, can change color
        if (result.Equals("VICTORY")) {
            title.GetComponent<TMP_Text>().text = result;
            button.transform.Find("Text").gameObject.GetComponent<TMP_Text>().text = "Continue";
            button.GetComponent<Button>().onClick.AddListener(() => {
                // Update level, go back to story scene
                if (!buttonClicked) {
                    buttonClicked = true;
                    button.GetComponent<Button>().onClick.RemoveAllListeners();

                    // Update player's level
                    int currentLevel = DataManager.GetLevel();
                    int nextLevel = currentLevel == 4 ? 1 : currentLevel + 1;
                    DataManager.SaveLevel(nextLevel);

                    SceneManager.LoadScene("StoryScene");
                }
                Debug.Log("Button pressed");
            });
        }
        else {
            title.GetComponent<TMP_Text>().text = result;
            button.transform.Find("Text").gameObject.GetComponent<TMP_Text>().text = "Retry";

            button.GetComponent<Button>().onClick.AddListener(() => {
                buttonClicked = true;
                button.GetComponent<Button>().onClick.RemoveAllListeners();

                // Reset scene with same parameters
                SceneManager.LoadScene("BattleScene");
                Debug.Log("Retry button pressed");
            });
        }

        // Set result values
        WPMResult.GetComponent<TMP_Text>().text = WPM.ToString();

        // Calculate accuracy
        accuracyResult.GetComponent<TMP_Text>().text = accuracy.ToString() + "%";

        // Set modal to active
        PanelModal.SetActive(true);
    }

    public PlayerInventory GetInventory(){
        return inventory;
    }

    private void ResetCombo() {
        comboCount = 0;
        for(int i = 0; i < debounceCombos.Count; i++) {
            debounceCombos[i] = false;
        }
    }

    // sets the WPM
    private IEnumerator checkWPMAndAccuracy() {
        while (true) {
            currWPM = statsCalc.getCurrWPM(numCorrectChars, numSpace);
            currWPMText.text = "" + currWPM;
            currAccText.text = String.Format("{0}%", statsCalc.getAccuracy(numCorrectChars, numCharsTyped));
            yield return null;
        }
    }

    // checks for idleness and makes the player take damage whenever idle
    private IEnumerator checkIdle() {
        while (true) {
            if (Time.time - lastIdleTime > idleTimeLimit) {
                player.TakeDamage(10);
                ResetCombo();
                lastIdleTime = Time.time;
            }
        
            yield return null;
        }
    }

    // checks if player is low HP to play SFX
    private IEnumerator checkLowHP() {
        while (true) {
            if (player.currentHealth <= 20 && !debounceSFXLowHP) {
                debounceSFXLowHP = true;        // set debounce to true
                soundManager.PlaySFXLowHP();
            }
            else if (debounceSFXLowHP && player.currentHealth > 20) {
                debounceSFXLowHP = false;
                soundManager.StopSFXLowHP();
            }
            yield return null;
        }
    }

    private IEnumerator updateCombo() {
        string showComboText = "";
        string text = "";
        string textColor = "white";
        while (true) {
            if (comboCount!= 0) {
                showComboText = "Combo:";
                text = "x " + comboCount;
            }
            else {
                showComboText = "";
                text = "";
            }

            if (comboCount > 0) textColor = ComboInfo.comboColor0;
            if (comboCount >= ComboInfo.combo1) textColor = ComboInfo.comboColor1;
            if (comboCount >= ComboInfo.combo2) textColor = ComboInfo.comboColor2;
            if (comboCount >= ComboInfo.combo3) textColor = ComboInfo.comboColor3;
            if (comboCount >= ComboInfo.combo4) textColor = ComboInfo.comboColor4;
            if (comboCount >= ComboInfo.combo5) textColor = ComboInfo.comboColor5;

            if (comboCount % 35 == 0 && comboCount > 0){
                inventory.clearDebuffFlag = true;
                modifier.SetBuffActive((int)Modifiers.Buffs.ClearDebuff, inventory.clearDebuffFlag);
            } 
            if (comboCount % 50 == 0 && comboCount > 0){
                inventory.hpRegenFlag = true;
                modifier.SetBuffActive((int)Modifiers.Buffs.HPRegen, inventory.hpRegenFlag);
            }
            if (comboCount % 60 == 0 && comboCount > 0){
                inventory.buttonMashFlag = true;
                modifier.SetBuffActive((int)Modifiers.Buffs.ButtonMash, inventory.buttonMashFlag);
            }

            string formattedText = String.Format("<color={0}>{1}</color>", textColor, text);
            string formattedComboText = String.Format("<color={0}>{1}</color>", textColor, showComboText);

            comboCounterText.text = formattedText;
            comboText.text = formattedComboText;
            
            float fontSize = 60f;
            float perHitFontSize = 0.7f;

            comboCounterText.fontSize = fontSize + perHitFontSize * comboCount;
            yield return null;
        }
    }

    private IEnumerator ComboSFXPlayer() {
        while (true) {
            if (comboCount == ComboInfo.combo1 && !debounceCombos[0]) {
                debounceCombos[0] = true;
                comboAnimator.SetTrigger("Pulse");
                soundManager.PlaySFXCombo(1);
            }
            else if (comboCount == ComboInfo.combo2 && !debounceCombos[1]) {
                comboAnimator.SetTrigger("Pulse");
                debounceCombos[1] = true;
                soundManager.PlaySFXCombo(2);
            }
            else if (comboCount == ComboInfo.combo3 && !debounceCombos[2]) {
                comboAnimator.SetTrigger("Pulse");
                debounceCombos[2] = true;
                soundManager.PlaySFXCombo(3);
            }
            else if (comboCount == ComboInfo.combo4 && !debounceCombos[3]) {
                comboAnimator.SetTrigger("Pulse");
                debounceCombos[3] = true;
                soundManager.PlaySFXCombo(4);
                
            }
            else if (comboCount == ComboInfo.combo5 && !debounceCombos[4]) {
                comboAnimator.SetTrigger("Pulse");
                debounceCombos[4] = true;
                soundManager.PlaySFXCombo(5);
            }
            yield return null;
        }
    }
}
