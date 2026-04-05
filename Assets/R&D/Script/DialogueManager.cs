using System.Collections.Generic;
using UnityEngine;

namespace NSJ_Dialogue
{
    public static class DialogueManager
    {
        public static DialogueSystem Dialogue => DialogueSystem.Instance;
        public static void SetDialogueDisplayContainer(DialogueDisplayContainer displayContainer) => Dialogue.SetDisplayContainer(displayContainer);
        public static void SetDialogue(List<DialogueLine> dialogueLines) => Dialogue.SetDialogue(dialogueLines);    
        public static void Next() => Dialogue.Next();
        public static void Next(string id) => Dialogue.Next(id);
    }
}