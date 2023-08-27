using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text m_text;

    public bool isOpen {  get; private set; }

    private DialogueResponseHandler responseHandler;
    private DialogueTyping dialogueTyping;

    private void Start()
    {
        responseHandler = GetComponent<DialogueResponseHandler>();
        dialogueTyping = GetComponent<DialogueTyping>();
        CloseDialogueBox();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        isOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++) 
        {
            string dialogue = dialogueObject.Dialogue[i];
            yield return dialogueTyping.Run(dialogue, m_text);

            if(i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponse) break;

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }

        Debug.Log("Dialogue object complete");

        if (dialogueObject.HasResponse) 
        {
            Debug.Log("Dialog Had Response");
            responseHandler.ShowResponses(dialogueObject.Responses);
        }

        else
        {
            CloseDialogueBox();
        }

        //CloseDialogueBox();   idk why its here tbh but it stays
    }

    private void CloseDialogueBox()
    {
        isOpen = false;
        dialogueBox.SetActive(false);
    }
}
