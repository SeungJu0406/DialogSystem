using NSJ_Dialogue;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueHeaderContainer", menuName = "Scriptable Objects/Dialogue/HeaderContainer")]
public class DialogueHeaderContainer : ScriptableObject
{
    public List<DialogueHeader> Headers = new List<DialogueHeader>();
}
