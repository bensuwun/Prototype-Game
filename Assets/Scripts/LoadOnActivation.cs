using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadOnActivation : MonoBehaviour
{
    public DialogueManager dialogueManager = null;
    public Button button = null;
    void OnEnable()
    {
        // Debug.Log("HERE");
        dialogueManager.IncrementDialogueIndex();
        button.interactable = true;
    }
}
