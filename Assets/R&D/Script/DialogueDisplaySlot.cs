using TMPro;
using UnityEngine;

namespace NSJ_Dialogue
{
    public abstract class DialogueDisplaySlot
    {
        protected Animator _animator;
        protected DialogueDisplayType Type;
        public abstract TSlot TryGet<TSlot>() where TSlot : Component;
    }

    public class DialogueDisplaySlot<T> : DialogueDisplaySlot where T : Component
    {
        private T _element;


        public DialogueDisplaySlot(DialogueDisplayData data)
        {
            T element = data.Root.GetComponent<T>();
            _element = element;

            _animator = data.Root.GetComponent<Animator>();
            Type = data.Type;
        }

        public override TSlot TryGet<TSlot>()
        {
            if (!typeof(TSlot).IsAssignableFrom(typeof(T)))
                return null;

            return _element as TSlot;
        }
    }
}