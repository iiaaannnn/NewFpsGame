using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator dialogueAnim;
    bool canClickNext = false;

    int aa;
    Dialogue[] dialogueVal;
    private Queue<string> sentences;

    bool dialogueStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("ContinueDialogue") && dialogueStarted && canClickNext)
        {
            DisplayNextSentence();
        }

    }

    public void StartDialogue(Dialogue[] dialogue)
    {
        dialogueStarted = true;

        dialogueVal = dialogue;
        dialogueAnim.SetBool("IsOpen", true);

        nameText.text = dialogue[aa].name;
        sentences.Clear();

        foreach (string sentence in dialogue[aa].sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();

    }


    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            aa++;
            if (!NextPerson())
            {
                return;
            }
        }
        if (sentences.Count != 0)
        {
            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }


    }

    bool NextPerson()
    {
        if (aa >= dialogueVal.Length)
        {
            EndDialogue();
            return true;
        }

        nameText.text = dialogueVal[aa].name;
        sentences.Clear();

        foreach (string sentence in dialogueVal[aa].sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
        return false;
    }

    void EndDialogue()
    {
        dialogueAnim.SetBool("IsOpen", false);
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);

            if (dialogueText.text == sentence)
            {
                canClickNext = true;
            }
            else
            {
                canClickNext = false;
            }
        }

    }


}