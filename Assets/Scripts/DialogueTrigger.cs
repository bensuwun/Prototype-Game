using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour {

	[SerializeField]
	public List<Dialogue> dialogue;
    public DialogueManager manager;
	public void TriggerDialogue(int index)
	{
		// Debug.Log(index);
		if(index < dialogue.Count)
			manager.StartDialogue(dialogue[index]);
		
		// TDOO: Setup scene transitions based on dialog index
		
	}

	public void InitializeDialogues(){
		// read file
		// store file content into List

	}

}