using UnityEngine;
using System.Collections.Generic;

public class TypeBoxManager : MonoBehaviour
{
    public static TypeBoxManager instance;

    [SerializeField] protected List<TypingBox> _activeTypeBoxs = new List<TypingBox>();

    [Header("Events")]
    [SerializeField] protected SetTypeBoxEventSO _setTypeBoxEvent;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void CheckTyping(string typedText)
    {
        // This method can be overridden by derived classes to implement specific typing logic
        List<TypingBox> macthingTypeBox = new List<TypingBox>();

        if (_activeTypeBoxs.Count == 0)
        {
            return;
        }   

        foreach (var typeBox in _activeTypeBoxs)
        {
            if (typeBox.CheckingText(typedText.ToLower()))
            {
                macthingTypeBox.Add(typeBox);
            }
        }

        if (macthingTypeBox.Count == 0)
        {
            ResetAllTypeBox();
        }
    }

    private void SetActiveTypeBox(TypingBox activeTypeBox)
    {
        if (!_activeTypeBoxs.Contains(activeTypeBox))
            {
                _activeTypeBoxs.Add(activeTypeBox);
            }
    }

    public void ResetAllTypeBox()
    {
        foreach (var typeBox in _activeTypeBoxs)
        {
            typeBox.ResetTypeBox();
        }   
    }

    public void RemoveTypeBox(TypingBox typeBox)
    {
        ResetAllTypeBox();
        _activeTypeBoxs.Remove(typeBox);
    }

    private void OnEnable()
    {
        _setTypeBoxEvent.Register(SetActiveTypeBox);
    }

    private void OnDisable()
    {
        _setTypeBoxEvent.Unregister(SetActiveTypeBox);
    }
}
