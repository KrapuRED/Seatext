using UnityEngine;
using System.Collections.Generic;
public class TypeBoxManager : MonoBehaviour
{

    [SerializeField] protected List<TypeBox> _activeTypeBoxs = new List<TypeBox>();

    [Header("Events")]
    [SerializeField] private SetTypeBoxEventSO _setTypeBoxEvent;
    public virtual void CheckTyping(string typedText)
    {
        // This method can be overridden by derived classes to implement specific typing logic
        foreach (TypeBox typeBox in _activeTypeBoxs)
            typeBox.CheckingText(typedText);
    }

    private void SetActiveBox(TypeBox activeTypeBox)
    {
        if (!_activeTypeBoxs.Contains(activeTypeBox))
            {
                _activeTypeBoxs.Add(activeTypeBox);
            }
    }

    private void OnEnable()
    {
        _setTypeBoxEvent.Register(SetActiveBox);
    }

    private void OnDisable()
    {
        _setTypeBoxEvent.Unregister(SetActiveBox);
    }
}
