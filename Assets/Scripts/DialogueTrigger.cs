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
		

	}

	public void InitializeDialogues(){
		// read file
		// store file content into List

	}
}