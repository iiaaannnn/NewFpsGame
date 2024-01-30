using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : Interactable
{
    public Dialogue dialogue;
    bool triggeredDialogue = false;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    protected override void Interact()
    {
        if(!triggeredDialogue)
        {
            TriggerDialogue();
        }
    }
}
