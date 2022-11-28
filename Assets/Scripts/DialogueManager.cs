using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Playables;

public class DialogueManager : MonoBehaviour {

	public TextMeshProUGUI nameText;
	public TextMeshProUGUI dialogueText;
	private Queue<string> sentences;
    public DialogueTrigger dialogueTrigger;
	public TimelinePlayer timeline;
	private int dialogueIndex = 0;
	public Button button;

    void OnEnable()
    {
        // Debug.Log("HERE");
        this.IncrementDialogueIndex();
        button.interactable = true;
    }
	// Use this for initialization
	void Start () {
		sentences = new Queue<string>();
        dialogueTrigger.TriggerDialogue(dialogueIndex);
		IncrementDialogueIndex();
	}
	

	public void StartDialogue (Dialogue dialogue)
	{
		// Debug.Log(String.Format("Dialogue: {0} - {1}", dialogue.name, dialogue.sentences[0]));
		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}
		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		string sentence = "";
		if(sentences.Count > 0){
			sentence = sentences.Dequeue();
		}else{
			// IncrementDialogueIndex();
			button.interactable = false;
			timeline.StartTimeline();
			return;
		}
		Debug.Log("XD");
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	public void IncrementDialogueIndex(){
        dialogueTrigger.TriggerDialogue(dialogueIndex);
		dialogueIndex++;
	}

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}


}