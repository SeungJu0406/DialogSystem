using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Selector : PointHandler
{
    public enum SelectionMode
    {
        Click,
        Hold,
        Toggle
    }

    public enum Selection
    {
        MouseClick,
        Key,
        Both
    }

    public UnityEvent OnSelect;
    public UnityEvent OnDeselect;

    [SerializeField] private bool _isSelected = false;
    [SerializeField] private SelectionMode _selectionMode = SelectionMode.Click;
    [SerializeField] private Selection _selection = Selection.MouseClick;

    private bool _isCurSelected = false;

    private void Awake()
    {
        SelectorManager.OnSelectorChanged += OnSelectorChange;
    }
    private void OnDestroy()
    {
        SelectorManager.OnSelectorChanged -= OnSelectorChange;
    }

    protected override void OnPointClick(PointerEventData eventData)
    {
        if (_selection == Selection.Key) return;
        Select();
    }

    private void OnSelectorChange(Selector selector)
    {
        if (_isCurSelected) return;
        if (selector != this && _isSelected)
        {
            Deselect();
        }
    }

    private void Select()
    {
        _isCurSelected = true;

        SelectorManager.SetSelector(this);
        _isSelected = true;
        OnSelect?.Invoke();

        _isCurSelected = false;
    }
    private void Deselect()
    {
        _isSelected = false;
        OnDeselect?.Invoke();
    }
}
