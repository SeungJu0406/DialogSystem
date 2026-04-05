using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NSJ_Dialogue
{
    public class DialogueRunner : MonoBehaviour
    {

        [SerializeField] DialogueScript _script;
        

        public void StartDialogue()
        {
            DialogueManager.SetDialogue(_script.DialogueLines);
        }
    }
}
