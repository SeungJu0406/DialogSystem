using NSJ_Dialogue;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScript", menuName = "Scriptable Objects/Dialogue/Script")]
public class DialogueScript : ScriptableObject
{
    [SerializeField] private bool _isUseCSV = false;
    [SerializeField] private string _CSVFileName = string.Empty;
    [SerializeField] private string _CSVFolderPath = "CSVDatabase";

    [SerializeField] private DialogueHeaderContainer _headerContainer;
    private List<DialogueHeader> _headers => _headerContainer.Headers;

    [SerializeField] private List<DialogueLine> _dialogueLines = new List<DialogueLine>();


    public List<DialogueLine> DialogueLines
    {
        get
        {
            if (_isUseCSV)
            {
                LoadDialogueFromCSV();
            }
            return _dialogueLines;
        }
    }

    [ContextMenu("LoadCSV")]
    private void LoadDialogueFromCSV()
    {
#if UNITY_EDITOR
        string path = Path.Combine(Application.dataPath, _CSVFolderPath);
#else
            string path = $"{Application.streamingAssetsPath}/{_CSVFolderPath}";
#endif
        bool exists = Directory.Exists(path);

        string filePath = Path.Combine(path, _CSVFileName + ".csv");
        string[] files = Directory.GetFiles(path, $"{_CSVFileName}*.csv");

        string dataFile = $"{path}/{_CSVFileName}.csv";

        string file = File.ReadAllText(dataFile);

        string[] lines = file.Split("\r\n");

        string[] headers = new string[] { };

        headers = lines[0].Split(',');

        for (int i = 1; i < lines.Length; i++)
        {
            string[] texts = lines[i].Split(',');

            DialogueLine dialogueLine;

            if (_dialogueLines.Count <= i - 1)
            {
                _dialogueLines.Add(new DialogueLine());
                dialogueLine = _dialogueLines[i - 1];
            }
            else
            {
                dialogueLine = _dialogueLines[i - 1];
            }
            dialogueLine.Initialize();
            for (int t = 0; t < texts.Length; t++)
            {
                if (texts[t] == string.Empty) continue;


                DialogueElementType type = default;
                string headerLabel = string.Empty;
                foreach (DialogueHeader header in _headers)
                {
                    if (header.HeaderLabel == headers[t])
                    {
                        type = header.Type;
                        headerLabel = header.HeaderLabel;
                        break;
                    }
                }

                switch (type)
                {
                    case DialogueElementType.None:
                        continue;
                    case DialogueElementType.ID:
                        dialogueLine.ID = texts[t];
                        break;
                    case DialogueElementType.NextID:
                        dialogueLine.NextID = texts[t];
                        break;
                    case DialogueElementType.Text:
                        DialogueTextElement newTextElement = new DialogueTextElement
                        {
                            Label = headers[t],
                            Text = texts[t]
                        };
                        dialogueLine.TextElementas.Add(newTextElement);
                        break;
                    case DialogueElementType.Image:
                        DialogueImageElement newImageElement = new DialogueImageElement
                        {
                            Label = headers[t],
                            Sprite = texts[t]
                        };
                        dialogueLine.ImageElementas.Add(newImageElement);
                        break;
                    case DialogueElementType.Choice:
                        DialogueChoiceElement newChoiceElement = new DialogueChoiceElement
                        {
                            Label = headers[t],
                            ChoiceText = texts[t],
                        };

                        // ChoiceID 헤더 추적 필요한데 일단 고정위치로
                        newChoiceElement.NextID = texts[t + 1];

                        dialogueLine.ChoiceElementas.Add(newChoiceElement);
                        break;
                }
            }
            _dialogueLines[i - 1] = dialogueLine;
        }
        for (int i = lines.Length - 1; i < _dialogueLines.Count; i++)
        {
            DialogueLine line = _dialogueLines[i];
            line.Initialize();
            _dialogueLines[i] = line;
        }

    }
}
