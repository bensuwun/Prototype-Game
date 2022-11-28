using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	[SerializeField]
	public List<Dialogue> dialogue;
    public DialogueManager manager;
	public void TriggerDialogue(int index)
	{
		// Debug.Log(index);
		manager.StartDialogue(dialogue[index]);
	}

	public void InitializeDialogues(){
		// read file
		// store file content into List

	}

}