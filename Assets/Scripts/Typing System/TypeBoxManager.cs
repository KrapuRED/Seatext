using UnityEngine;
using System.Collections.Generic;
public class TypeBoxManager : MonoBehaviour
{
    public static TypeBoxManager instance;

    [SerializeField] protected List<TypeBox> _activeTypeBoxs = new List<TypeBox>();

    [Header("Events")]
    [SerializeField] protected SetTypeBoxEventSO _setTypeBoxEvent;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public virtual void CheckTyping(string typedText)
    {
        // This method can be overridden by derived classes to implement specific typing logic
        foreach (TypeBox typeBox in _activeTypeBoxs)
            typeBox.CheckingText(typedText.ToLower());
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
