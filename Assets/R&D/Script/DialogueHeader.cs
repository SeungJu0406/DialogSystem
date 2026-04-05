using UnityEngine;

namespace NSJ_Dialogue
{
    public enum DialogueElementType
    {
        None,
        ID,
        NextID,
        Text,
        Image,
        Object,
        Choice,
    }

    [CreateAssetMenu(fileName = "DialogueHeader", menuName = "Scriptable Objects/Dialogue/Header")]
    public class DialogueHeader : ScriptableObject
    {
        public DialogueElementType Type;
        public string HeaderLabel;
    }
}