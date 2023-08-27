using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{

    [SerializeField][TextArea] private string[] dialogue;
    [SerializeField] private DialogueResponse[] responses;

    public string[] Dialogue => dialogue;

    public bool HasResponse => Responses != null && Responses.Length > 0;
    public DialogueResponse[] Responses => responses;
}