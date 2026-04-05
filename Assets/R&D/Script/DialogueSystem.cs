using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace NSJ_Dialogue
{
    [System.Serializable]
    public struct DialogueLine
    {
        [SerializeField] private string _id;

        public string ID { get => _id; set { _id = value; } }
        public List<DialogueTextElement> TextElementas;
        public List<DialogueImageElement> ImageElementas;
        public List<DialogueChoiceElement> ChoiceElementas;
        public string NextID;

        [HideInInspector] public bool IsEndLine => ID == string.Empty;

        public void Initialize()
        {
            ID = string.Empty;
            NextID = string.Empty;

            ResetList(ref TextElementas);
            ResetList(ref ImageElementas);
            ResetList(ref ChoiceElementas);
        }
        private void ResetList<T>(ref List<T> list)
        {
            if (list == null)
                list = new List<T>();
            else
                list.Clear();
        }
    }

    [System.Serializable]
    public struct DialogueTextElement
    {
        public string Label;
        public string Text;
    }
    [System.Serializable]
    public struct DialogueImageElement
    {
        public string Label;
        public string Sprite;
    }
    [System.Serializable]
    public struct DialogueChoiceElement
    {
        public string Label;
        public string ChoiceText;
        public string NextID;
    }
    public class DialogueSystem : SingleTon<DialogueSystem>
    {
        DialogueDisplayContainer _displayContainer;

        private List<DialogueLine> _dialogueLines = new List<DialogueLine>();

        private int _dialogueCurrentIndex;
        protected override void InitAwake()
        {
            
        }


        public void SetDisplayContainer(DialogueDisplayContainer displayContainer)
        {
            _displayContainer = displayContainer;
        }


        public void SetDialogue(List<DialogueLine> dialogueLines)
        {
            if (_dialogueLines != null)
            {
                _dialogueLines.Clear();
            }
            
            foreach(var line in dialogueLines)
            {
                _dialogueLines.Add(line);
            }
            _dialogueCurrentIndex = 0;

            ShowDialogue(_dialogueCurrentIndex);
        }

        public void Next()
        {
            _dialogueCurrentIndex++;
            if (_dialogueLines.Count == 0 || _dialogueCurrentIndex >= _dialogueLines.Count || _dialogueLines[_dialogueCurrentIndex].IsEndLine)
            {
                Debug.LogWarning($"Dialogue index out of range");
                _dialogueCurrentIndex--;
                return;
            }

            DialogueLine currentLine = _dialogueLines[_dialogueCurrentIndex];
            if (currentLine.ChoiceElementas.Count > 0)
                return;

            if (currentLine.NextID == string.Empty)
            {
                ShowDialogue(_dialogueCurrentIndex);
            }
            else
            {
                int index = _dialogueLines.FindIndex(line => line.ID == currentLine.NextID);

                if (index < 0)
                {
                    return;
                }

                ShowDialogue(index);
                _dialogueCurrentIndex = index;
            }
        }

        public void Next(string id)
        {
            int index = _dialogueLines.FindIndex(line => line.ID == id);
            ShowDialogue(index);
            _dialogueCurrentIndex = index;
        }


        public void ShowDialogue(int index)
        {
            DialogueLine dialogueLine = _dialogueLines[index];

            if (dialogueLine.ID == string.Empty)
                return;

            ShowDialogueDisplay(dialogueLine);
        }

        public void ShowDialogueDisplay(DialogueLine dialogueLine)
        {
            foreach (var textElement in dialogueLine.TextElementas)
            {
                _displayContainer.SetText(textElement.Label, textElement.Text);
            }
            foreach (var imageElement in dialogueLine.ImageElementas)
            {
                Sprite sprite = ResourcesContainer.GetSprite(imageElement.Sprite);
                _displayContainer.SetImage(imageElement.Label, sprite);
            }

            _displayContainer.SetChoice(dialogueLine.ChoiceElementas);
        }


    }
}