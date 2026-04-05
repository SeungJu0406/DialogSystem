using NSJ_Dialogue;
using System;
using TMPro;
using UnityEngine;

public class DialogueChoiceDisplay : MonoBehaviour
{
    private TMP_Text _text;

    private DialogueChoiceElement _data;

    public void Select()
    {
        DialogueManager.Next(_data.NextID);
    }

    private void Awake()
    {
        if(_text == null)
        {
            _text = GetComponentInChildren<TMP_Text>();
        }

    }

    public void SetChoiceData(DialogueChoiceElement choice)
    {
        _data = choice;
        if(_text != null)
        {
            _text.text = choice.ChoiceText;
        }
    }

}
