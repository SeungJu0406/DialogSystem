using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static NSJ_Dialogue.DialogueDisplayContainer;

namespace NSJ_Dialogue
{
    [System.Serializable]
    public struct DialogueDisplayData
    {
        public string Label;
        public DialogueDisplayType Type;
        public GameObject Root;
    }
    public enum DialogueDisplayType
    {
        Text,
        Image,
        Choice,
    }
    public class DialogueDisplayContainer : MonoBehaviour
    {


        [SerializeField] private GameObject _dialogueChoiceSlot;

        [SerializeField] private DialogueDisplayData[] _dialogueDisplayDataArray;

        private Dictionary<string, DialogueDisplaySlot> _dialogueDisplayDataDict = new Dictionary<string, DialogueDisplaySlot>();

        private string _currentChoiceLabel = string.Empty;

        private List<GameObject> _currentChoiceObjects = new List<GameObject>();

        private void Awake()
        {
            DialogueManager.SetDialogueDisplayContainer(this);

            foreach (var data in _dialogueDisplayDataArray)
            {
                _dialogueDisplayDataDict.Add(data.Label, OrganizeDisplay(data));
            }
        }

        private DialogueDisplaySlot OrganizeDisplay(DialogueDisplayData data)
        {
            switch (data.Type)
            {
                case DialogueDisplayType.Text:
                    return new DialogueDisplaySlot<TMP_Text>(data);
                case DialogueDisplayType.Image:
                    return new DialogueDisplaySlot<Image>(data);
                case DialogueDisplayType.Choice:
                    _currentChoiceLabel = data.Label;
                    return new DialogueDisplaySlot<Transform>(data);
                default:
                    return new DialogueDisplaySlot<Transform>(data);
            }
        }

        public void SetText(string label, string text)
        {
            if (_dialogueDisplayDataDict.TryGetValue(label, out var slot))
            {
                var textComponent = slot.TryGet<TMP_Text>();
                if (textComponent != null)
                {
                    textComponent.text = text;
                }
            }
        }
        public void SetImage(string label, Sprite sprite)
        {
            if (_dialogueDisplayDataDict.TryGetValue(label, out var slot))
            {
                var imageComponent = slot.TryGet<Image>();
                if (imageComponent != null)
                {
                    imageComponent.sprite = sprite;
                }
            }
        }

        public void SetChoice(List<DialogueChoiceElement> choices)
        {
            for(int i = _currentChoiceObjects.Count -1; i >=0; i--)
            {
                Destroy(_currentChoiceObjects[i]);
            }

            _currentChoiceObjects.Clear();

            if (_dialogueDisplayDataDict.TryGetValue(_currentChoiceLabel, out var slot))
            {
                Transform parentComponent = slot.TryGet<Transform>();
                if(parentComponent != null)
                {
                    foreach (var choice in choices)
                    {
                        GameObject choiceObject = Instantiate(_dialogueChoiceSlot, parentComponent);
                        DialogueChoiceDisplay choiceDisplay = choiceObject.GetComponent<DialogueChoiceDisplay>();
                        if (choiceDisplay == null)
                        {
                           choiceDisplay = choiceObject.AddComponent<DialogueChoiceDisplay>();
                        }

                        choiceDisplay.SetChoiceData(choice);

                        _currentChoiceObjects.Add(choiceObject);
                    }
                }
            }
        }
    }
}