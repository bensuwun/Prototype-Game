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
	private Queue<string> sentences = new Queue<string>();
    public DialogueTrigger dialogueTrigger;
	public TimelinePlayer timeline;
	private int dialogueIndex = 0;
	public Button button;

    void OnEnable()
    {
        // Debug.Log("HERE");
		IncrementDialogueIndex();
        button.interactable = true;
	}
	
	void Awake()
	{
		dialogueIndex = DataManager.GetDialogueIndex();
		Debug.Log(String.Format("Dialogue Index: {0}", dialogueIndex));
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
			IncrementDialogueIndex();
			return;
		}
		// Debug.Log("XD");
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
		
	}

	public void IncrementDialogueIndex(){
        dialogueTrigger.TriggerDialogue(dialogueIndex);
		dialogueIndex++;
		DataManager.SaveDialogueIndex(dialogueIndex);
	}

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			if(String.Equals(dialogueText.text,sentence)){
				Debug.Log(String.Format("Marker: {0}", sentence));
				timeline.PlayTimeline(sentence);
			}
			yield return null;
		}
	}


}