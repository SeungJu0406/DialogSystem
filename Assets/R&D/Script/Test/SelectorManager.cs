using System;
using UnityEngine;

public static class SelectorManager
{
    public static event Action<Selector> OnSelectorChanged;

    private static Selector _curSelector;


    public static void SetSelector(Selector selector)
    {
        if (_curSelector == selector) return;
        _curSelector = selector;
        OnSelectorChanged?.Invoke(selector);
    }
}
